using System.Text.RegularExpressions;

namespace Dutchskull.Aspire.PolyRepo;

internal static partial class GitUrlUtilities
{
    internal static string GetProjectNameFromGitUrl(string gitUrl) => 
        gitUrl.RemovePostfix(".git").Split('/')[^1];

    internal static bool IsValidGitUrl(string url) =>
        !string.IsNullOrEmpty(url) && GitUrlRegex().IsMatch(url);

    [GeneratedRegex(@"^(?:(?:git|https?):\/\/[^\s]+\.git(?:\/)?|https:\/\/(?:[\w.-]+@)?dev\.azure\.com\/[^\/\s]+\/[^\/\s]+\/_git\/[^\/\s]+(?:\/)?|git@ssh\.dev\.azure\.com:v3\/[^\/\s]+\/[^\/\s]+\/[^\/\s]+(?:\/)?)$")]
    private static partial Regex GitUrlRegex();
}
