using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Builds a sort of parse-tree of nodes
/// </summary>
/// <typeparam name="C"></typeparam>
public class NodeTree<C>
{
	public readonly C value;
	public readonly NodeTree<C> parent;
	private List<NodeTree<C>> children = new List<NodeTree<C>>();

	public NodeTree<C>[] Children { get { return children.ToArray(); } }

	private NodeTree(C value, NodeTree<C> parent)
	{
		this.value = value;
		this.parent = parent;

		if (null != parent)
			parent.children.Add(this);
	}

	public override bool Equals(object obj)
	{
		var other = obj as NodeTree<C>;

		if (null == other)
			return false;

		return value.Equals(other.value) && children.SequenceEqual(other.children);
	}

	public override int GetHashCode()
	{
		var hash = GetType().Name.GetHashCode() ^ value.GetHashCode();

		foreach (var child in children)
		{
			hash = ((hash << 3) | (hash >> (32 - 3))) ^ child.GetHashCode();
		}

		return hash;
	}

	public static NodeTree<C> Root(C value)
	{
		return new NodeTree<C>(value, null);
	}

	public NodeTree<C> Append(C value)
	{
		return new NodeTree<C>(value, this);
	}

	public delegate T Visitor<T>(T last, C next) where T : class;

	public void visit<T>(T root, Visitor<T> visitor) where T : class
	{
		var self = visitor(root, value);

		foreach (var child in children)
		{
			child.visit(self, visitor);
		}
	}

	public delegate bool Test(C value);
	public static NodeTree<C> Graph(IEnumerable<C> source, Test open, Test close)
	{
		var iterator = source.GetEnumerator();

		if (!iterator.MoveNext())
			return null;

		var root = Root(iterator.Current);
		var tail = new List<NodeTree<C>>();
		tail.Insert(0, root);

		while (iterator.MoveNext())
		{
			var next = iterator.Current;

			if (open(next))
			{
				tail.Insert(0, tail[0]);
			}
			else if (close(next))
			{
				tail.Pull();
			}
			else
			{
				tail[0] = tail[0].Append(next);
			}
		}

		return root;
	}
}


