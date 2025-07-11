use anyhow::Result;
use std::{env, path::PathBuf, process::Command};

pub fn relaunch_using_java() -> Result<()> {
    let appdata = dirs_next::data_dir().unwrap().join("MyJavaDownloader");
    let java_exe = appdata.join("JDK-24/bin/java.exe");

    let current_exe = env::current_exe()?;
    println!("Re-launching with: {} -jar {}", java_exe.display(), current_exe.display());

    Command::new(java_exe)
        .arg("-jar")
        .arg(current_exe)
        .spawn()?; // not waiting intentionally

    Ok(())
}