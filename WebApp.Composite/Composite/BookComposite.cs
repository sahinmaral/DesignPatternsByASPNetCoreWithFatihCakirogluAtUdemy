using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Composite.Composite;

public class BookComposite:IComponent
{
    public BookComposite(int id, string name)
    {
        Id = id;
        Name = name;
        _components = new();
    }

    private List<IComponent> _components;
    public IReadOnlyCollection<IComponent> Components => _components;
    public int Id { get; set; }
    public string Name { get; set; }
    public int Count()
    {
        return _components.Sum(x => x.Count());
    }

    public string DisplayAsList()
    {
        var sb = new StringBuilder();
        sb.Append($"<div class='text-primary my-1'><a href='#' class='menu'>{Name}</a> ({Count()})</div>");

        if (!_components.Any())
        {
            return sb.ToString();
        }

        sb.Append("<ul class='list-group list-group-flush ms-3'>");
        foreach (var component in _components)
        {
            sb.Append(component.DisplayAsList());
        }
        sb.Append("</ul>");
        return sb.ToString();
    }

    public List<SelectListItem> GetSelectedListItems(string line)
    {
        var list = new List<SelectListItem>() { new($"{line}{Name}", Id.ToString()) };
        
        if (_components.Any(component => component is BookComposite))
        {
            line += " - ";
        }

        foreach (var component in _components)
        {
            if (component is BookComposite bookComposite)
            {
                list.AddRange(bookComposite.GetSelectedListItems(line));
            }
        }

        return list;
    }

    public void Add(IComponent component)
    {
        _components.Add(component);
    }
    
    public void Remove(IComponent component)
    {
        _components.Remove(component);
    }
}