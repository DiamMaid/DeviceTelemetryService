# Инфраструктура DeviceTelemetryService

Эта папка содержит конфигурационные файлы для запуска всей необходимой инфраструктуры проекта.

## Необходимые компоненты

### 1. Docker Desktop

**Установка на Windows:**

1. Скачайте Docker Desktop с официального сайта: https://www.docker.com/products/docker-desktop/
2. Запустите установочный файл и следуйте инструкциям
3. После установки перезагрузите компьютер
4. Запустите Docker Desktop и дождитесь инициализации

**Проверка установки:**
```bash
docker --version
docker compose version
```

## Запуск инфраструктуры

### Шаг 1: Перейдите в директорию проекта

```bash
cd "D:\Net Core\DeviceTelemetryService"
```

### Шаг 2: Запустите все сервисы

```bash
docker compose up -d
```

Флаг `-d` означает "detached mode" — сервисы запустятся в фоновом режиме.

### Шаг 3: Проверьте статус контейнеров

```bash
docker ps
```

Вы должны увидеть следующие контейнеры со статусом **Up**:
- `device-telemetry-timescaledb` (TimescaleDB)
- `device-telemetry-rabbitmq` (RabbitMQ + Management UI)
- `device-telemetry-redis` (Redis)
- `device-telemetry-prometheus` (Prometheus)
- `device-telemetry-grafana` (Grafana)
- `device-telemetry-jaeger` (Jaeger)

Если какой-то контейнер имеет статус **Restarting** или **Exited**, посмотрите логи:
```bash
docker logs device-telemetry-<название-сервиса>
```

## Доступ к сервисам

| Сервис | URL | Логин | Пароль |
|--------|-----|-------|--------|
| RabbitMQ Management | http://localhost:15672 | telemetry_user | telemetry_password |
| Grafana | http://localhost:3000 | admin | admin |
| Prometheus | http://localhost:9090 | - | - |
| Jaeger UI | http://localhost:16686 | - | - |

## Остановка инфраструктуры

```bash
docker compose down
```

Это остановит и удалит все контейнеры, но сохранит данные в volumes.

## Полная очистка (включая данные)

```bash
docker compose down -v
```

⚠️ **Внимание:** Эта команда удалит все данные из баз данных!

## Что внутри?

### TimescaleDB
- **Порт:** 5432
- **База данных:** telemetry_db
- **Пользователь:** telemetry_user
- **Пароль:** telemetry_password

Содержит:
- Гипертаблицу `telemetry_data` для хранения телеметрических данных
- Таблицу `devices` для регистрации устройств
- Автоматические политики сжатия и удаления старых данных
- Представление `telemetry_last_hour` для агрегированных данных

### RabbitMQ
- **AMQP порт:** 5672
- **Management UI порт:** 15672
- **Пользователь:** telemetry_user
- **Пароль:** telemetry_password

Используется для асинхронной обработки сообщений телеметрии.

### Redis
- **Порт:** 6379

Используется для:
- Кэширования часто запрашиваемых данных
- Хранения скользящих окон (sliding windows)
- Блокировок и координации между экземплярами сервиса

### Prometheus
- **Порт:** 9090

Собирает метрики со всех сервисов. Конфигурация в `infra/prometheus.yml`.

### Grafana
- **Порт:** 3000
- **Логин:** admin
- **Пароль:** admin

Визуализация метрик. Дашборды автоматически загружаются из `infra/grafana/dashboards/`.

### Jaeger
- **UI порт:** 16686
- **OTLP порт:** 4317

Распределенная трассировка для отладки запросов через всю систему.

## Troubleshooting

### Контейнер не запускается

1. Проверьте логи: `docker logs <имя-контейнера>`
2. Убедитесь, что порты не заняты другими приложениями
3. Проверьте, что Docker Desktop запущен

### Порт уже используется

Измените порт в `docker-compose.yml`:
```yaml
ports:
  - "5433:5432"  # Вместо 5432:5432
```

### Данные потерялись после перезапуска

Данные хранятся в Docker volumes. Проверьте их наличие:
```bash
docker volume ls | grep device-telemetry
```

### Медленный первый запуск

При первом запуске Docker скачивает образы из интернета. Это может занять несколько минут. Последующие запуски будут быстрее.

## Следующие шаги

После успешного запуска инфраструктуры:
1. Проверьте доступность RabbitMQ Management UI: http://localhost:15672
2. Войдите в Grafana: http://localhost:3000 (admin/admin)
3. Убедитесь, что дашборд "Device Telemetry Overview" загружен

Готово! Инфраструктура готова к разработке. 🎉
