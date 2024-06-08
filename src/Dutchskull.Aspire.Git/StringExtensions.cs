namespace Dutchskull.Aspire.Git;

internal static class StringExtensions
{
    internal static string EndsWith(this string text, string value, Func<string, string> action) =>
        text.EndsWith(value) ? action(text) : text;

    internal static string RemovePostfix(this string text, string postFix) =>
        text.EndsWith(postFix, gitUrl => gitUrl[..^postFix.Length]);
}