use anyhow::{anyhow, Result};
use chrono::{DateTime, Duration, Utc};
use serde::{Deserialize, Serialize};
use std::{fs, path::PathBuf};

#[derive(Debug, Serialize, Deserialize)]
pub struct Config {
    pub java_version: String,
    pub java_feature_version: u32,
    pub install_date: DateTime<Utc>,
}

pub fn save_version_info(version: &str, feature_version : u32) -> Result<()> {
    let config = Config {
        java_version: version.to_string(),
        java_feature_version: feature_version,
        install_date: chrono::Utc::now(),
    };
    let path = config_path()?;
    fs::create_dir_all(path.parent().unwrap())?;
    let content = serde_json::to_string_pretty(&config)?;
    fs::write(path, content)?;
    Ok(())
}

pub fn load_version_info() -> Result<Option<Config>> {
    let path = config_path()?;
    if !path.exists() {
        return Ok(None);
    }
    let json = fs::read_to_string(path)?;
    let config: Config = serde_json::from_str(&json)?;
    Ok(Some(config))
}

pub fn is_java_outdated(metadata: &Config) -> bool {
    let age = chrono::Utc::now() - metadata.install_date;
    age > Duration::days(30)
}


pub fn get_appdata_dir() -> Result<PathBuf> {
    dirs_next::data_dir().ok_or_else(|| anyhow!("Cannot find AppData directory"))
}

pub fn target_dir() -> Result<PathBuf> {
    Ok(get_appdata_dir()?.join("GravitLauncherStore"))
}

fn config_path() -> Result<PathBuf> {
    Ok(target_dir()?.join("prestarter-config.json"))
}