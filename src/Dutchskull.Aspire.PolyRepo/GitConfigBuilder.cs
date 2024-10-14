namespace Dutchskull.Aspire.PolyRepo;

public class GitConfigBuilder
{
    private string? _password;
    private string? _url;
    private string? _username;

    internal GitConfigBuilder WithUrl(string url)
    {
        _url = url;

        return this;
    }

    public GitConfigBuilder WithAuthentication(string username, string password)
    {
        _username = username;
        _password = password;

        return this;
    }

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

        return new GitConfig
        {
            Url = _url,
            Username = _username,
            Password = _password
        };
    }
}