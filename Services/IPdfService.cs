using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.Services
{

    public interface IPdfService
    {
        // Existing: opens the PDF with default viewer
        Task OpenPdfFromAssetsAsync(string gzipFileName);

        // New: materializes the gz asset to a local PDF and returns the full path
        // Accepts either "Examples/foo.pdf.gz" or just "foo.pdf.gz"
        Task<string> MaterializePdfFromAssetsAsync(string packagedGzipPath);
    }
}
