# Настройка MCP Server для DeviceTelemetryService

## Обзор

MCP (Model Context Protocol) сервер `device-telemetry-orchestrator` предоставляет мульти-агентную архитектуру для разработки проекта DeviceTelemetryService.

## Архитектура агентов

### 1. Lead Orchestrator & Fast Coder
- **Модель:** qwen3.8-flash
- **Роль:** Быстрая кодогенерация CRUD, DTO, контроллеров
- **Инструменты:** read_file, write_file, list_directory, execute_command

### 2. Architecture & Deep Logic
- **Модель:** deepseek-v4-pro
- **Роль:** Проектирование Clean Architecture, валидация слоев
- **Инструменты:** sub-agent, reasoning-tool

### 3. Test & QA Engineer
- **Модель:** deepseek-v4.1-flash
- **Роль:** Unit-тесты, интеграционные тесты с Testcontainers
- **Инструменты:** run_tests, read_test_logs, generate_mocks

### 4. DevOps, Infrastructure & Telemetry
- **Модель:** glm-5.3
- **Роль:** Docker, MassTransit, OpenTelemetry, миграции
- **Инструменты:** docker_cli, read_logs, validate_yaml

## Расположение сервера

```
C:\Users\diman\.qwen\mcp-servers\device-telemetry-orchestrator\
├── index.js              # Основной файл сервера
├── package.json          # Зависимости
├── package-lock.json     # Lock файл зависимостей
└── README.md             # Документация
```

## Конфигурация

Конфигурация MCP сервера находится в `.qwen/mcp.json`:

```json
{
  "mcpServers": {
    "device-telemetry-orchestrator": {
      "command": "node",
      "args": ["C:\\Users\\diman\\.qwen\\mcp-servers\\device-telemetry-orchestrator\\index.js"],
      "env": {
        "PROJECT_ROOT": "D:\\Net Core\\DeviceTelemetryService"
      }
    }
  }
}
```

## Установка и запуск

### Установка зависимостей

```bash
cd C:\Users\diman\.qwen\mcp-servers\device-telemetry-orchestrator
npm install
```

### Проверка работы

```bash
cd C:\Users\diman\.qwen\mcp-servers\device-telemetry-orchestrator
echo {} | node index.js
```

Ожидаемый вывод:
```
MCP Server device-telemetry-orchestrator запущен
```

## Доступные инструменты

### get_agents
Получить список всех агентов проекта.

**Пример использования:**
```javascript
// Через MCP клиент
await callTool('get_agents', {});
```

### get_agent_info
Получить информацию о конкретном агенте.

**Параметры:**
- `agentId`: 'orchestrator' | 'architect' | 'tester' | 'devops'

### create_crud
Сгенерировать CRUD операции для сущности.

**Параметры:**
- `entityName`: имя сущности
- `properties`: массив свойств

### build_project
Запустить сборку проекта dotnet build.

## Troubleshooting

### Ошибка "Cannot find module '@modelcontextprotocol/sdk'"

**Решение:** Убедитесь, что зависимости установлены в правильной директории:
```bash
cd C:\Users\diman\.qwen\mcp-servers\device-telemetry-orchestrator
npm install @modelcontextprotocol/sdk
```

### Сервер не запускается

**Проверка:**
1. Node.js установлен и доступен в PATH
2. Зависимости установлены (`node_modules` существует)
3. Пути в `index.js` корректны (без `/dist/` префикса)

## Версии

- **@modelcontextprotocol/sdk:** 0.5.0
- **Node.js:** v24.21.0+
- **npm:** latest

## Ссылки

- [GitHub репозиторий](https://github.com/DiamMaid/DeviceTelemetryService)
- [MCP Protocol Documentation](https://modelcontextprotocol.io/)
