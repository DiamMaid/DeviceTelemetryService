-- Включение расширения TimescaleDB
CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;

-- Создание гипертаблицы для хранения телеметрических данных
CREATE TABLE IF NOT EXISTS telemetry_data (
    time TIMESTAMPTZ NOT NULL,
    device_id VARCHAR(50) NOT NULL,
    metric_name VARCHAR(100) NOT NULL,
    metric_value DOUBLE PRECISION NOT NULL,
    unit VARCHAR(20),
    tags JSONB,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

-- Преобразование таблицы в гипертaблицу с сегментацией по времени
SELECT create_hypertable('telemetry_data', 'time', chunk_time_interval => INTERVAL '1 day');

-- Индексы для оптимизации запросов
CREATE INDEX IF NOT EXISTS idx_telemetry_device_id ON telemetry_data (device_id, time DESC);
CREATE INDEX IF NOT EXISTS idx_telemetry_metric_name ON telemetry_data (metric_name, time DESC);
CREATE INDEX IF NOT EXISTS idx_telemetry_tags ON telemetry_data USING GIN (tags);

-- Политика удаления старых данных (хранить 90 дней)
SELECT add_retention_policy('telemetry_data', INTERVAL '90 days');

-- Политика сжатия данных (сжимать данные старше 7 дней)
SELECT add_compression_policy('telemetry_data', INTERVAL '7 days');

-- Создание представления для агрегированных данных за последний час
CREATE OR REPLACE VIEW telemetry_last_hour AS
SELECT 
    time_bucket('5 minutes', time) AS bucket,
    device_id,
    metric_name,
    AVG(metric_value) AS avg_value,
    MIN(metric_value) AS min_value,
    MAX(metric_value) AS max_value,
    COUNT(*) AS sample_count
FROM telemetry_data
WHERE time >= NOW() - INTERVAL '1 hour'
GROUP BY bucket, device_id, metric_name
ORDER BY bucket DESC;

-- Таблица устройств
CREATE TABLE IF NOT EXISTS devices (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    device_name VARCHAR(100) NOT NULL UNIQUE,
    device_type VARCHAR(50) NOT NULL,
    location VARCHAR(200),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW()
);

-- Триггер для обновления updated_at
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

CREATE TRIGGER update_devices_updated_at 
    BEFORE UPDATE ON devices 
    FOR EACH ROW 
    EXECUTE FUNCTION update_updated_at_column();
