@echo off
echo ========================================
echo DeviceTelemetryService Infrastructure
echo ========================================
echo.

REM Проверяем Docker
echo [1/3] Checking Docker...
docker --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Docker not found!
    echo Please install Docker Desktop from: https://www.docker.com/products/docker-desktop/
    pause
    exit /b 1
)
echo OK: Docker is available
echo.

REM Переходим в директорию проекта
cd /d "%~dp0"

REM Запускаем инфраструктуру
echo [2/3] Starting infrastructure containers...
docker compose up -d
if %errorlevel% neq 0 (
    echo ERROR: Failed to start containers!
    pause
    exit /b 1
)
echo OK: Containers started
echo.

REM Проверяем статус
echo [3/3] Checking container status...
docker compose ps
echo.

echo ========================================
echo Infrastructure is ready!
echo ========================================
echo.
echo Services available at:
echo   - RabbitMQ Management: http://localhost:15672 (telemetry_user / telemetry_password)
echo   - Grafana: http://localhost:3000 (admin / admin)
echo   - Prometheus: http://localhost:9090
echo   - Jaeger UI: http://localhost:16686
echo.
pause
