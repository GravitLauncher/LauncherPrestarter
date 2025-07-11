use anyhow::{Context, Result};
use serde::{Deserialize, Serialize};
use std::{fs, path::PathBuf};

#[derive(Debug, Serialize, Deserialize)]
pub struct Config {
    pub java_version: String,
}

pub fn save_version_info(version: &str) -> Result<()> {
    let config = Config {
        java_version: version.to_string(),
    };
    let path = config_path()?;
    fs::create_dir_all(path.parent().unwrap())?;
    let content = serde_json::to_string_pretty(&config)?;
    fs::write(path, content)?;
    Ok(())
}

fn config_path() -> Result<PathBuf> {
    let dir = dirs_next::data_dir().ok_or_else(|| anyhow::anyhow!("No AppData found"))?;
    Ok(dir.join("MyJavaDownloader/config.json"))
}