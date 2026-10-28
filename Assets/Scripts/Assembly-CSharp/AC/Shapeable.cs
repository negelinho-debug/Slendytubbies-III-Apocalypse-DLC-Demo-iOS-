using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Shapeable : MonoBehaviour
	{
		public List<ShapeGroup> shapeGroups = new List<ShapeGroup>();

		private SkinnedMeshRenderer skinnedMeshRenderer;

		private bool isChanging;

		private float targetShape;

		private float actualShape;

		private float originalShape;

		private int shapeKey;

		private float startTime;

		private float deltaTime;

		private void Awake()
		{
			AssignSkinnedMeshRenderer();
			if (!(skinnedMeshRenderer != null))
			{
				return;
			}
			foreach (ShapeGroup shapeGroup in shapeGroups)
			{
				shapeGroup.SetSMR(skinnedMeshRenderer);
				foreach (ShapeKey shapeKey in shapeGroup.shapeKeys)
				{
					shapeKey.SetValue(0f, skinnedMeshRenderer);
				}
			}
		}

		public void DisableAllKeys(int _groupID, float _deltaTime, MoveMethod _moveMethod, AnimationCurve _timeCurve)
		{
			foreach (ShapeGroup shapeGroup in shapeGroups)
			{
				if (shapeGroup.ID == _groupID)
				{
					shapeGroup.SetActive(-1, 0f, _deltaTime, _moveMethod, _timeCurve);
				}
			}
		}

		public void SetActiveKey(int _groupID, int _keyID, float _value, float _deltaTime, MoveMethod _moveMethod, AnimationCurve _timeCurve)
		{
			foreach (ShapeGroup shapeGroup in shapeGroups)
			{
				if (shapeGroup.ID == _groupID)
				{
					shapeGroup.SetActive(_keyID, _value, _deltaTime, _moveMethod, _timeCurve);
				}
			}
		}

		private void AssignSkinnedMeshRenderer()
		{
			skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			if (skinnedMeshRenderer == null)
			{
				skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
			}
		}

		public ShapeGroup GetGroup(int ID)
		{
			AssignSkinnedMeshRenderer();
			foreach (ShapeGroup shapeGroup in shapeGroups)
			{
				if (shapeGroup.ID == ID)
				{
					return shapeGroup;
				}
			}
			return null;
		}

		private void Update()
		{
			foreach (ShapeGroup shapeGroup in shapeGroups)
			{
				shapeGroup.UpdateKeys();
			}
			if (isChanging)
			{
				actualShape = Mathf.Lerp(originalShape, targetShape, AdvGame.Interpolate(startTime, deltaTime, MoveMethod.Linear, null));
				if (Time.time > startTime + deltaTime)
				{
					isChanging = false;
					actualShape = targetShape;
				}
				if ((bool)skinnedMeshRenderer)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(shapeKey, actualShape);
				}
			}
		}

		public void Change(int _shapeKey, float _targetShape, float _deltaTime)
		{
			if (targetShape < 0f)
			{
				targetShape = 0f;
			}
			else if (targetShape > 100f)
			{
				targetShape = 100f;
			}
			isChanging = true;
			targetShape = _targetShape;
			deltaTime = _deltaTime;
			startTime = Time.time;
			shapeKey = _shapeKey;
			if ((bool)skinnedMeshRenderer)
			{
				originalShape = skinnedMeshRenderer.GetBlendShapeWeight(shapeKey);
			}
		}
	}
}
