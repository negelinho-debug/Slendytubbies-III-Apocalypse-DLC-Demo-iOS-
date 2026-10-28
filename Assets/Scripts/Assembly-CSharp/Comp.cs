using System.Collections.Generic;

public class Comp : IComparer<EffectData>
{
	public int Compare(EffectData x, EffectData y)
	{
		if (x == null)
		{
			if (y == null)
			{
				return 0;
			}
			return 1;
		}
		if (y == null)
		{
			return -1;
		}
		float num = x.m_fTimeSec.CompareTo(y.m_fTimeSec);
		if (num > 0f)
		{
			return 1;
		}
		if (num < 0f)
		{
			return -1;
		}
		return 0;
	}
}
