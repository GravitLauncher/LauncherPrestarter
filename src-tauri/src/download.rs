use anyhow::{anyhow, Context, Result};
use serde::Deserialize;
use std::{
    env,
    fs::{self, File},
    io::{Read, Write},
    path::PathBuf,
};
use crate::{config, extract};

#[derive(Debug, Deserialize)]
pub struct JavaRelease {
    pub(crate) downloadUrl: String,
    pub(crate) version: String,
    pub(crate) filename: String,
    pub(crate) size: u64,
}

pub fn download_and_install_java() -> Result<()> {
    let release = fetch_latest_release()?;
    let appdata_dir = get_appdata_dir()?.join("MyJavaDownloader");
    let jdk_dir = appdata_dir.join(format!("JDK-{}", release.version));
    let zip_path = appdata_dir.join(&release.filename);

    if jdk_dir.join("bin/java.exe").exists() {
        println!("Java {} already installed.", release.version);
        return Ok(());
    }

    fs::create_dir_all(&appdata_dir)?;

    //download_file(&release.downloadUrl, &zip_path, release.size)?;
    //extract::extract_zip(&zip_path, &jdk_dir)?;

    config::save_version_info(&release.version)?;
    Ok(())
}

pub fn fetch_latest_release() -> Result<JavaRelease> {
    let url = "https://api.bell-sw.com/v1/liberica/releases?version-modifier=latest&version-feature=24&bitness=64&os=windows&arch=x86&package-type=zip&bundle-type=jre-full";
    let resp = reqwest::blocking::get(url)?.error_for_status()?;
    let releases: Vec<JavaRelease> = serde_json::from_reader(resp)?;

    releases.into_iter().next().ok_or_else(|| anyhow!("No releases found"))
}

pub type ProgressCallback = dyn Fn(u64, u64);

/// Download the file and report progress through the callback.
pub fn download_file(url: &str, dest: &PathBuf, total_size: u64, progress: &ProgressCallback) -> Result<()> {
    let mut response = reqwest::blocking::get(url)?.error_for_status()?;
    let mut file = File::create(dest)?;
    let mut buffer = [0; 8192];
    let mut downloaded: u64 = 0;

    loop {
        let n = response.read(&mut buffer)?;
        if n == 0 {
            break;
        }
        file.write_all(&buffer[..n])?;
        downloaded += n as u64;
        progress(downloaded, total_size);
    }

    Ok(())
}

pub fn get_appdata_dir() -> Result<PathBuf> {
    dirs_next::data_dir().ok_or_else(|| anyhow!("Cannot find AppData directory"))
}