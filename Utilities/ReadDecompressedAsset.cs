using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace AwardQuick.Utilities
{
    public static class ReadDecompressedAsset
    {
        public static async Task<string> ReadDecompressedAssetAsync(string assetName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(assetName);
            using var gzip = new GZipStream(stream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip);
            return await reader.ReadToEndAsync();
        }

        // Existing PDF helper kept for compatibility
        public static async Task<MemoryStream> ReadDecompressedPDFAssetAsync(string assetName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(assetName); // e.g. "file.html.gz"
            using var gzip = new GZipStream(stream, CompressionMode.Decompress);

            var memoryStream = new MemoryStream();
            await gzip.CopyToAsync(memoryStream);

            memoryStream.Position = 0; // reset so consumer reads from start
            return memoryStream;
        }

        // Generic decompression to MemoryStream for any .gz asset (PDF, DOCX, etc.)
        public static async Task<MemoryStream> ReadDecompressedAssetStreamAsync(string assetName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(assetName);
            using var gzip = new GZipStream(stream, CompressionMode.Decompress);

            var memoryStream = new MemoryStream();
            await gzip.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}