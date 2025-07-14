<script lang="ts">
  import "reset-css";
  import "$lib/css/global.css";
  import logo from "$lib/images/logo.svg";
  import { invoke } from "@tauri-apps/api/core";
  import { listen, type EventCallback } from "@tauri-apps/api/event";
  import ProgressBar from "$lib/ProgressBar.svelte";

  let downloadProgress = 0;
  let downloadTotal = 1;
  let error = "";
  let speedMb: string | number = 0;
  let totalLabel = "";
  let done = false;
  let running = false;

  let lastProgress = 0;
  let lastTimestamp = Date.now();

  // A buffer to store the last few speed samples for smoothing
  const speedSamples: number[] = [];
  const MAX_SAMPLES = 30; // adjust for more/less smoothing

  async function startDownload() {
    error = "";
    done = false;
    lastProgress = 0;
    lastTimestamp = Date.now();
    speedSamples.length = 0; // clear previous samples
    speedMb = 0;
    await invoke("start_download");
  }

  function close_application() {
    invoke("close_app");
  }

  function updateSpeed(currentProgress: number) {
    const now = Date.now();
    const elapsedMs = now - lastTimestamp;


    if (elapsedMs <= 0) return;

    const bytesDownloaded = currentProgress - lastProgress;
    const secondsElapsed = elapsedMs / 1000;

    const speedBps = bytesDownloaded / secondsElapsed; // bytes per second
    const speedMbps = (speedBps * 8) / (1024 * 1024); // convert to megabits

    // Update rolling average buffer
    speedSamples.push(speedMbps);
    if (speedSamples.length > MAX_SAMPLES) {
      speedSamples.shift(); // remove oldest sample
    }

    // Smooth speed using average
    const sum = speedSamples.reduce((a, b) => a + b, 0);
    speedMb = +(sum / speedSamples.length).toFixed(0);

    // Prepare for next tick
    lastProgress = currentProgress;
    lastTimestamp = now;

  }

  listen<any>("download-progress", (event) => {
    const current = event.payload.downloaded;
    downloadProgress = current;
    downloadTotal = event.payload.total;
    totalLabel = (downloadTotal / 1000 / 1000).toFixed(0) + " MB";
    updateSpeed(current);
  });

  listen<any>("extract-progress", (event) => {
    const current = event.payload.processed;
    downloadProgress = current;
    downloadTotal = event.payload.total;
    speedMb = "--";
    totalLabel = downloadProgress + " / " + downloadTotal;
  });

  listen<any>("error", (event) => {
    speedMb = "ERR";
    error = event.payload;
    console.log(error);
  });

  listen("running", () => {
    running = true;
  });

  listen("done", () => {
    done = true;
    close_application();
  });

  setTimeout(() => {
    startDownload();
  }, 300);
</script>

<div class="layout">
  <div class="logo-container">
    <img class="logo" alt="logo" src={logo} />
    <p class="description">GravitLauncher</p>
  </div>

  <div class="download-block">
    <div class="speed-block">
      <div>
        <font class="speed">{speedMb}</font>
        <font class="speed-label">MB/S</font>
      </div>
      <font class="total-label">{{ totalLabel }}</font>
    </div>

    <ProgressBar value={downloadProgress} total={downloadTotal} />
  </div>
</div>
