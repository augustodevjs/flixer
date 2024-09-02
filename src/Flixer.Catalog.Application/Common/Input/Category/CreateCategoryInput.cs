using MediatR;
using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Output.Category;

namespace Flixer.Catalog.Application.Common.Input.Category;

public class CreateCategoryInput : IRequest<CategoryOutput>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    
    public CreateCategoryInput(string name, string description, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        Description = description;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }
}