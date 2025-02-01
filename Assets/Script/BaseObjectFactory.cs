using UnityEngine;

namespace Script
{
	public static class BaseObjectFactory
	{
		public static MyGameObject CreateBaseObject(Vector3 position,ForkTree forkTree)
		{
			var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.name = $"RunTest_({position})";
			go.transform.position = position;
			// go.transform.localScale = Vector3.one * Random.Range(0.5f,5f);
			go.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
			var newGameObject = new MyGameObject()
			{
				mass = Random.Range(0.1f,1000f),
				speed = new Vector3(Random.Range(1, 10f), Random.Range(1, 10f), 0),
				Transform = go.transform,
			};
			forkTree.Insert(newGameObject);
			return newGameObject;
		}
	}
}