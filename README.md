# Cloth WebSite

### Необходимые пакеты
- SDK .NET
- PostgreSQL

## Установка и запуск (Linux)

### 1. Клонируте репозиторий 

```git clone https://github.com/NikitaSantalov/WebSite.git```

### 2. Перейдите в папку проекта
```cd WebSite```

### 3. Соберите проект
```dotnet build```

### 4. Скопируйте папки wwwroot и Views в рабочую директорию 
```cp -r wwwroot Views ./bin/Debug/net8.0```

### 5 Перейдите в рабочую директорию
```cd bin/Debug/net8.0```

### 5. Запустите проект
```sudo dotnet WebSite.dll```

## Просмотр конечных точек

### 1. Откройте Swagger
Откройте браузер и вставте в поисковую строку: ```localhost/swagger```
