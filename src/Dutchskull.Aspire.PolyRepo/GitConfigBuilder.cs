namespace Dutchskull.Aspire.PolyRepo;

public class GitConfigBuilder
{
    private string? _url;
    private string? _username;
    private string? _password;

    public GitConfigBuilder SetUrl(string url)
    {
        _url = url;
        return this;
    }

    public GitConfigBuilder SetUsername(string username)
    {
        _username = username;
        return this;
    }

    public GitConfigBuilder SetPassword(string password)
    {
        _password = password;
        return this;
    }

    public GitConfig Build()
    {
        if (string.IsNullOrEmpty(_url))
        {
            throw new InvalidOperationException("Url must be set");
        }

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
