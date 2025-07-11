// Learn more about Tauri commands at https://tauri.app/develop/calling-rust/

mod config;
mod download;
mod extract;
mod runner;

use download::download_file;
use extract::extract_zip;
use std::{
    fs,
    sync::{atomic::AtomicU64, Arc},
    time::{SystemTime, UNIX_EPOCH},
};
use tauri::Emitter;

use crate::{
    config::target_dir,
    download::fetch_latest_release,
    runner::{java_executable_file, relaunch_using_java},
};

#[tauri::command]
fn start_download(app_handle: tauri::AppHandle) -> Result<(), String> {
    // We'll spawn a background thread
    std::thread::spawn(move || {
        let arc_handle = Arc::new(app_handle);
        let handle = arc_handle.clone();
        let last_update_time = AtomicU64::new(0);
        let emit_progress = move |downloaded: u64, total: u64| {
            let current_time = SystemTime::now()
                .duration_since(UNIX_EPOCH)
                .unwrap()
                .as_millis() as u64;
            // Prevent event spam (300ms cooldown)
            if current_time > last_update_time.load(std::sync::atomic::Ordering::Relaxed) + 300 {
                let _ = handle.emit("download-progress", ProgressEvent { downloaded, total });
            }
        };

        let handle = arc_handle.clone();
        let emit_extract = move |processed: u64, total: u64| {
            let _ = handle.emit("extract-progress", ExtractEvent { processed, total });
        };

        let handle = arc_handle.clone();
        let release = match fetch_latest_release() {
            Ok(e) => e,
            Err(e) => {
                let _ = handle.emit("error", e.to_string());
                return;
            }
        };
        let handle = arc_handle.clone();
        let appdata_dir = match target_dir() {
            Ok(e) => e,
            Err(e) => {
                let _ = handle.emit("error", e.to_string());
                return;
            }
        };
        let jdk_dir = appdata_dir.join(format!("JRE-{}", release.version));
        let zip_path = appdata_dir.join(&release.filename);

        if !java_executable_file(&jdk_dir).exists() {
            let handle = arc_handle.clone();
            match fs::create_dir_all(&appdata_dir) {
                Ok(e) => e,
                Err(e) => {
                    let _ = handle.emit("error", e.to_string());
                    return;
                }
            }

            let handle = arc_handle.clone();
            match config::save_version_info(&release.version) {
                Ok(e) => e,
                Err(e) => {
                    let _ = handle.emit("error", e.to_string());
                    return;
                }
            }

            let handle = arc_handle.clone();
            if let Err(e) = download_file(
                &release.downloadUrl,
                &zip_path,
                release.size,
                &emit_progress,
            ) {
                let _ = handle.emit("error", e.to_string());
                return;
            }

            let handle = arc_handle.clone();
            if let Err(e) = extract_zip(&zip_path, &jdk_dir, &emit_extract) {
                let _ = handle.emit("error", e.to_string());
                return;
            }
        }

        let handle = arc_handle.clone();
        let _ = handle.emit("running", ());

        let handle = arc_handle.clone();
        match relaunch_using_java(&jdk_dir) {
            Ok(e) => e,
            Err(e) => {
                let _ = handle.emit("error", e.to_string());
                return;
            }
        }

        let handle = arc_handle.clone();
        let _ = handle.emit("done", ());
    });

    Ok(())
}

#[tauri::command]
fn close_app(app_handle: tauri::AppHandle) -> Result<(), String> {
    app_handle.exit(0);
    Ok(())
}

#[derive(Clone, serde::Serialize)]
struct ProgressEvent {
    downloaded: u64,
    total: u64,
}

#[derive(Clone, serde::Serialize)]
struct ExtractEvent {
    processed: u64,
    total: u64,
}

#[tauri::command]
fn greet(name: &str) -> String {
    format!("Hello, {}! You've been greeted from Rust!", name)
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    #[cfg(target_family = "unix")]
    {
        std::env::set_var("__GL_THREADED_OPTIMIZATIONS", "0");
        std::env::set_var("__NV_DISABLE_EXPLICIT_SYNC", "1");
    }
    tauri::Builder::default()
        .plugin(tauri_plugin_opener::init())
        .invoke_handler(tauri::generate_handler![greet, start_download, close_app])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
