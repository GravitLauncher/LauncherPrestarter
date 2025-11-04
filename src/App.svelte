<script lang="ts">
    import "reset-css";
    import { invoke } from "@tauri-apps/api/core";
    import { listen } from "@tauri-apps/api/event";
    import { getCurrentWindow } from "@tauri-apps/api/window";
    import { onMount } from "svelte";

    import "$lib/assets/css/global.scss";

    import { assets } from "$lib/assets";

    import { appConfig, tauriCommands, tauriEvents } from "$lib/config/app";
    import { DownloadTracker } from "$lib/utils/download";
    import type {
        ExtractProgressEvent,
        DownloadProgressEvent,
    } from "$lib/types/events";
    import DownloadBlock from "$lib/components/ui/DownloadBlock.svelte";

    // Состояние приложения
    let lastSpeedUpdate = 0;
    let error = "";
    let speedMb: string = "";
    let percentage: number = 0;
    let totalLabel = "";
    let done = false;
    let running = false;

    const appWindow = getCurrentWindow();
    const downloadTracker = new DownloadTracker();

    // Функции управления окном
    const minimize = () => appWindow.minimize();
    const close_application = () => invoke(tauriCommands.closeApp);

    // Загрузка
    async function startDownload() {
        try {
            console.log("Инициализация загрузки...");
            error = "";
            done = false;
            running = false;
            downloadTracker.reset();
            speedMb = "";

            const result = await invoke<string>(tauriCommands.startDownload);
            console.log("Результат запуска:", result || "успешно");
        } catch (err) {
            console.error("Ошибка инициализации загрузки:", err);
            error = String(err);
            speedMb = "ERR";
        }
    }

    // Добавьте async/await для обработки ошибок
    async function setupListeners() {
        try {
            await listen<DownloadProgressEvent>(
                tauriEvents.downloadProgress,
                (event) => {
                    const now = Date.now();
                    const current = event.payload.downloaded;
                    const total = event.payload.total;

                    // Обновляем через DownloadTracker
                    const result = downloadTracker.update(current, total);

                    speedMb = result.speed;
                    percentage = result.percentage;

                    if (now - lastSpeedUpdate > 200) {
                        totalLabel = downloadTracker.formatTotalLabel(total);
                        lastSpeedUpdate = now;
                    }
                },
            );

            await listen<ExtractProgressEvent>(
                tauriEvents.extractProgress,
                (event) => {
                    speedMb = "--";
                    percentage = downloadTracker.percentageCalculation(
                        event.payload.processed,
                        event.payload.total,
                    );
                    totalLabel = downloadTracker.formatTotalLabel(
                        event.payload.total,
                    );
                },
            );
            await listen<string>(tauriEvents.error, (event) => {
                speedMb = "ERR";
                error = event.payload;
            });
            await listen(tauriEvents.running, () => {
                running = true;
            });
            await listen(tauriEvents.done, () => {
                done = true;
                close_application();
            });
        } catch (err) {
            console.error("Failed to setup listeners:", err);
        }
    }

    onMount(() => {
        setupListeners();
        setTimeout(startDownload, appConfig.download.initialDelay);
    });
</script>

<svelte:head>
    <title>GravitLauncher Prestarter</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
</svelte:head>

<div data-tauri-drag-region class="app">
    <div class="noise"></div>
    <div class="titlebar">
        <div class="controls">
            <button onclick={minimize}>
                <img alt="minimize" src={assets.minimize} />
            </button>
            <button onclick={close_application}>
                <img alt="close" src={assets.close} />
            </button>
        </div>
    </div>
    <div class="layout">
        <div data-tauri-drag-region class="logo-container">
            <img data-tauri-drag-region alt="logo" src={assets.logo} />
            <p data-tauri-drag-region>{appConfig.name}</p>
        </div>

        <DownloadBlock {error} {speedMb} {percentage} {totalLabel} />
    </div>
</div>

<style lang="scss">
    :global(.app) {
        border-radius: 8px;
        width: 100vw;
        height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;
        overflow: hidden;
        background-image: url("lib/assets/images/bg.svg");
        font-family: "Open Sans";
    }
    .layout {
        width: 100%;
    }

    .titlebar {
        user-select: none;
        margin: 0.5em 0.5em 0 0;
        position: fixed;
        top: 0;
        right: 0;

        > .controls {
            display: flex;
            justify-content: flex-end;
        }

        button {
            appearance: none;
            padding: 0;
            margin: 0;
            border: none;
            display: inline-flex;
            justify-content: center;
            align-items: center;
            width: 30px;
            background-color: transparent;
            &:hover {
                cursor: pointer;
                opacity: 0.7;
            }
        }
    }
    .logo-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
        height: 10rem;
        justify-content: flex-start;
        > img {
            display: block;
            width: 7rem;
            height: 7rem;
        }
        > p {
            color: $text-primary;
            font-size: 2.2rem;
            font-weight: 500;
        }
    }
</style>
