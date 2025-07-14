use anyhow::{anyhow, Result};
use serde::Deserialize;
use std::{
    fs::{File},
    io::{Read, Write},
    path::PathBuf,
};

// BellSoft API
#[allow(non_snake_case)]
#[derive(Debug, Deserialize)]
pub struct JavaRelease {
    pub(crate) downloadUrl: String,
    pub(crate) featureVersion: u32,
    pub(crate) version: String,
    pub(crate) filename: String,
    pub(crate) size: u64,
}

#[allow(non_snake_case)]
#[derive(Debug, Deserialize)]
struct GitHubTagInfo {
    tag: String
}

pub fn fetch_latest_release() -> Result<JavaRelease> {
    fetch_latest_release_from_api().or_else(|_| Ok(fetch_emergency_release()))
}

fn fetch_latest_release_from_api() -> Result<JavaRelease> {
    let url = "https://api.bell-sw.com/v1/liberica/releases?version-modifier=latest&version-feature=24&bitness=64&os=windows&arch=x86&package-type=zip&bundle-type=jre-full";
    let resp = reqwest::blocking::get(url)?.error_for_status()?;
    let releases: Vec<JavaRelease> = serde_json::from_reader(resp)?;

    releases.into_iter().next().ok_or_else(|| anyhow!("No releases found"))
}

fn fetch_emergency_release() -> JavaRelease {
    JavaRelease { downloadUrl: "https://github.com/bell-sw/Liberica/releases/download/24.0.1+11/bellsoft-jre24.0.1+11-windows-amd64-full.zip".to_owned(),
    featureVersion: 24, version: "24.0.1+11".to_owned(), filename: "bellsoft-jre24.0.1+11-windows-amd64-full.zip".to_owned(), size: 115975373 }
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