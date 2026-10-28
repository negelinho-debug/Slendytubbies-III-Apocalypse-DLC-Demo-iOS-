using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionCharRender : Action
	{
		public int parameterID = -1;

		public int constantID;

		public bool isPlayer;

		public Char _char;

		public RenderLock renderLock_sorting;

		public SortingMapType mapType;

		public int sortingOrder;

		public string sortingLayer;

		public RenderLock renderLock_scale;

		public int scale;

		public RenderLock renderLock_direction;

		public CharDirection direction;

		public ActionCharRender()
		{
			isDisplayed = true;
			title = "Character: Change rendering";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			_char = AssignFile(parameters, parameterID, constantID, _char);
			if (isPlayer)
			{
				_char = KickStarter.player;
			}
		}

		public override float Run()
		{
			if ((bool)_char)
			{
				if (renderLock_sorting == RenderLock.Set)
				{
					if (mapType == SortingMapType.OrderInLayer)
					{
						_char.SetSorting(sortingOrder);
					}
					else if (mapType == SortingMapType.SortingLayer)
					{
						_char.SetSorting(sortingLayer);
					}
				}
				else if (renderLock_sorting == RenderLock.Release)
				{
					_char.ReleaseSorting();
				}
				if (_char.animEngine == null)
				{
					_char.ResetAnimationEngine();
				}
				if (_char.animEngine != null)
				{
					_char.animEngine.ActionCharRenderRun(this);
				}
			}
			return 0f;
		}
	}
}
