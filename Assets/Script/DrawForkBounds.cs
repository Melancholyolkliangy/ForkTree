using UnityEngine;

namespace Script.Editor
{
	public class DrawForkBounds : MonoBehaviour
	{
		public ForkTree forkTree;
		public Color color = new Color(1,1,1,0.5f);
		private void OnDrawGizmos()
		{
			if(forkTree == null)
				return;
			color.a = 0.5f;
			if (forkTree.IsDivided)
			{
				DrawBounds(forkTree.NorthEast);
				color.r = 0;
				color.g = 1;
				DrawBounds(forkTree.NorthWest);
				color.r = 0.2f;
				color.g = 1;
				DrawBounds(forkTree.SouthWest);
				color.r = 0.4f;
				color.g = 1;
				DrawBounds(forkTree.SouthEast);
				color.r = 0.6f;
				color.g = 1;
			}
			else
			{
				DrawBounds(forkTree);
			}
		}

		void DrawBounds(ForkTree forkTree)
		{
			color.g *= 0.5f; 
			Gizmos.color = color;
			Gizmos.DrawCube(forkTree.Bounds.center,forkTree.Bounds.size);
			if (forkTree.NorthEast != null)
			{
				DrawBounds(forkTree.NorthEast);
			}
			if (forkTree.NorthWest != null)
			{
				DrawBounds(forkTree.NorthWest);
			}
			if (forkTree.SouthWest != null)
			{
				DrawBounds(forkTree.SouthWest);
			}
			if (forkTree.SouthEast != null)
			{
				DrawBounds(forkTree.SouthEast);
			}
		}
	}
}