using UnityEngine;

namespace AC
{
	public struct LipSyncShape
	{
		public int frame;

		public float timeIndex;

		public LipSyncShape(int _frame, float _timeIndex, float speed, float fps)
		{
			frame = _frame;
			timeIndex = _timeIndex / 15f / speed / fps + Time.time;
		}

		public LipSyncShape(int _frame, float _timeIndex, float speed)
		{
			frame = _frame;
			timeIndex = _timeIndex / 15f / speed + Time.time;
		}
	}
}
