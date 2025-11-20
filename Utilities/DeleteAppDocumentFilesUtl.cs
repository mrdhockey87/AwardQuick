using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AwardQuick.Utilities
{
    public static class DeleteAppDocumentFilesUtl
    {
        // <summary>
        /// Deletes files from the app data directory.
        /// By default deletes files with common temp/asset extensions. Pass allowedExtensions = null to delete all files.
        /// Recurses into subfolders and removes empty directories.
        /// </summary>
        public static Task DeleteAllFilesInAppDocumentsFolderAsync(string[]? allowedExtensions = null, bool includeSubfolders = true)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var appDataPath = FileSystem.AppDataDirectory;
                    if (string.IsNullOrWhiteSpace(appDataPath) || !Directory.Exists(appDataPath))
                        return;

                    // Normalize extensions (lowercase, include dot)
                    string[]? extensions = allowedExtensions?
                        .Where(e => !string.IsNullOrWhiteSpace(e))
                        .Select(e => e.StartsWith('.') ? e.ToLowerInvariant() : "." + e.ToLowerInvariant())
                        .ToArray();

                    var searchOption = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                    var files = Directory.GetFiles(appDataPath, "*", searchOption);

                    foreach (var file in files)
                    {
                        try
                        {
                            if (extensions != null)
                            {
                                var ext = Path.GetExtension(file).ToLowerInvariant();
                                if (!extensions.Contains(ext))
                                    continue; // skip files not in allowed list
                            }

                            // Retry loop for transient locks
                            var attempts = 0;
                            var success = false;
                            while (attempts < 5 && !success)
                            {
                                try
                                {
                                    File.Delete(file);
                                    success = true;
                                    Debug.WriteLine($"Deleted app data file: {file}");
                                }
                                catch (IOException)
                                {
                                    attempts++;
                                    await Task.Delay(150);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    // cannot delete this file; break out
                                    Debug.WriteLine($"Unauthorized deleting file: {file}");
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Failed deleting file '{file}': {ex.Message}");
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error deleting file {file}: {ex.Message}");
                        }
                    }

                    // Optionally remove empty directories left behind
                    if (includeSubfolders)
                    {
                        try
                        {
                            var dirs = Directory.GetDirectories(appDataPath, "*", SearchOption.AllDirectories)
                                                .OrderByDescending(d => d.Length); // delete children first
                            foreach (var dir in dirs)
                            {
                                try
                                {
                                    if (!Directory.EnumerateFileSystemEntries(dir).Any())
                                    {
                                        Directory.Delete(dir);
                                        Debug.WriteLine($"Deleted empty directory: {dir}");
                                    }
                                }
                                catch { /* ignore individual dir errors */ }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error cleaning directories: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"DeleteAllFilesInAppDocumentsFolderAsync error: {ex.Message}");
                }
            });
        }
    }
}