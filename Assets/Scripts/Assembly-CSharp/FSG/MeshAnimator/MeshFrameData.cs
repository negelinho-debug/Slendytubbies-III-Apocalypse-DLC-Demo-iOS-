using System;
using UnityEngine;

namespace FSG.MeshAnimator
{
	[Serializable]
	public class MeshFrameData
	{
		[NonSerialized]
		private Vector3[] decompressed;

		[NonSerialized]
		public Matrix4x4[] exposedTransforms;

		[NonSerialized]
		public Vector3 rootMotionPosition;

		[NonSerialized]
		public Quaternion rootMotionRotation;

		public Vector3[] verts => decompressed;

		public void SetVerts(Vector3[] v)
		{
			decompressed = v;
		}

		public override bool Equals(object obj)
		{
			if (obj is MeshFrameData)
			{
				MeshFrameData meshFrameData = (MeshFrameData)obj;
				if (meshFrameData.verts.Length != verts.Length)
				{
					return false;
				}
				for (int i = 0; i < meshFrameData.verts.Length; i++)
				{
					if (verts[i] != meshFrameData.verts[i])
					{
						return false;
					}
				}
				return true;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
