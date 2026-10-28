using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSG.MeshAnimator
{
	[Serializable]
	public class DeltaCompressedFrameData
	{
		public static float compressionAccuracy = 1000f;

		public float accuracy = 1000f;

		public int sizeOffset = 1;

		public int vertLength;

		public int exposedLength;

		[HideInInspector]
		public short[] positionsUShort;

		[HideInInspector]
		public int[] positions;

		[HideInInspector]
		public int[] frameIndexes;

		[HideInInspector]
		public Matrix4x4[] exposedTransforms;

		[HideInInspector]
		public Vector3[] rootMotionPositions;

		[HideInInspector]
		public Quaternion[] rootMotionRotations;

		public static implicit operator MeshFrameData[](DeltaCompressedFrameData s)
		{
			MeshFrameData[] array = new MeshFrameData[s.frameIndexes.Length / s.vertLength];
			bool flag = s.positionsUShort != null && s.positionsUShort.Length > 0;
			for (int i = 0; i < array.Length; i++)
			{
				Vector3[] array2 = new Vector3[s.vertLength];
				for (int j = 0; j < array2.Length; j++)
				{
					Vector3 zero = Vector3.zero;
					int num = s.frameIndexes[i * s.vertLength + j] * 3;
					int num2 = num;
					int num3 = num + 1;
					int num4 = num + 2;
					if (flag)
					{
						zero.x = (float)s.positionsUShort[num2] / s.accuracy;
						zero.y = (float)s.positionsUShort[num3] / s.accuracy;
						zero.z = (float)s.positionsUShort[num4] / s.accuracy;
					}
					else
					{
						zero.x = (float)s.positions[num2] / s.accuracy;
						zero.y = (float)s.positions[num3] / s.accuracy;
						zero.z = (float)s.positions[num4] / s.accuracy;
					}
					zero.x -= s.sizeOffset;
					zero.y -= s.sizeOffset;
					zero.z -= s.sizeOffset;
					array2[j] = zero;
				}
				array[i] = new MeshFrameData();
				array[i].exposedTransforms = new Matrix4x4[s.exposedLength];
				for (int k = 0; k < s.exposedLength; k++)
				{
					array[i].exposedTransforms[k] = s.exposedTransforms[k * array.Length + i];
				}
				array[i].SetVerts(array2);
				if (s.rootMotionPositions != null && s.rootMotionPositions.Length > i)
				{
					array[i].rootMotionPosition = s.rootMotionPositions[i];
				}
				if (s.rootMotionRotations != null && s.rootMotionRotations.Length > i)
				{
					array[i].rootMotionRotation = s.rootMotionRotations[i];
				}
			}
			return array;
		}

		public static implicit operator DeltaCompressedFrameData(MeshFrameData[] frames)
		{
			if (frames.Length == 0)
			{
				return new DeltaCompressedFrameData();
			}
			bool flag = false;
			for (int i = 0; i < frames.Length; i++)
			{
				if (frames[i].rootMotionPosition.x != 0f || frames[i].rootMotionPosition.y != 0f || frames[i].rootMotionPosition.z != 0f || frames[i].rootMotionRotation.x != 0f || frames[i].rootMotionRotation.y != 0f || frames[i].rootMotionRotation.z != 0f || frames[i].rootMotionRotation.w != 0f)
				{
					flag = true;
				}
			}
			DeltaCompressedFrameData deltaCompressedFrameData = new DeltaCompressedFrameData();
			deltaCompressedFrameData.vertLength = frames[0].verts.Length;
			deltaCompressedFrameData.frameIndexes = new int[frames[0].verts.Length * frames.Length];
			deltaCompressedFrameData.accuracy = compressionAccuracy;
			deltaCompressedFrameData.exposedLength = frames[0].exposedTransforms.Length;
			deltaCompressedFrameData.exposedTransforms = new Matrix4x4[frames.Length * frames[0].exposedTransforms.Length];
			deltaCompressedFrameData.rootMotionPositions = ((!flag) ? null : new Vector3[frames.Length]);
			deltaCompressedFrameData.rootMotionRotations = ((!flag) ? null : new Quaternion[frames.Length]);
			DeltaCompressedFrameData deltaCompressedFrameData2 = deltaCompressedFrameData;
			List<Vector3> list = new List<Vector3>();
			Dictionary<Vector3, int> dictionary = new Dictionary<Vector3, int>();
			int num = 1;
			for (int j = 0; j < frames.Length; j++)
			{
				for (int k = 0; k < frames[j].verts.Length; k++)
				{
					if (!dictionary.ContainsKey(frames[j].verts[k]))
					{
						dictionary.Add(frames[j].verts[k], list.Count);
						list.Add(frames[j].verts[k]);
						while (Mathf.Abs(frames[j].verts[k].x) > (float)num)
						{
							num *= 10;
						}
						while (Mathf.Abs(frames[j].verts[k].y) > (float)num)
						{
							num *= 10;
						}
						while (Mathf.Abs(frames[j].verts[k].z) > (float)num)
						{
							num *= 10;
						}
					}
					deltaCompressedFrameData2.frameIndexes[j * deltaCompressedFrameData2.vertLength + k] = dictionary[frames[j].verts[k]];
				}
				for (int l = 0; l < frames[j].exposedTransforms.Length; l++)
				{
					deltaCompressedFrameData2.exposedTransforms[frames.Length * l + j] = frames[j].exposedTransforms[l];
				}
				if (deltaCompressedFrameData2.rootMotionPositions != null)
				{
					deltaCompressedFrameData2.rootMotionPositions[j] = frames[j].rootMotionPosition;
				}
				if (deltaCompressedFrameData2.rootMotionRotations != null)
				{
					deltaCompressedFrameData2.rootMotionRotations[j] = frames[j].rootMotionRotation;
				}
			}
			deltaCompressedFrameData2.sizeOffset = num;
			bool flag2 = true;
			deltaCompressedFrameData2.positions = new int[list.Count * 3];
			for (int m = 0; m < list.Count; m++)
			{
				deltaCompressedFrameData2.positions[m * 3] = (int)((list[m].x + (float)deltaCompressedFrameData2.sizeOffset) * deltaCompressedFrameData2.accuracy);
				deltaCompressedFrameData2.positions[m * 3 + 1] = (int)((list[m].y + (float)deltaCompressedFrameData2.sizeOffset) * deltaCompressedFrameData2.accuracy);
				deltaCompressedFrameData2.positions[m * 3 + 2] = (int)((list[m].z + (float)deltaCompressedFrameData2.sizeOffset) * deltaCompressedFrameData2.accuracy);
				if (flag2)
				{
					if (deltaCompressedFrameData2.positions[m * 3] > 65535)
					{
						flag2 = false;
					}
					else if (deltaCompressedFrameData2.positions[m * 3 + 1] > 65535)
					{
						flag2 = false;
					}
					else if (deltaCompressedFrameData2.positions[m * 3 + 2] > 65535)
					{
						flag2 = false;
					}
				}
			}
			if (flag2)
			{
				deltaCompressedFrameData2.positionsUShort = new short[deltaCompressedFrameData2.positions.Length];
				for (int n = 0; n < deltaCompressedFrameData2.positions.Length; n++)
				{
					deltaCompressedFrameData2.positionsUShort[n] = (short)deltaCompressedFrameData2.positions[n];
				}
				deltaCompressedFrameData2.positions = null;
			}
			return deltaCompressedFrameData2;
		}
	}
}
