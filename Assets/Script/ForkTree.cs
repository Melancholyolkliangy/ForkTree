using System.Collections.Generic;
using UnityEngine;

public class ForkTree
{
	public bool IsDivided { get;private set; }
	public ForkTree NorthWest { get;private set; }
	public ForkTree NorthEast { get;private set; }
	public ForkTree SouthEast { get;private set; }
	public ForkTree SouthWest { get;private set; }
	public int Capacity { get; private set; } = 10;
	public Bounds Bounds { get; private set; }
	private List<Transform> Transforms = new List<Transform>();

	public ForkTree(Bounds bounds,int capacity = 10)
	{
		this.Bounds = bounds;
		this.Capacity = capacity;
		IsDivided = false;
	}

	public void Modified()
	{
		var modifiedObjects = GetModifiedObject();
		foreach (var modifiedObject in modifiedObjects)
		{
			Insert(modifiedObject);
		}
	}

	List<Transform> GetModifiedObject()
	{
		List<Transform> result = new List<Transform>();
		if (!IsDivided)
		{
			for (int i = Transforms.Count - 1; i >= 0; i--)
			{
				var transform = Transforms[i];
				if (!Bounds.Contains(transform.position))
				{
					Transforms.Remove(transform);
					result.Add(transform);
				}
			}
		}
		else
		{
			result.AddRange(NorthEast.GetModifiedObject());
			result.AddRange(NorthWest.GetModifiedObject());
			result.AddRange(SouthEast.GetModifiedObject());
			result.AddRange(SouthWest.GetModifiedObject());
		}
		return result;
	}
	public List<(Transform, Transform)> GetCollisionList()
	{
		List<(Transform, Transform)> collisions = new List<(Transform, Transform)>();
		if (!IsDivided)
		{
			for (int i = 0; i < Transforms.Count; i++)
			{
				for (int j = i + 1; j < Transforms.Count; j++)
				{
					if(CheckCollision(Transforms[i], Transforms[j]))
						collisions.Add((Transforms[i], Transforms[j]));
				}
			}
		}
		else
		{
			collisions.AddRange(NorthEast.GetCollisionList());
			collisions.AddRange(NorthWest.GetCollisionList());
			collisions.AddRange(SouthWest.GetCollisionList());
			collisions.AddRange(SouthEast.GetCollisionList());
		}
		return collisions;
	}
	bool CheckCollision(Transform t1,Transform t2)
	{
		UnityEngine.Bounds b1 = t1.GetComponent<BoxCollider>().bounds;
		UnityEngine.Bounds b2 = t2.GetComponent<BoxCollider>().bounds;
		return b1.Intersects(b2);
	}
	//向四叉树插入新物体
	public void Insert(Transform transform)
	{
		if (IsDivided)
		{
			InsertToChildren(transform);
			return;
		}
		if(Transforms.Contains(transform))
			return;
		if (Transforms.Count == Capacity)
		{
			//分割树
			Divide();
			InsertToChildren(transform);
			return;
		}
		Transforms.Add(transform);
	}

	private void Divide()
	{
		NorthWest = new ForkTree(new Bounds(new Vector2(Bounds.center.x - Bounds.extents.x / 2,Bounds.center.y + Bounds.extents.y / 2), Bounds.extents),10);
		NorthEast = new ForkTree(new Bounds(new Vector2(Bounds.center.x + Bounds.extents.x / 2,Bounds.center.y + Bounds.extents.y / 2), Bounds.extents),10);
		SouthEast = new ForkTree(new Bounds(new Vector2(Bounds.center.x + Bounds.extents.x / 2,Bounds.center.y - Bounds.extents.y / 2), Bounds.extents),10);
		SouthWest = new ForkTree(new Bounds(new Vector2(Bounds.center.x - Bounds.extents.x / 2,Bounds.center.y - Bounds.extents.y / 2), Bounds.extents),10);
		IsDivided = true;
		//将存储的物体转移到子树中
		foreach (var transform in Transforms)
		{
			InsertToChildren(transform);
		}
		Transforms.Clear();
		Transforms = null;
	}

	private void InsertToChildren(Transform transform)
	{
		if (NorthEast.Bounds.Contains(transform.position))
		{
			NorthEast.Insert(transform);
		}
		else if (NorthWest.Bounds.Contains(transform.position))
		{
			NorthWest.Insert(transform);
		}
		else if (SouthWest.Bounds.Contains(transform.position))
		{
			SouthWest.Insert(transform);
		}
		else
		{
			SouthEast.Insert(transform);
		}
	}
}