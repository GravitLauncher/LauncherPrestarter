use std::{fs::{self, File}, io::{copy, Write}, path::Path};
use zip::read::ZipArchive;

pub type ProgressCallback = dyn Fn(u64, u64);

/// Extract the zip and report progress.
pub fn extract_zip(zip_path: &Path, dest_dir: &Path, progress: &ProgressCallback) -> std::io::Result<()> {
    let file = File::open(zip_path)?;
    let mut archive = ZipArchive::new(file)?;
    let total_files = archive.len() as u64;

    for i in 0..archive.len() {
        let mut zip_file = archive.by_index(i)?;
        let outpath = dest_dir.join(zip_file.mangled_name());

        if zip_file.name().ends_with('/') {
            fs::create_dir_all(&outpath)?;
        } else {
            if let Some(parent) = outpath.parent() {
                fs::create_dir_all(parent)?;
            }
            let mut outfile = File::create(&outpath)?;
            copy(&mut zip_file, &mut outfile)?;
        }

        progress(i as u64 + 1, total_files);
    }

    Ok(())
}