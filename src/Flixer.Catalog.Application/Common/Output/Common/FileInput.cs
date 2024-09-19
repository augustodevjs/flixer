namespace Flixer.Catalog.Application.Common.Output.Common;

public class FileInput
{
    public string Extension { get; private set; }
    public Stream FileStream { get; private set; }
    public string ContentType { get; private set; }

    public FileInput(string extension, Stream fileStream, string contentType)
    {
        Extension = extension;
        FileStream = fileStream;
        ContentType = contentType;
    }
}