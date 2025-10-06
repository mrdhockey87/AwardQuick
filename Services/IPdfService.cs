using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.Services
{
    public interface IPdfService
    {
        Task OpenPdfFromAssetsAsync(string gzipFileName);
    }
}
