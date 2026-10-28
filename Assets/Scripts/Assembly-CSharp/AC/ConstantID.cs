using UnityEngine;

namespace AC
{
	[ExecuteInEditMode]
	public class ConstantID : MonoBehaviour
	{
		public int constantID;

		public bool retainInPrefab;

		public AutoManual autoManual;

		protected bool GameIsPlaying()
		{
			return true;
		}
	}
}
