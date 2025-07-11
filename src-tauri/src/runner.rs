use anyhow::Result;
use std::{env, path::PathBuf, process::Command};

pub fn java_executable_file(java_dir: &PathBuf) -> PathBuf {

    #[cfg(target_family = "windows")]
    let java_exe = java_dir.join("bin/java.exe");

    #[cfg(target_family = "unix")]
    let java_exe = java_dir.join("bin/java");

    java_exe
}

pub fn relaunch_using_java(java_dir: &PathBuf) -> Result<()> {
    let java_exe = java_executable_file(java_dir);

    let current_exe = env::current_exe()?;

    Command::new(java_exe)
        .arg("-Dlauncher.noJavaCheck=true")
        .arg("-jar")
        .arg(current_exe)
        .spawn()?; // not waiting intentionally

    Ok(())
}