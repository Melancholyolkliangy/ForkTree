using UnityEditor;
using UnityEngine;

namespace Script.Editor
{
	[CustomEditor(typeof(RunTest))]
	public class RunTestEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			RunTest myTarget = (RunTest)target;
			if (GUILayout.Button("Add New GameObject"))
			{
				var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				go.name = $"RunTest_({myTarget.toAddPosition})";
				go.transform.position = myTarget.toAddPosition;
				myTarget.forkTree.Insert(go.transform);
				myTarget.toAddPosition += Vector2.one * 1.2f;
			}

			if (GUILayout.Button("Find Collide Boxes"))
			{
				var collisionList = myTarget.forkTree.GetCollisionList();
				for (int i = 0; i < collisionList.Count; i++)
				{
					var tuple = collisionList[i];
					Color color = Random.ColorHSV();
					tuple.Item1.GetComponent<MeshRenderer>().material.color = color;
					tuple.Item2.GetComponent<MeshRenderer>().material.color = color;
				}
			}
			if (GUILayout.Button("Modified Boxes"))
			{
				myTarget.forkTree.Modified();
			}
		}
	}
}