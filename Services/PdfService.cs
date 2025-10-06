using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.Services
{
    public class PdfService : IPdfService
    {
        private string _lastTempFile;

        public async Task OpenPdfFromAssetsAsync(string gzipFileName)
        {
            try
            {
                // Clean up previous temp file if exists
                if (!string.IsNullOrEmpty(_lastTempFile) && File.Exists(_lastTempFile))
                {
                    try { File.Delete(_lastTempFile); } catch { }
                }

                // Read and decompress
                using var compressedStream = await FileSystem.OpenAppPackageFileAsync($"Examples/{gzipFileName}");
                using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
                using var decompressedStream = new MemoryStream();
                await gzipStream.CopyToAsync(decompressedStream);
                decompressedStream.Position = 0;

                // Save to temp
                var pdfFileName = Path.GetFileNameWithoutExtension(gzipFileName) + ".pdf";
                var tempFilePath = Path.Combine(FileSystem.CacheDirectory, pdfFileName);

                using (var fileStream = File.Create(tempFilePath))
                {
                    await decompressedStream.CopyToAsync(fileStream);
                }

                _lastTempFile = tempFilePath;

                // Open with default viewer
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(tempFilePath),
                    Title = "Open PDF"
                });
            }
            catch (Exception ex)
            {
                await Utilities.OpenPDFFile.ShowAlert("Error", $"Could not open PDF: {ex.Message}", "OK");
            }
        }
    }
}
