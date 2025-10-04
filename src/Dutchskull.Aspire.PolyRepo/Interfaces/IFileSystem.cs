namespace Dutchskull.Aspire.PolyRepo.Interfaces;

public interface IFileSystem
{
    bool DirectoryExists(string path);

    bool FileExists(string path);

    bool FileOrDirectoryExists(string path);
}