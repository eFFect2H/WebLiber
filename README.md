# UniversalCrudLibrary

Универсальная библиотека для быстрого создания CRUD Web-приложений на ASP.NET Core.

## Установка

```bash
dotnet add package UniversalCrudLibrary
```

## Генерация HTML-страницы CRUD из CMD

После установки NuGet-пакета в ASP.NET Core проект можно сгенерировать готовую страницу в `wwwroot` через MSBuild target:

```bash
dotnet msbuild /t:GenerateCrudHtmlPage /p:MyWebLibCrudControllerUrl=https://localhost:5001/api/products
```

Опциональные параметры:

- `MyWebLibCrudOutputFile` — путь к выходному файлу (по умолчанию: `wwwroot/crud-generated.html`)
- `MyWebLibCrudTitle` — заголовок HTML-страницы

Пример:

```bash
dotnet msbuild /t:GenerateCrudHtmlPage ^
  /p:MyWebLibCrudControllerUrl=https://localhost:5001/api/products ^
  /p:MyWebLibCrudOutputFile=wwwroot/products.html ^
  /p:MyWebLibCrudTitle="Products CRUD"
```

Сгенерированный HTML содержит:

- базовые CSS-стили;
- таблицу для вывода данных;
- `input` поля для ID и JSON payload;
- кнопки и JS-функции для `GET`, `POST`, `PUT`, `DELETE`.
