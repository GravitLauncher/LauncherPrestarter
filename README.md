# LauncherPrestarter

Это престартер для GravitLauncher, написанный на языке Rust с использованием [tauri](https://v2.tauri.app/)

## Клонирование репозитория

```bash
git clone -b rust/5.7.x https://github.com/GravitLauncher/LauncherPrestarter.git
```

## Подготовка окружения (Windows)

- Установите [Visual Studio](https://visualstudio.microsoft.com/) (не Vistal Studio Code) с компонентом "Разработка приложений на C++"
- Следуйте [инструкции](https://rust-lang.org/tools/install/) и по установке окружения для разработки на Rust
- Установите [NodeJS](https://nodejs.org/en/download/current)
- Установите yarn с помощью npm
```bash
npm install --global yarn
```
- Откройте папку с престартером в консоли и выполните следующую команду:
```
yarn
```

## Отладка и сборка

Выполните `yarn tauri dev` что бы запустить престартер в режиме отладки. Престартер всегда в таком случае будет показывать окно скачки (для удобства отладки). Если вам необходимо что бы престартер не начинал скачивание Java, закомментируйте строчку `setTimeout(startDownload, appConfig.download.initialDelay);` в `src/App.svelte`. Не забудьте потом вернуть эту строчку обратно!

Выполните `yarn tauri build` для сборки итогового exe файла. Он будет лежать в `src-tauri/target/release`

## Редактирование дизайна

### Настройка IDE

[VS Code](https://code.visualstudio.com/) + [Svelte](https://marketplace.visualstudio.com/items?itemName=svelte.svelte-vscode) + [Tauri](https://marketplace.visualstudio.com/items?itemName=tauri-apps.tauri-vscode) + [rust-analyzer](https://marketplace.visualstudio.com/items?itemName=rust-lang.rust-analyzer).

### Архитектура проекта

В папке `src` находится исходный код фронтенда(по сути, проект на Svelte который собирается в html/css/js с помощью vite)

В папке `src-tauri` находится исходный код бекенда(явдяющийся Rust приложением)

Полезные ссылки:

- [Svelte](https://svelte.dev/)
- [Tauri](https://v2.tauri.app/)
- [Rust](https://rust-lang.org/)
- [CSS](https://developer.mozilla.org/en-US/docs/Web/CSS)
- [HTML](https://developer.mozilla.org/en-US/docs/Web/HTML)
- [JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript)

### Смена иконки

Логотип, отображаемый внутри приложения находится в `src/lib/assets/images/logo.svg`

Для замены лого в панели задач выполните команду

```bash
yarn tauri icon PATH_TO_ICON_PNG
```
