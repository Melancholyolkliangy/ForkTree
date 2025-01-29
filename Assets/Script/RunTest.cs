using Script.Editor;
using UnityEngine;

namespace Script
{
	public class RunTest : MonoBehaviour
	{
		public Vector2 toAddPosition;
		public ForkTree forkTree;
		private void Start()
		{
			forkTree = new ForkTree(new Bounds(Vector3.zero, Vector2.one * 100));
			var drawer = FindObjectOfType<DrawForkBounds>();
			drawer.forkTree = forkTree;
		}
	}
}