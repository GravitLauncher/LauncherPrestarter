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
    pub(crate) packageType: String,
    pub(crate) version: String,
    pub(crate) filename: String,
    pub(crate) size: u64,
}

#[allow(non_snake_case)]
#[derive(Debug, Deserialize)]
struct GitHubTagInfo {
    // tag: String
}

pub fn get_platform_name() -> &'static str {

    #[cfg(target_os = "windows")]
    return "windows";

    #[cfg(target_os = "linux")]
    return "linux";

    #[cfg(target_os = "macos")]
    return "macos";
}

pub fn get_package_type() -> &'static str {

    #[cfg(target_os = "windows")]
    return "zip";

    #[cfg(target_family = "unix")]
    return "tar.gz";
}

pub fn get_arch_name() -> &'static str {

    #[cfg(target_arch = "x86_64")]
    return "x86";

    #[cfg(target_arch = "aarch64")]
    return "aarch64";
}

pub fn get_arch_name_v2() -> &'static str {

    #[cfg(target_arch = "x86_64")]
    return "amd64";

    #[cfg(target_arch = "aarch64")]
    return "aarch64";
}

pub fn fetch_latest_release() -> Result<JavaRelease> {
    fetch_latest_release_from_api().or_else(|_| Ok(fetch_emergency_release()))
}

fn fetch_latest_release_from_api() -> Result<JavaRelease> {
    let url = format!("https://api.bell-sw.com/v1/liberica/releases?version-modifier=latest&version-feature=25&bitness=64&os={}&arch={}&package-type={}&bundle-type=jre-full",
        get_platform_name(), get_arch_name(), get_package_type());
    let resp = reqwest::blocking::get(&url).map_err(|e| {
        anyhow!("Request failed for {}: {:?}", url, e)
    })?.error_for_status()?;
    let releases: Vec<JavaRelease> = serde_json::from_reader(resp)?;

    releases.into_iter().next().ok_or_else(|| anyhow!("No releases found"))
}

fn fetch_emergency_release() -> JavaRelease {
    JavaRelease { downloadUrl: format!("https://github.com/bell-sw/Liberica/releases/download/25+37/bellsoft-jre25+37-{}-{}-full.{}", get_platform_name(),
    get_arch_name_v2(),
    get_package_type()),
    packageType: get_package_type().to_owned(),
    featureVersion: 25, version: "25+37".to_owned(), filename: format!("bellsoft-jre25+37-{}-{}-full.{}", get_platform_name(), get_arch_name_v2(), get_package_type()), size: 117540219 }
}

pub type ProgressCallback = dyn Fn(u64, u64);

/// Download the file and report progress through the callback.
pub fn download_file(url: &str, dest: &PathBuf, total_size: u64, progress: &ProgressCallback) -> Result<()> {
    let mut response = reqwest::blocking::get(url).map_err(|e| {
    anyhow!("Download failed for {}: {:?}", url, e)
})?.error_for_status()?;
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