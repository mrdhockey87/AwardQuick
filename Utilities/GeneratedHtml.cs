using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.Utilities
{
    public static class GeneratedHtml
    {
        // Add method to load CSS content
        private static async Task<string> LoadCssContentAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("Statements/full.css");
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading CSS file: {ex.Message}");
                return ""; // Return empty string if CSS can't be loaded
            }
        }

        // Add method to load and format statement HTML with CSS
        public static async Task<string> LoadAndFormatStatementHtmlAsync(string filePath)
        {
            try
            {
                string rawContent;

                // Check if the file is compressed (.gz extension)
                if (filePath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
                {
                    rawContent = await ReadDecompressedAsset.ReadDecompressedAssetAsync(filePath);
                }
                else
                {
                    // Load regular HTML file
                    using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);
                    using var reader = new StreamReader(stream);
                    rawContent = await reader.ReadToEndAsync();
                }

                return await WrapHtmlWithFullCss(rawContent);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading HTML file {filePath}: {ex.Message}");
                return GenerateStatementErrorHtml(filePath, ex.Message);
            }
        }

        private static async Task<string> WrapHtmlWithFullCss(string rawHtmlContent)
        {
            var cssContent = await LoadCssContentAsync();

            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Award Quick - Statements & Citations</title>
    <style>
{cssContent}
    </style>
</head>
<body>
{rawHtmlContent}
</body>
</html>";
        }

        private static string GenerateStatementErrorHtml(string filePath, string errorMessage)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Error Loading Statement</title>
    <style>
        body {{
            font-family: Arial, Helvetica, sans-serif;
            background-color: #FFFFFF;
            color: #333333;
            margin: 30px;
            line-height: 150%;
            font-size: 12px;
        }}
        .error-container {{
            background: #fff;
            border: 1px solid #dc3545;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(220, 53, 69, 0.1);
        }}
        .error-title {{ 
            color: #dc3545; 
            text-align: center;
            margin-bottom: 15px;
            font-weight: bold;
            font-size: 18px;
        }}
    </style>
</head>
<body>
    <div class='error-container'>
        <h1 class='error-title'>Error Loading Statement Content</h1>
        <p><strong>File:</strong> {filePath}</p>
        <p><strong>Error:</strong> {errorMessage}</p>
        <p>Please check that the HTML file exists in the correct location.</p>
    </div>
</body>
</html>";
        }

        public static string GenerateLicenseErrorHtml(string title, string message, string details = "", string suggestion = "") => $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #f8f9fa;
        }}
        
        .error-container {{
            background: #fff;
            border: 1px solid #dc3545;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(220, 53, 69, 0.1);
        }}
        
        .error-icon {{
            font-size: 48px;
            text-align: center;
            margin-bottom: 20px;
        }}
        
        .error-title {{
            color: #dc3545;
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .error-message {{
            font-size: 16px;
            margin-bottom: 15px;
            color: #495057;
        }}
        
        .error-details {{
            background: #f8f9fa;
            border-left: 4px solid #dc3545;
            padding: 10px 15px;
            margin: 15px 0;
            font-family: 'Courier New', monospace;
            font-size: 14px;
            white-space: pre-line;
            color: #6c757d;
        }}
        
        .error-suggestion {{
            background: #d1ecf1;
            border: 1px solid #bee5eb;
            border-radius: 5px;
            padding: 10px 15px;
            margin-top: 15px;
            color: #0c5460;
        }}
        
        .timestamp {{
            text-align: center;
            font-size: 12px;
            color: #6c757d;
            margin-top: 20px;
            border-top: 1px solid #dee2e6;
            padding-top: 10px;
        }}
    </style>
</head>
<body>
    <div class='error-container'>
        <div class='error-title'>{title}</div>
        <div class='error-message'>{message}</div>
        {(string.IsNullOrEmpty(details) ? "" : $"<div class='error-details'>{details}</div>")}
        {(string.IsNullOrEmpty(suggestion) ? "" : $"<div class='error-suggestion'><strong> Suggestion:</strong> {suggestion}</div>")}
        <div class='timestamp'>Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</div>
    </div>
</body>
</html>";
        
         public static string StatementGetHtmlFilePath(string contentKey)
         {
             // Map content keys to actual file paths in Resources/Raw
             return contentKey switch
             {
                 // Achievement Statements - Update to use .gz files if compressed
                 "AchievementDeployment" => "Statements/Achievement/Achievement-Deployment.html.gz",
                 "AchievementInspection" => "Statements/Achievement/Achievement-Inspection.html.gz",
                 "AchievementLeadership" => "Statements/Achievement/Achievement-Leadership.html.gz",
                 "AchievementRecognition" => "Statements/Achievement/Achievement-Recognition.html.gz",
                 "AchievementSpecial" => "Statements/Achievement/Achievement-Special.html.gz",
                 "AchievementStaff" => "Statements/Achievement/Achievement-Staff.html.gz",
                 "AchievementVolunteer" => "Statements/Achievement/Achievement-Volunteer.html.gz",
                 "AchievementMiscellaneous" => "Statements/Achievement/Achievement-Misc.html.gz",

                 // Opening Statements
                 "OpeningDeployment" => "Statements/Opening/Opening-Deployment.html.gz",
                 "OpeningInspection" => "Statements/Opening/Opening-Inspection.html.gz",
                 "OpeningLeadership" => "Statements/Opening/Opening-Leadership.html.gz",
                 "OpeningRecognition" => "Statements/Opening/Opening-Recognition.html.gz",
                 "OpeningSpecial" => "Statements/Opening/Opening-Special.html.gz",
                 "OpeningStaff" => "Statements/Opening/Opening-Staff.html.gz",
                 "OpeningVolunteer" => "Statements/Opening/Opening-Volunteer.html.gz",
                 "OpeningMiscellaneous" => "Statements/Opening/Opening-Misc.html.gz",

                 // Helping Statements
                 "HelpingDeployment" => "Statements/Helping/Helping-Deployment.html.gz",
                 "HelpingInspection" => "Statements/Helping/Helping-Inspection.html.gz",
                 "HelpingLeadership" => "Statements/Helping/Helping-Leadership.html.gz",
                 "HelpingRecognition" => "Statements/Helping/Helping-Recognition.html.gz",
                 "HelpingSpecial" => "Statements/Helping/Helping-Special.html.gz",
                 "HelpingStaff" => "Statements/Helping/Helping-Staff.html.gz",
                 "HelpingVolunteer" => "Statements/Helping/Helping-Volunteer.html.gz",
                 "HelpingMiscellaneous" => "Statements/Helping/Helping-Misc.html.gz",

                 // Citations
                 "CitationsDeployment" => "Statements/Citations/Citations-Deployment.html.gz",
                 "CitationsInspection" => "Statements/Citations/Citations-Inspection.html.gz",
                 "CitationsLeadership" => "Statements/Citations/Citations-Leadership.html.gz",
                 "CitationsRecognition" => "Statements/Citations/Citations-Recognition.html.gz",
                 "CitationsSpecial" => "Statements/Citations/Citations-Special.html.gz",
                 "CitationsStaff" => "Statements/Citations/Citations-Staff.html.gz",
                 "CitationsVolunteer" => "Statements/Citations/Citations-Volunteer.html.gz",
                 "CitationsMiscellaneous" => "Statements/Citations/Citations-Misc.html.gz",

                 // Closing Sentences
                 "ClosingSentences" => "Statements/Closing/Closing.html.gz",

                 _ => ""
             };
         }

        public static string StatementGenerateErrorHtml(string mainTab, string nestedTab, string errorMessage)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Error Loading Content</title>
    <style>
        body {{
            font-family: Arial, Helvetica, sans-serif;
            background-color: #FFFFFF;
            color: #333333;
            margin: 30px;
            line-height: 150%;
            font-size: 12px;
        }}
        .error-container {{
            background: #fff;
            border: 1px solid #dc3545;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(220, 53, 69, 0.1);
        }}
        .error-title {{ 
            color: #dc3545; 
            font-weight: bold;
            font-size: 18px;
        }}
        .tab-info {{
            background: #f8f9fa;
            border-left: 4px solid #6c757d;
            padding: 10px 15px;
            margin: 15px 0;
        }}
    </style>
</head>
<body>
    <div class='error-container'>
        <h1 class='error-title'>Error Loading Content</h1>
        <div class='tab-info'>
            <strong>Attempted to load:</strong><br>
            Main Tab: {mainTab}<br>
            Sub Tab: {nestedTab}
        </div>
        <p><strong>Error:</strong> {errorMessage}</p>
        <p>Please check that the HTML file exists in the correct Resources/Raw subdirectory.</p>
    </div>
</body>
</html>";
        }

        public static string StatementDefaultHtml(string contentKey, string _currentMainTab, string _currentNestedTab)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{contentKey}</title>
    <style>
        body {{
            font-family: Arial, Helvetica, sans-serif;
            background-color: #FFFFFF;
            color: #333333;
            margin: 30px;
            line-height: 150%;
            font-size: 12px;
        }}
        .container {{
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        .title {{
            font-family: Arial, Helvetica, sans-serif;
            font-size: 30px;
            line-height: 30px;
            color: #1c222b;
            text-indent: 0;
            font-weight: bold;
        }}
        .tab-info {{
            background: #e8f4fd;
            border-left: 4px solid #2196F3;
            padding: 10px 15px;
            margin: 15px 0;
        }}
        .preference-info {{
            background: #f0f9ff;
            border-left: 4px solid #0ea5e9;
            padding: 10px 15px;
            margin: 15px 0;
            font-size: 14px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1 class='title'>{contentKey}</h1>
        <div class='tab-info'>
            <strong>Current Selection:</strong><br>
            Main Tab: {_currentMainTab}<br>
            Sub Tab: {_currentNestedTab}
        </div>
        <div class='preference-info'>
            <strong>Note:</strong> Your tab selection will be remembered for next time.
        </div>
        <p>Content for <strong>{contentKey}</strong> will be loaded here.</p>
        <p>Please add the corresponding HTML file to <code>Resources/Raw/[appropriate subdirectory]</code>.</p>
    </div>
</body>
</html>";
        }
    }
}