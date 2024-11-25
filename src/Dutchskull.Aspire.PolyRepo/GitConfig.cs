namespace Dutchskull.Aspire.PolyRepo;

public record GitConfig
{
    internal GitConfig()
    {
    }

    public required string Url { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }

    public string[] CustomHeaders { get; init; } = [];
}