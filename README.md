# LauncherPrestarter
Установщик Java и GravitLauncher для вашего проекта, написанный на .NET
### Совместимость
Установщик требует .NET 4.5.1 и выше, который предустановлен на Windows 8.1/10/11. Установить эту версию можно на Windows 7 и выше. Многие сборки Windows уже содержат в себе .NET Framework подходящей версии  
Prestarter_module работает только с GravitLauncher 5.5.0+
### Сборка на Windows
- Установите Visual Studio 2022 и откройте проект
- Откройте `PrestarterForm.cs`
- В конструкторе измените дизайн, надписи и логотип под ваш проект
- Откройте файл `Config.cs`
- Настройте обязательные параметры: `Project`(название вашего проекта как в лаунчсервере) и `LauncherUrl`(ссылку на ваш лаунчер как в лаунчсервере)  
- Выберите тип сборки Release  и соберите проект комбинацоей клавиш `Ctrl+Shift+B
### Сборка модуля на Windows
- Перейдите в каталог `Prestarter_module`
- Откройте терминал в папке в которую мы зашли ранее
- Запустите сборку командой `gradlew.bat build`
- Собранный файл вы найдете в `build\libs`

### Сборка на Linux
- Установите `mono-msbuild`
- Откройте файл `Config.cs`
- Настройте обязательные параметры: `Project`(название вашего проекта как в лаунчсервере)
- Запустите сборку командой `msbuild -p:Configuration=Release`
### Сборка модуля на Linux
- Перейдите в каталог `Prestarter_module`
- Запустите сборку командой `./gradlew build`
- Собранный файл вы найдете в `build\libs`

### Установка на LaunchServer
Для использования Prestarter выполните следующие действия:
- Установите модуль `Prestarter_module.jar` на лаунчсервер в папку `modules`
- Соберите проект с помощью Visual Studio(Windows) или msbuild(Linux)
- Поместите собранный файл в корень лаунчсервера с названием `Prestarter.exe`

### Prestarter как отдельный файл (Обновляться у пользователей не способен)
- В `LauncherUrl` впишите ссылку на файл `Launcher.jar` в конфиге `Prestarter\Config.cs`
  - Пример для разработки:
  ```cs
     public static string LauncherDownloadUrl = "http://127.0.0.1:9274/Launcher.jar"
  ```
- Установите модуль `Prestarter_module.jar` на лаунчсервер в папку `modules`
- Соберите проект с помощью Visual Studio(Windows) или msbuild(Linux)