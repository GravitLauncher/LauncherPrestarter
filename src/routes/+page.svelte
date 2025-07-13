<script lang="ts">
  import "reset-css";
  import "$lib/css/global.css";
  import logo from "$lib/images/logo.svg";
  import { invoke } from "@tauri-apps/api/core";
  import { listen, type EventCallback } from "@tauri-apps/api/event";

  let downloadProgress = 0.4;
  let downloadTotal = 1;
  let extractProgress = 0;
  let extractTotal = 1;
  let error = "";
  let done = false;
  let running = false;

  function startDownload() {
    error = "";
    done = false;
    invoke("start_download");
  }

  function close_application() {
    invoke("close_app");
  }

  listen<any>("download-progress", (event) => {
    downloadProgress = event.payload.downloaded;
    downloadTotal = event.payload.total;
  });

  listen<any>("extract-progress", (event) => {
    downloadProgress = event.payload.processed;
    downloadTotal = event.payload.total;
  });

  listen<any>("error", (event) => {
    error = event.payload;
  });

  listen("running", () => {
    running = true;
  });

  listen("done", () => {
    done = true;
    close_application();
  });

  startDownload();
</script>

<div class="layout">
  <div class="logo-container">
    <img class="logo" alt="logo" src={logo} />
    <p class="description">GravitLauncher</p>
  </div>

  <div class="download-block">
    <div class="speed-block">
      <div>
       <font class="speed">214</font>
       <font class="speed-label">MB/S</font>
      </div>
      <font class="total-label">100 MB</font>
    </div>
    <progress class="progress-bar" value={downloadProgress} max={downloadTotal}></progress>
  </div>
</div>