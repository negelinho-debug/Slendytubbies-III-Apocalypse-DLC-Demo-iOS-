using UnityEngine;

namespace FSG.MeshAnimator
{
	[ExecuteInEditMode]
	public class AttachObjectToFace : MonoBehaviour
	{
		public MeshAnimator meshAnimator;

		public int faceIndex;

		public Vector3 offset;

		public Vector3 rotationOffset;

		public bool drawFaceDebugInfo;

		public Color debugColor = Color.black;

		private Transform mTransform;

		private Transform mMeshAnimatorTransform;

		private int[] triangles;

		private Vector3[] vertices;

		private void Awake()
		{
			mTransform = transform;
			if ((bool)meshAnimator)
			{
				mMeshAnimatorTransform = meshAnimator.transform;
				triangles = meshAnimator.GetComponent<MeshFilter>().sharedMesh.triangles;
				vertices = meshAnimator.GetComponent<MeshFilter>().sharedMesh.vertices;
			}
		}

		private void LateUpdate()
		{
			if ((bool)meshAnimator)
			{
				if (!mMeshAnimatorTransform)
				{
					mMeshAnimatorTransform = meshAnimator.transform;
				}
				if (triangles.Length == 0)
				{
					triangles = meshAnimator.GetComponent<MeshFilter>().sharedMesh.triangles;
					vertices = meshAnimator.GetComponent<MeshFilter>().sharedMesh.vertices;
				}
				faceIndex = Mathf.Clamp(faceIndex, 0, triangles.Length);
				Vector3 vector;
				Vector3 vector2;
				Vector3 vector3;
				if (!Application.isPlaying || !meshAnimator || !meshAnimator.currentAnimation)
				{
					vector = vertices[triangles[faceIndex]];
					vector2 = vertices[triangles[faceIndex + 1]];
					vector3 = vertices[triangles[faceIndex + 2]];
				}
				else
				{
					vector = meshAnimator.currentAnimation.GetFrame(meshAnimator.currentFrame)[triangles[faceIndex]];
					vector2 = meshAnimator.currentAnimation.GetFrame(meshAnimator.currentFrame)[triangles[faceIndex + 1]];
					vector3 = meshAnimator.currentAnimation.GetFrame(meshAnimator.currentFrame)[triangles[faceIndex + 2]];
				}
				Vector3 zero = Vector3.zero;
				zero += vector;
				zero += vector2;
				zero += vector3;
				zero /= 3f;
				Vector3 lhs = vector - vector2;
				Vector3 rhs = vector - vector3;
				Vector3 vector4 = Vector3.Cross(lhs, rhs);
				vector4.Normalize();
				Quaternion quaternion = Quaternion.identity;
				if (vector4 != Vector3.zero)
				{
					quaternion = Quaternion.LookRotation(vector4);
				}
				mTransform.position = mMeshAnimatorTransform.TransformPoint(zero) + quaternion * offset;
				mTransform.rotation = quaternion;
				mTransform.Rotate(rotationOffset);
			}
		}
	}
}
