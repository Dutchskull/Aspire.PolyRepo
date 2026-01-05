namespace Dutchskull.Aspire.PolyRepo;

public class GitConfigBuilder
{
    private string[]? _customHeaders;
    private string? _password;
    private string _url = string.Empty;
    private string? _username;

    public GitConfig Build()
    {
        if (string.IsNullOrEmpty(_username))
        {
            _username = string.Empty;
        }

        if (string.IsNullOrEmpty(_password))
        {
            _password = string.Empty;
        }

        _customHeaders ??= [];

        return new GitConfig
        {
            Url = _url,
            Username = _username,
            Password = _password,
            CustomHeaders = _customHeaders
        };
    }

    public GitConfigBuilder WithAuthentication(string username, string password)
    {
        _username = username;
        _password = password;

        return this;
    }

    public GitConfigBuilder WithCustomHeaders(params string[] headers)
    {
        _customHeaders = headers;
        return this;
    }

    internal GitConfigBuilder WithUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentNullException(nameof(url), "URL must not be null, expected a valid git url.");
        }

        _url = url;
        return this;
    }
}