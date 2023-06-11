using Dekompiler.Core;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Components.TreeView;

public class TreeViewNodeModel
{
    public TreeViewNodeModel(Icons.IconInfo icon, Action action, Func<RenderFragment> childContent) :
        this(icon, action,
            childContent, new())
    {
    }
    
    public TreeViewNodeModel(Icons.IconInfo icon, Func<RenderFragment> childContent) :
        this(icon, Nothing,
            childContent, new())
    {
    }

    public TreeViewNodeModel(Icons.IconInfo icon, Func<RenderFragment> childContent, List<TreeViewNodeModel> nodes) :
        this(icon, Nothing,
            childContent, nodes)
    {
    }

    public TreeViewNodeModel(Icons.IconInfo icon, Action action, Func<RenderFragment> childContent,
        List<TreeViewNodeModel> nodes) : this(false, action, icon,
        childContent, nodes)
    {
    }

    public TreeViewNodeModel(bool toggled, Action action, Icons.IconInfo icon, Func<RenderFragment> childContent,
        List<TreeViewNodeModel> nodes)
    {
        Toggled = toggled;
        Action = action;
        Icon = icon;
        ChildContent = childContent;
        Nodes = nodes;
    }

    public bool Toggled { get; set; }

    public Action Action { get; }

    public Icons.IconInfo Icon { get; }

    public Func<RenderFragment> ChildContent { get; }

    public List<TreeViewNodeModel> Nodes { get; }

    private static void Nothing()
    {
    }
}