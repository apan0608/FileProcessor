using Shared;

namespace FileReader
{
    public interface IFileReader
    {
        FileContent GetFileContent(string filePath, bool hasHeader);
    }
}