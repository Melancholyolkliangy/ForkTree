using System;
using System.Collections.Generic;
using Script.Editor;
using UnityEngine;

namespace Script
{
	
	public class RunTest : MonoBehaviour
	{
		public Vector2 toAddPosition;
		public ForkTree forkTree;
		public float tickRate;
		public List<MyGameObject> objects = new ();
		public uint width = 80;
		public uint height = 80;
		private void Start()
		{
			forkTree = new ForkTree(new Bounds(Vector3.zero, new Vector2(width, height)));
			var drawer = FindObjectOfType<DrawForkBounds>();
			drawer.forkTree = forkTree;
			InvokeRepeating(nameof(UpdateTree),0,tickRate);
		}

		void UpdateTree()
		{
			forkTree.Modified();
			var collisions = forkTree.GetCollisionList();
			for (int i = 0; i < collisions.Count; i++)
			{
				var tuple = collisions[i];
				CalculateSpeed(tuple.Item1, tuple.Item2);
				//防止卡在内部需要碰撞对象互相挤开
				Vector2 item1Translate = Vector2.zero;
				Vector2 item2Translate = Vector2.zero;
				Bounds item1Bounds = tuple.Item1.bounds;
				Bounds item2Bounds = tuple.Item2.bounds;
				if (item1Bounds.min.x < item2Bounds.max.x && item1Bounds.max.x > item2Bounds.min.x)
				{
					float x_distance = Mathf.Min(item1Bounds.max.x - item1Bounds.min.x, item2Bounds.max.x - item1Bounds.min.x);
					float x_unit = x_distance / (Mathf.Abs(tuple.Item2.speed.x) + Mathf.Abs(tuple.Item1.speed.x));
					item1Translate.x = x_unit * tuple.Item1.speed.x;
					item2Translate.x = x_unit * tuple.Item2.speed.x;
				}

				if (item1Bounds.min.y < item2Bounds.max.y && item1Bounds.max.y > item2Bounds.min.y)
				{
					float y_distance = Mathf.Min(item1Bounds.max.y - item1Bounds.min.y, item2Bounds.max.y - item1Bounds.min.y);
					float y_unit = y_distance / (Mathf.Abs(tuple.Item2.speed.y) + Mathf.Abs(tuple.Item1.speed.y));
					item1Translate.y = y_unit * tuple.Item1.speed.y;
					item2Translate.y = y_unit * tuple.Item2.speed.y;
				}
				tuple.Item1.compensation = item1Translate;
				tuple.Item2.compensation = item2Translate;
				Debug.Log($"Collide {tuple.Item1.Transform} and {tuple.Item2.Transform}");
			}
			for (int i = 0; i < objects.Count; i++)
			{
				var objectTransform = objects[i];
				var nextPosition = objectTransform.Transform.position + objectTransform.speed * tickRate;
				Vector3 nextSpeed = objectTransform.speed;
				if (nextPosition.x >= width / 2f ||
				    nextPosition.x <= -width / 2f)
				{
					if (Vector3.Dot(-nextPosition,objectTransform.speed) < 0)
					{
						nextSpeed.x *= -1;
					}
				}

				if (nextPosition.y >= height / 2f ||
				    nextPosition.y <= -height / 2f)
				{
					if (Vector3.Dot(-nextPosition,objectTransform.speed) < 0)
					{
						nextSpeed.y *= -1;
					}
				}

				objectTransform.Transform.Translate(nextSpeed * tickRate + objectTransform.compensation);
				objectTransform.speed = nextSpeed;
				objectTransform.compensation = Vector3.zero;
			}
		}

		void CalculateSpeed(MyGameObject item1,MyGameObject item2)
		{
			switch (item2.mass - item1.mass)
			{
				case 0 :
					//动量守恒
					(item1.speed, item2.speed) = (item2.speed, item1.speed);
					break;
				default:
					item1.speed = (item1.mass - item2.mass) / (item2.mass + item1.mass) * item1.speed +
					              2 * item2.mass / (item1.mass + item2.mass) * item2.speed;
					item2.speed = 2 * item1.mass * item1.speed / (item1.mass + item2.mass) +
					              (item2.mass - item1.mass) / (item1.mass + item2.mass) * item2.speed;
					break;
			}
		}
	}
}