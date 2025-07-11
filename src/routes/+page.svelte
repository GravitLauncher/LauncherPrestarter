<script lang="ts">
  import { invoke } from "@tauri-apps/api/core";
  import { listen, type EventCallback } from "@tauri-apps/api/event";

  let downloadProgress = 0;
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
    extractProgress = event.payload.processed;
    extractTotal = event.payload.total;
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
</script>

<button on:click={startDownload}>Start Download</button>

{#if error}
  <p style="color: red">Error: {error}</p>
{/if}

<h3>Download Progress: {downloadProgress} / {downloadTotal}</h3>
<progress value={downloadProgress} max={downloadTotal}></progress>

<h3>Extract Progress: {extractProgress} / {extractTotal}</h3>
<progress value={extractProgress} max={extractTotal}></progress>

{#if running && !done}
  <p>Start GravitLauncher</p>
{/if}

{#if done}
  <p>Complete</p>
{/if}
