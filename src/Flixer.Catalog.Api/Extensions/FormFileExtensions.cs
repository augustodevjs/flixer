using Flixer.Catalog.Application.Common.Output.Common;

namespace Flixer.Catalog.Api.Extensions;

public static class FormFileExtensions
{
    public static FileInput? ToFileInput(this IFormFile? formFile)
    {
        if (formFile == null) return null;
        var fileStream = new MemoryStream();
        
        formFile.CopyTo(fileStream);
        
        return new FileInput(
            Path.GetExtension(formFile.FileName),
            fileStream,
            formFile.ContentType
        );
    }
}