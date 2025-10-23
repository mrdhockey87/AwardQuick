using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.Utilities
{
    public static class ServiceHelper
    {
        public static IServiceProvider Services =>
            Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Service provider not initialized yet.");

        public static T GetService<T>() where T : notnull =>
            Services.GetRequiredService<T>();
    }
}
