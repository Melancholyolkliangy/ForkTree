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
				myTarget.objects.Add(BaseObjectFactory.CreateBaseObject(myTarget.toAddPosition, myTarget.forkTree));
				myTarget.toAddPosition += Vector2.one * 1.2f;
			}

			if (GUILayout.Button("Find Collide Boxes"))
			{
				var collisionList = myTarget.forkTree.GetCollisionList();
				for (int i = 0; i < collisionList.Count; i++)
				{
					var tuple = collisionList[i];
					Color color = Random.ColorHSV();
					tuple.Item1.Transform.GetComponent<MeshRenderer>().material.color = color;
					tuple.Item2.Transform.GetComponent<MeshRenderer>().material.color = color;
				}
			}
			if (GUILayout.Button("Modified Boxes"))
			{
				myTarget.forkTree.Modified();
			}
		}
	}
}