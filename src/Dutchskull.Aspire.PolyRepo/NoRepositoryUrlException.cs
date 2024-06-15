namespace Dutchskull.Aspire.PolyRepo;

[Serializable]
internal class NoRepositoryUrlException : Exception
{
    public NoRepositoryUrlException()
    {
    }

    public NoRepositoryUrlException(string? message) : base(message)
    {
    }

    public NoRepositoryUrlException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}