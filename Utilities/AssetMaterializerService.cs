using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Maui.ApplicationModel;

namespace AwardQuick.Utilities
{
    public class AssetMaterializerService
    {
        /// <summary>
        /// Materialize an asset from the app package into the AppDataDirectory.
        /// Supports plain files (e.g. "Forms/foo.pdf") and compressed files ending with ".gz"
        /// (e.g. "Forms/foo.pdf.gz" or "Forms/foo.docx.gz").
        /// Returns the full destination path.
        /// </summary>
        public async Task<string> MaterializeAssetAsync(string packagedPath)
        {
            if (string.IsNullOrWhiteSpace(packagedPath))
                throw new ArgumentException("packagedPath is null or empty", nameof(packagedPath));

            // Compute destination filename (remove single trailing ".gz" if present)
            var fileName = packagedPath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase)
                ? Path.GetFileName(packagedPath[..^3])
                : Path.GetFileName(packagedPath);

            // Ensure extension exists; leave as-is otherwise
            if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)))
                fileName = Path.ChangeExtension(fileName, ".dat");

            var destPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            // Overwrite existing file
            if (packagedPath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
            {
                // Decompress stream and write to dest
                using var ms = await ReadDecompressedAsset.ReadDecompressedAssetStreamAsync(packagedPath);
                ms.Position = 0;
                await using var outFs = File.Create(destPath);
                ms.Position = 0;
                await ms.CopyToAsync(outFs);
            }
            else
            {
                // Copy raw package file to dest
                using var src = await FileSystem.OpenAppPackageFileAsync(packagedPath);
                src.Position = 0;
                await using var outFs = File.Create(destPath);
                await src.CopyToAsync(outFs);
            }

            return destPath;
        }

        /// <summary>
        /// Open the provided file path with the system default app.
        /// </summary>
        public async Task OpenWithDefaultAppAsync(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath) || !File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            var file = new ReadOnlyFile(fullPath);
            await Launcher.OpenAsync(new OpenFileRequest { File = file, Title = Path.GetFileName(fullPath) });
        }
    }
}