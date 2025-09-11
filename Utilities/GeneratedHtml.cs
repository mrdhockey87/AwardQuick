using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwardQuick.Utilities
{
    public static class GeneratedHtml
    {

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
                // Achievement Statements
                "AchievementDeployment" => "Statements/Achievement/Achievement-Deployment.html",
                "AchievementInspection" => "Statements/Achievement/Achievement-Inspection.html",
                "AchievementLeadership" => "Statements/Achievement/Achievement-Leadership.html",
                "AchievementRecognition" => "Statements/Achievement/Achievement-Recognition.html",
                "AchievementSpecial" => "Statements/Achievement/Achievement-Special.html",
                "AchievementStaff" => "Statements/Achievement/Achievement-Staff.html",
                "AchievementVolunteer" => "Statements/Achievement/Achievement-Volunteer.html",
                "AchievementMiscellaneous" => "Statements/Achievement/Achievement-Misc.html",

                // Opening Statements
                "OpeningDeployment" => "Statements/Opening/Opening-Deployment.html",
                "OpeningInspection" => "Statements/Opening/Opening-Inspection.html",
                "OpeningLeadership" => "Statements/Opening/Opening-Leadership.html",
                "OpeningRecognition" => "Statements/Opening/Opening-Recognition.html",
                "OpeningSpecial" => "Statements/Opening/Opening-Special.html",
                "OpeningStaff" => "Statements/Opening/Opening-Staff.html",
                "OpeningVolunteer" => "Statements/Opening/Opening-Volunteer.html",
                "OpeningMiscellaneous" => "Statements/Opening/Opening-Misc.html",

                // Helping Statements
                "HelpingDeployment" => "Statements/Helping/Helping-Deployment.html",
                "HelpingInspection" => "Statements/Helping/Helping-Inspection.html",
                "HelpingLeadership" => "Statements/Helping/Helping-Leadership.html",
                "HelpingRecognition" => "Statements/Helping/Helping-Recognition.html",
                "HelpingSpecial" => "Statements/Helping/Helping-Special.html",
                "HelpingStaff" => "Statements/Helping/Helping-Staff.html",
                "HelpingVolunteer" => "Statements/Helping/Helping-Volunteer.html",
                "HelpingMiscellaneous" => "Statements/Helping/Helping-Misc.html",

                // Citations
                "CitationsDeployment" => "Statements/Citations/Citations-Deployment.html",
                "CitationsInspection" => "Statements/Citations/Citations-Inspection.html",
                "CitationsLeadership" => "Statements/Citations/Citations-Leadership.html",
                "CitationsRecognition" => "Statements/Citations/Citations-Recognition.html",
                "CitationsSpecial" => "Statements/Citations/Citations-Special.html",
                "CitationsStaff" => "Statements/Citations/Citations-Staff.html",
                "CitationsVolunteer" => "Statements/Citations/Citations-Volunteer.html",
                "CitationsMiscellaneous" => "Statements/Citations/Citations-Misc.html",

                // Closing Sentences
                "ClosingSentences" => "Statements/Closing/Closing.html",

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
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            line-height: 1.6;
            color: #333;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .error-container {{
            background: #fff;
            border: 1px solid #dc3545;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(220, 53, 69, 0.1);
        }}
        .error-title {{ color: #dc3545; }}
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
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            line-height: 1.6;
            color: #333;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .container {{
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        h1 {{ color: #2c3e50; }}
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
        <h1>{contentKey}</h1>
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
