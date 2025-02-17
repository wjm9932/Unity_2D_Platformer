using System.Collections.Generic;
public abstract class CompositeNode : INode
{
    protected List<INode> children = new List<INode>();

    public void AddChild(INode child)
    {
        children.Add(child);
    }

    public abstract NodeState Evaluate();

    public virtual void Reset(int parentCompositionNodeIndex)
    {
        foreach (var child in children)
        {
            if (child is CompositeNode compositeChild)
            {
                compositeChild.Reset(parentCompositionNodeIndex);
            }
        }
    }

    public List<INode> GetChildren()
    {
        return children;
    }
}

