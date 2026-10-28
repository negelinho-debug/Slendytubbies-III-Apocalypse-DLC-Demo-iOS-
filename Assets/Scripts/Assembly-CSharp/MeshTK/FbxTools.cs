using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MeshTK
{
	public class FbxTools : MonoBehaviour
	{
		private class FbxNode
		{
			public int indentation;

			public string key;

			public string val;

			public string content;

			public FbxNode(int i, string k, string v)
			{
				indentation = i;
				key = k;
				val = v;
			}

			public FbxNode(int i, string k, string v, string c)
			{
				indentation = i;
				key = k;
				val = v;
				content = c;
			}

			public override string ToString()
			{
				string text = new string('\t', indentation) + key + ": " + val;
				if (!string.IsNullOrEmpty(content))
				{
					string text2 = text;
					text = text2 + " {\n" + content + new string('\t', indentation) + "}";
				}
				return text + "\n";
			}
		}

		private class FbxContainerNode : FbxNode
		{
			public FbxNode[] subnodes;

			public FbxContainerNode(int i, string k, string v, FbxNode[] n)
				: base(i, k, v, string.Empty)
			{
				subnodes = n;
			}

			public override string ToString()
			{
				if (subnodes != null)
				{
					content = subnodes.Aggregate(string.Empty, (string s, FbxNode i) => s + i.ToString());
				}
				return base.ToString();
			}
		}

		public static void ExportToFile(SkinnedMeshRenderer renderer, string filename, bool uselhcoords = true)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return;
			}
			using (StreamWriter streamWriter = new StreamWriter(filename))
			{
				Mesh sharedMesh = renderer.sharedMesh;
				Renderer renderer2 = renderer;
				streamWriter.Write(ExportToString(sharedMesh, ref renderer2, uselhcoords));
			}
		}

		public static void ExportToFile(Mesh mesh, Renderer renderer, string filename, bool uselhcoords = true)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return;
			}
			using (StreamWriter streamWriter = new StreamWriter(filename))
			{
				streamWriter.Write(ExportToString(mesh, ref renderer, uselhcoords));
			}
		}

		public static string ExportToString(Mesh mesh, ref Renderer renderer, bool uselhcoords = true)
		{
			string text = "; FBX 6.1.0 project file\n; Copyright (C) 1997-2008 Autodesk Inc. and/or its licensors.\n; All rights reserved.\n; ----------------------------------------------------\n\n";
			text += FBXHeaderExtensionNode();
			text += "\n\n; Object definitions\n;------------------------------------------------------------------\n\n";
			text += DefinitionsNode(ref mesh, ref renderer).ToString();
			text += "\n\n; Object properties\n;------------------------------------------------------------------\n\n";
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			if (renderer.GetType() == typeof(SkinnedMeshRenderer))
			{
				SkinnedMeshRenderer renderer2 = renderer as SkinnedMeshRenderer;
				text2 = SkinnedModelNodes(ref renderer2);
				text3 = DeformationNodes(ref mesh, ref renderer2);
				text4 = PoseNode(ref mesh);
			}
			text += ObjectsNode(ModelNodes(ref mesh, uselhcoords) + text2, MaterialNodes(ref renderer), TextureNodes(ref renderer), text3, text4, GlobalSettingsNode().ToString());
			text += "\n\n; Object connections\n;------------------------------------------------------------------\n\n";
			text += ConnectionsNode(ref mesh, ref renderer).ToString();
			text += "\n\n;Version 5 settings\n;------------------------------------------------------------------\n\n";
			return text + Version5Node();
		}

		private static string FBXHeaderExtensionNode()
		{
			DateTime now = DateTime.Now;
			return "FBXHeaderExtension:  {\n\tFBXHeaderVersion: 1003\n\tFBXVersion: 6100\n\tCreationTimeStamp:  {\n\t\tVersion: 1000\n" + string.Format("\t\tYear: {0:yyyy}\n\t\tMonth: {0:MM}\n\t\tDay: {0:dd}\n\t\tHour: {0:HH}\n\t\tMinute: {0:mm}\n\t\tSecond: {0:ss}\n\t\tMillisecond: {0:fff}\n", now) + "\t}\n\tCreator: \"EJM Software FBX Exporter\"\n\tOtherFlags:  {\n\t\tFlagPLE: 0\n\t}\n}\n" + string.Format("CreationTime: \"{0:yyyy}-{0:MM}-{0:dd} {0:HH}:{0:mm}:{0:ss}:{0:fff}\"\n", now) + "Creator: \"EJM Software FBX Exporter\"";
		}

		private static FbxNode DefinitionsNode(ref Mesh mesh, ref Renderer renderer)
		{
			int num = 6;
			if (renderer.GetType() == typeof(SkinnedMeshRenderer))
			{
				num += 2;
			}
			int num2 = 1;
			int num3 = MaterialNodeCount(ref renderer);
			int num4 = TextureNodeCount(ref renderer);
			FbxContainerNode fbxContainerNode = new FbxContainerNode(0, "Definitions", string.Empty, new FbxNode[num]);
			fbxContainerNode.subnodes[0] = new FbxNode(1, "Version", "100");
			fbxContainerNode.subnodes[1] = new FbxNode(1, "Count", (num2 + num3 + num4 + 1).ToString());
			fbxContainerNode.subnodes[2] = new FbxContainerNode(2, "ObjectType", "\"Model\"", new FbxNode[1]
			{
				new FbxNode(3, "Count", num2.ToString())
			});
			fbxContainerNode.subnodes[3] = new FbxContainerNode(2, "ObjectType", "\"Material\"", new FbxNode[1]
			{
				new FbxNode(3, "Count", num3.ToString())
			});
			fbxContainerNode.subnodes[4] = new FbxContainerNode(2, "ObjectType", "\"Texture\"", new FbxNode[1]
			{
				new FbxNode(3, "Count", num4.ToString())
			});
			if (renderer.GetType() == typeof(SkinnedMeshRenderer))
			{
				SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
				fbxContainerNode.subnodes[5] = new FbxContainerNode(2, "ObjectType", "\"Deformer\"", new FbxNode[1]
				{
					new FbxNode(3, "Count", skinnedMeshRenderer.bones.Length.ToString())
				});
				fbxContainerNode.subnodes[6] = new FbxContainerNode(2, "ObjectType", "\"Pose\"", new FbxNode[1]
				{
					new FbxNode(3, "Count", "1")
				});
			}
			fbxContainerNode.subnodes[num - 1] = new FbxContainerNode(2, "ObjectType", "\"GlobalSettings\"", new FbxNode[1]
			{
				new FbxNode(3, "Count", "1")
			});
			return fbxContainerNode;
		}

		private static string ObjectsNode(params string[] nodes)
		{
			return "Objects:  {\n" + string.Join("\n", nodes) + "\n}\n";
		}

		private static string PoseNode(ref Mesh mesh)
		{
			string text = "\tPose: \"Pose::BIND_POSES\", \"BindPose\" {\n\t\tType: \"BindPose\"\n\t\tVersion: 100\n";
			text = text + "\t\tNbPoseNodes: " + mesh.bindposes.Length + "\n";
			for (int i = 0; i < mesh.bindposes.Length; i++)
			{
				text += "\t\tPoseNode:  {\n";
				text = text + "\t\t\tNode: \"Model::Bone_" + i + "\"\n";
				text += "\t\t\tMatrix: ";
				for (int j = 0; j < 16; j++)
				{
					if (j > 0)
					{
						text += ",";
					}
					text += mesh.bindposes[i][j % 4, j / 4].ToString("n8");
				}
				text += "\n";
				text += "\t\t}\n";
			}
			return text + "\t}\n";
		}

		private static FbxNode ConnectionsNode(ref Mesh mesh, ref Renderer renderer)
		{
			List<FbxNode> list = new List<FbxNode>();
			list.Add(new FbxNode(1, "Connect", "\"OO\", \"Model::" + mesh.name + "\", \"Model::Scene\""));
			Material[] sharedMaterials = renderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (!(material == null))
				{
					list.Add(new FbxNode(1, "Connect", "\"OO\", \"Material::" + material.name + "\", \"Model::" + mesh.name + "\""));
					if (material.HasProperty("_MainTex"))
					{
						list.Add(new FbxNode(1, "Connect", "\"OO\", \"Texture::" + material.name + "_diff\", \"Model::" + mesh.name + "\""));
					}
					if (material.HasProperty("_BumpMap"))
					{
						list.Add(new FbxNode(1, "Connect", "\"OO\", \"Texture::" + material.name + "_bump\", \"Model::" + mesh.name + "\""));
					}
				}
			}
			if (renderer.GetType() == typeof(SkinnedMeshRenderer))
			{
				SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
				list.Add(new FbxNode(1, "Connect", "\"OO\", \"Deformer::Skin " + mesh.name + "\", \"Model::" + mesh.name + "\""));
				Transform[] bones = skinnedMeshRenderer.bones;
				foreach (Transform transform in bones)
				{
					list.Add(new FbxNode(1, "Connect", "\"OO\", \"SubDeformer::Cluster " + mesh.name + " " + transform.name + "\", \"Deformer::Skin " + mesh.name + "\""));
				}
				Transform[] bones2 = skinnedMeshRenderer.bones;
				foreach (Transform transform2 in bones2)
				{
					list.Add(new FbxNode(1, "Connect", "\"OO\", \"Model::" + transform2.name + "\", \"SubDeformer::Cluster " + mesh.name + " " + transform2.name + "\""));
				}
				List<Transform> list2 = skinnedMeshRenderer.bones.ToList();
				while (list2.Count > 0)
				{
					for (int l = 0; l < list2.Count; l++)
					{
						Transform parent = list2[l].parent;
						if (!list2.Contains(parent))
						{
							if (Array.Exists(skinnedMeshRenderer.bones, (Transform p) => p == parent))
							{
								list.Add(new FbxNode(1, "Connect", "\"OO\", \"Model::" + list2[l].name + "\", \"Model::" + parent.name + "\""));
							}
							else
							{
								list.Add(new FbxNode(1, "Connect", "\"OO\", \"Model::" + list2[l].name + "\", \"Model::Scene\""));
							}
							list2.RemoveAt(l);
						}
					}
				}
			}
			return new FbxContainerNode(0, "Connections", string.Empty, list.ToArray());
		}

		private static FbxNode Version5Node()
		{
			return new FbxNode(0, "Version5", string.Empty, "\tAmbientRenderSettings:  {\n\t\tVersion: 101\n\t\tAmbientLightColor: 0.0,0.0,0.0,0\n\t}\n\tFogOptions:  {\n\t\tFogEnable: 0\n\t\tFogMode: 0\n\t\tFogDensity: 0.000\n\t\tFogStart: 5.000\n\t\tFogEnd: 25.000\n\t\tFogColor: 0.1,0.1,0.1,1\n\t}\n\tSettings:  {\n\t\tFrameRate: \"24\"\n\t\tTimeFormat: 1\n\t\tSnapOnFrames: 0\n\t\tReferenceTimeIndex: -1\n\t\tTimeLineStartTime: 0\n\t\tTimeLineStopTime: 479181389250\n\t}\n\tRendererSetting:  {\n\t\tDefaultCamera: \"Producer Perspective\"\n\t\tDefaultViewingMode: 0\n\t}\n");
		}

		private static string ModelNodes(ref Mesh mesh, bool uselhcoords = true)
		{
			bool flag = mesh.uv2.Length > 0;
			bool flag2 = mesh.colors.Length > 0;
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			string text5 = string.Empty;
			string text6 = string.Empty;
			string text7 = string.Empty;
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				if (i != 0)
				{
					text += ",";
					text2 += ",";
					text3 += ",";
					text4 += ",";
					text5 += ",";
					text6 += ",";
					text7 += ",";
				}
				text += $"{mesh.vertices[i].x * (float)((!uselhcoords) ? 1 : (-1)):n6},{mesh.vertices[i].y:n6},{mesh.vertices[i].z:n6}";
				text2 += $"{mesh.normals[i].x * (float)((!uselhcoords) ? 1 : (-1)):n6},{mesh.normals[i].y:n6},{mesh.normals[i].z:n6}";
				if (flag2)
				{
					text3 += $"{mesh.colors[i].r:n6},{mesh.colors[i].g:n6},{mesh.colors[i].b:n6},{mesh.colors[i].a:n6}";
				}
				text4 = text4 + mesh.uv[i].x.ToString("n6") + "," + mesh.uv[i].y.ToString("n6");
				if (flag)
				{
					text5 = text5 + mesh.uv2[i].x.ToString("n6") + "," + mesh.uv2[i].y.ToString("n6");
				}
			}
			string text8 = "\n\t\tPolygonVertexIndex: ";
			for (int j = 0; j < mesh.triangles.Length; j += 3)
			{
				if (j != 0)
				{
					text8 += ",";
				}
				text8 += string.Format((!uselhcoords) ? "{0},{1},{2}" : "{1},{0},{2}", mesh.triangles[j], mesh.triangles[j + 1], -1 - mesh.triangles[j + 2]);
			}
			string text9 = "\n\t\tLayerElementNormal: 0 {\n\t\t\tVersion: 101\n\t\t\tName: \"\"\n\t\t\tMappingInformationType: \"ByVertice\"\n\t\t\tReferenceInformationType: \"Direct\"\n\t\t\tNormals: " + text2 + "\n\t\t}";
			string text10 = string.Empty;
			if (flag2)
			{
				text10 = "\n\t\tLayerElementColor: 0 {\n\t\t\tVersion: 101\n\t\t\tName: \"Col\"\n\t\t\tMappingInformationType: \"ByVertice\"\n\t\t\tReferenceInformationType: \"Direct\"\n\t\t\tColors: " + text3 + "\n\t\t}";
			}
			string text11 = "\n\t\tLayerElementUV: 0 {\n\t\t\tVersion: 101\n\t\t\tName: \"UVMap\"\n\t\t\tMappingInformationType: \"ByVertice\"\n\t\t\tReferenceInformationType: \"Direct\"\n\t\t\tUV: ";
			text11 = text11 + text4 + "\n\t\t}";
			string text12 = string.Empty;
			if (flag)
			{
				text12 = "\n\t\tLayerElementUV: 1 {\n\t\t\tVersion: 101\n\t\t\tName: \"UVMap_2\"\n\t\t\tMappingInformationType: \"ByVertice\"\n\t\t\tReferenceInformationType: \"Direct\"\n\t\t\tUV: " + text5 + "\n\t\t}";
			}
			string text13 = "\n\t\tLayerElementTexture: 0 {\n\t\t\tVersion: 101\n\t\t\tName: \"\"\n\t\t\tMappingInformationType: \"NoMappingInformation\"\n\t\t\tReferenceInformationType: \"IndexToDirect\"\n\t\t\tBlendMode: \"Translucent\"\n\t\t\tTextureAlpha: 1\n\t\t\tTextureId: \n\t\t}";
			string text14 = "\n\t\tLayerElementMaterial: 0 {\n\t\t\tVersion: 101\n\t\t\tName: \"\"\n\t\t\tMappingInformationType: \"ByPolygon\"\n\t\t\tReferenceInformationType: \"IndexToDirect\"\n\t\t\tMaterials: ";
			for (int k = 0; k < mesh.subMeshCount; k++)
			{
				if (k != 0)
				{
					text14 += ",";
				}
				text14 += string.Join(",", Enumerable.Repeat(k.ToString(), mesh.GetTriangles(k).Length / 3).ToArray());
			}
			text14 += "\n\t\t}";
			string text15 = "\n\t\tLayer: 0 {\n\t\t\tVersion: 100\n\t\t\tLayerElement:  {\n\t\t\t\tType: \"LayerElementNormal\"\n\t\t\t\tTypedIndex: 0\n\t\t\t}\n\t\t\tLayerElement:  {\n\t\t\t\tType: \"LayerElementSmoothing\"\n\t\t\t\tTypedIndex: 0\n\t\t\t}\n\t\t\tLayerElement:  {\n\t\t\t\tType: \"LayerElementUV\"\n\t\t\t\tTypedIndex: 0\n\t\t\t}" + ((!flag2) ? string.Empty : "\n\t\t\tLayerElement:  {\n\t\t\t\tType: \"LayerElementColor\"\n\t\t\t\tTypedIndex: 0\n\t\t\t}") + "\n\t\t\tLayerElement:  {\n\t\t\t\tType: \"LayerElementTexture\"\n\t\t\t\tTypedIndex: 0\n\t\t\t}\n\t\t\tLayerElement:  {\n\t\t\t\tType: \"LayerElementMaterial\"\n\t\t\t\tTypedIndex: 0\n\t\t\t}\n\t\t}";
			string text16 = string.Empty;
			if (flag)
			{
				text16 = "\n\t\tLayer: 1 {\n\t\t\tVersion: 100\n\t\t\tLayerElement:  {\n\t\t\t\ttype: \"LayerElementUV\"\n\t\t\t\tTypedIndex: 1\n\t\t\t}\n\t\t}";
			}
			return "\tModel: \"Model::" + mesh.name + "\", \"Mesh\" {\n\t\tVersion: 232\n\t\tProperties60:  {\n\t\t\tProperty: \"GeometricScaling\", \"Vector3D\", \"\",1,1,1\n\t\t\tProperty:\"Show\",\"bool\",\"\",1\n\t\t\tProperty:\"NegativePercentShapeSupport\",\"bool\",\"\",1\n\t\t\tProperty:\"DefaultAttributeIndex\",\"int\",\"\",0\n\t\t\tProperty:\"Visibility\",\"Visibility\",\"A+\",1\n\t\t\tProperty:\"Color\",\"ColorRGB\",\"N\",0.23921568627451,0.52156862745098,0.0235294117647059\n\t\t\tProperty: \"BBoxMin\",\"Vector3D\",\"N\",0,0,0\n\t\t\tProperty: \"BBoxMax\",\"Vector3D\",\"N\",0,0,0\n\t\t}\n\t\tMultiLayer: 1\n\t\tMultiTake: 1\n\t\tShading: T\n\t\tCulling: \"CullingOff\"\n\t\tVertices: " + text + text8 + "\n\t\tGeometryVersion: 124" + text9 + text10 + "\n\t\tLayerElementSmoothing: 0 {\n\t\t\tVersion: 102\n\t\t\tName: \"\"\n\t\t\tMappingInformationType: \"ByPolygon\"\n\t\t\tReferenceInformationType: \"Direct\"\n\t\t\tSmoothing: 1\n\t\t}" + text11 + text12 + text13 + text14 + text15 + text16 + "\n\t}\n";
		}

		private static Vector3 UnityToFbx(Vector3 rotation)
		{
			Vector3 vector = new Vector3(rotation.x, 0f - rotation.y, 0f - rotation.z);
			Quaternion quaternion = Quaternion.Euler(vector.x, vector.y, vector.z);
			float y;
			float z;
			float x;
			if (quaternion.x * quaternion.y + quaternion.z * quaternion.w > 0.4999f)
			{
				y = 2f * Mathf.Atan2(quaternion.x, quaternion.w) * 57.29578f;
				z = 90f;
				x = 0f;
			}
			else if (quaternion.x * quaternion.y + quaternion.z * quaternion.w < -0.4999f)
			{
				y = -2f * Mathf.Atan2(quaternion.x, quaternion.w) * 57.29578f;
				z = -90f;
				x = 0f;
			}
			else
			{
				y = Mathf.Atan2(2f * quaternion.y * quaternion.w - 2f * quaternion.x * quaternion.z, 1f - 2f * quaternion.y * quaternion.y - 2f * quaternion.z * quaternion.z) * 57.29578f;
				z = Mathf.Asin(2f * quaternion.x * quaternion.y + 2f * quaternion.z * quaternion.w) * 57.29578f;
				x = Mathf.Atan2(2f * quaternion.x * quaternion.w - 2f * quaternion.y * quaternion.z, 1f - 2f * quaternion.x * quaternion.x - 2f * quaternion.z * quaternion.z) * 57.29578f;
			}
			return new Vector3(x, y, z);
		}

		private static string SkinnedModelNodes(ref SkinnedMeshRenderer renderer)
		{
			string text = string.Empty;
			Transform[] bones = renderer.bones;
			foreach (Transform transform in bones)
			{
				Vector3 vector = UnityToFbx(transform.localEulerAngles);
				string text2 = text;
				text = text2 + "\tModel: \"Model::" + transform.name + "\", \"Limb\" {\n\t\tVersion: 232\n\t\tProperties60:  {\n\t\t\tProperty: \"Visibility\", \"Visibility\", \"A+\",1\n\t\t\tProperty: \"Lcl Translation\", \"Lcl Translation\", \"A+\"," + (0f - transform.localPosition.x).ToString("n15") + "," + transform.localPosition.y.ToString("n15") + "," + transform.localPosition.z.ToString("n15") + "\n\t\t\tProperty: \"Lcl Rotation\", \"Lcl Rotation\", \"A+\"," + vector.x.ToString("n15") + "," + vector.y.ToString("n15") + "," + vector.z.ToString("n15") + "\n\t\t\tProperty: \"Lcl Scaling\", \"Lcl Scaling\", \"A+\"," + transform.localScale.x.ToString("n15") + "," + transform.localScale.y.ToString("n15") + "," + transform.localScale.z.ToString("n15") + "\n\t\t}\n\t\tMultiLayer: 0\n\t\tCulling: \"CullingOff\"\n\t\tTypeFlags: \"Skeleton\"\n\t}\n";
			}
			return text;
		}

		private static string MaterialNodes(ref Renderer renderer)
		{
			string text = string.Empty;
			Material[] sharedMaterials = renderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (!(material == null))
				{
					Color color = Color.white;
					if (material.HasProperty("_Color"))
					{
						color = material.color;
					}
					string text2 = text;
					text = text2 + "\tMaterial: \"Material::" + material.name + "\", \"\" {\n\t\tVersion: 102\n\t\tShadingModel: \"lambert\"\n\t\tMultiLayer: 0\n\t\tProperties60:  {\n\t\t\tProperty: \"ShadingModel\", \"KString\", \"\", \"Lambert\"\n\t\t\tProperty: \"MultiLayer\", \"bool\", \"\",0\n\t\t\tProperty: \"EmissiveColor\", \"ColorRGB\", \"\",0.8000,0.8000,0.8000\n\t\t\tProperty: \"EmissiveFactor\", \"double\", \"\",0.0000\n\t\t\tProperty: \"AmbientColor\", \"ColorRGB\", \"\"," + color.r.ToString("n4") + "," + color.g.ToString("n4") + "," + color.b.ToString("n4") + "\n\t\t\tProperty: \"AmbientFactor\", \"double\", \"\",1.0000\n\t\t\tProperty: \"DiffuseColor\", \"ColorRGB\", \"\"," + color.r.ToString("n4") + "," + color.g.ToString("n4") + "," + color.b.ToString("n4") + "\n\t\t\tProperty: \"DiffuseFactor\", \"double\", \"\",0.8000\n\t\t\tProperty: \"Bump\", \"Vector3D\", \"\",0,0,0\n\t\t\tProperty: \"TransparentColor\", \"ColorRGB\", \"\",1,1,1\n\t\t\tProperty: \"TransparencyFactor\", \"double\", \"\",0.0000\n\t\t\tProperty: \"SpecularColor\", \"ColorRGB\", \"\",1.0000,1.0000,1.0000\n\t\t\tProperty: \"SpecularFactor\", \"double\", \"\",0.5000\n\t\t\tProperty: \"ShininessExponent\", \"double\", \"\",12.3\n\t\t\tProperty: \"ReflectionColor\", \"ColorRGB\", \"\",0,0,0\n\t\t\tProperty: \"ReflectionFactor\", \"double\", \"\",1\n\t\t\tProperty: \"Emissive\", \"ColorRGB\", \"\",0,0,0\n\t\t\tProperty: \"Ambient\", \"ColorRGB\", \"\",1.0,1.0,1.0" + color.r.ToString("n1") + "," + color.g.ToString("n1") + "," + color.b.ToString("n1") + "\n\t\t\tProperty: \"Diffuse\", \"ColorRGB\", \"\",0.8,0.8,0.8" + color.r.ToString("n1") + "," + color.g.ToString("n1") + "," + color.b.ToString("n1") + "\n\t\t\tProperty: \"Specular\", \"ColorRGB\", \"\",1.0,1.0,1.0\n\t\t\tProperty: \"Shininess\", \"double\", \"\",12.3\n\t\t\tProperty: \"Opacity\", \"double\", \"\",1.0\n\t\t\tProperty: \"Reflectivity\", \"double\", \"\",0\n\t\t}\n\t}\n";
				}
			}
			return text;
		}

		private static int MaterialNodeCount(ref Renderer renderer)
		{
			int num = 0;
			Material[] sharedMaterials = renderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (material != null)
				{
					num++;
				}
			}
			return num;
		}

		private static string TextureNodes(ref Renderer renderer)
		{
			string text = string.Empty;
			Material[] sharedMaterials = renderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (!(material == null))
				{
					if (material.HasProperty("_MainTex"))
					{
						string text2 = text;
						text = text2 + "\tTexture: \"Texture::" + material.name + "_diff\", \"\" {\n\t\tType: \"TextureVideoClip\"\n\t\tVersion: 202\n\t\tTextureName: \"Texture::" + material.name + "_diff\"\n\t\tProperties60:  {\n\t\t\tProperty: \"TextureTypeUse\", \"enum\", \"\",0\n\t\t\tProperty: \"Texture alpha\", \"Number\", \"A+\",1\n\t\t\tProperty: \"CurrentMappingType\", \"enum\", \"\",0\n\t\t\tProperty: \"WrapModeU\", \"enum\", \"\",0\n\t\t\tProperty: \"WrapModeV\", \"enum\", \"\",0\n\t\t\tProperty: \"UVSwap\", \"bool\", \"\",0\n\t\t\tProperty: \"Translation\", \"Vector\", \"A+\"," + material.mainTextureOffset.x + "," + material.mainTextureOffset.y + ",0\n\t\t\tProperty: \"Rotation\", \"Vector\", \"A+\",0,0,0\n\t\t\tProperty: \"Scaling\", \"Vector\", \"A+\"," + material.mainTextureScale.x + "," + material.mainTextureScale.y + ",1\n\t\t\tProperty: \"TextureRotationPivot\", \"Vector3D\", \"\",0,0,0\n\t\t\tProperty: \"TextureScalingPivot\", \"Vector3D\", \"\",0,0,0\n\t\t\tProperty: \"UseMaterial\", \"bool\", \"\",1\n\t\t\tProperty: \"UseMipMap\", \"bool\", \"\",0\n\t\t\tProperty: \"CurrentTextureBlendMode\", \"enum\", \"\",1\n\t\t\tProperty: \"UVSet\", \"KString\", \"\", \"UVChannel_1\"\n\t\t}\n\t\tModelUVTranslation: 0,0\n\t\tModelUVScaling: 1,1\n\t\tTexture_Alpha_Source: \"None\"\n\t\tCropping: 0,0,0,0\n\t}\n";
					}
					if (material.HasProperty("_BumpMap"))
					{
						string text2 = text;
						text = text2 + "\tTexture: \"Texture::" + material.name + "_bump\", \"\" {\n\t\tType: \"TextureVideoClip\"\n\t\tVersion: 202\n\t\tTextureName: \"Texture::" + material.name + "_bump\"\n\t\tProperties60:  {\n\t\t\tProperty: \"TextureTypeUse\", \"enum\", \"\",0\n\t\t\tProperty: \"Texture alpha\", \"Number\", \"A+\",1\n\t\t\tProperty: \"CurrentMappingType\", \"enum\", \"\",0\n\t\t\tProperty: \"WrapModeU\", \"enum\", \"\",0\n\t\t\tProperty: \"WrapModeV\", \"enum\", \"\",0\n\t\t\tProperty: \"UVSwap\", \"bool\", \"\",0\n\t\t\tProperty: \"Translation\", \"Vector\", \"A+\"," + material.GetTextureOffset("_BumpMap").x + "," + material.GetTextureOffset("_BumpMap").y + ",0\n\t\t\tProperty: \"Rotation\", \"Vector\", \"A+\",0,0,0\n\t\t\tProperty: \"Scaling\", \"Vector\", \"A+\"," + material.GetTextureScale("_BumpMap").x + "," + material.GetTextureScale("_BumpMap").y + ",1\n\t\t\tProperty: \"TextureRotationPivot\", \"Vector3D\", \"\",0,0,0\n\t\t\tProperty: \"TextureScalingPivot\", \"Vector3D\", \"\",0,0,0\n\t\t\tProperty: \"UseMaterial\", \"bool\", \"\",1\n\t\t\tProperty: \"UseMipMap\", \"bool\", \"\",0\n\t\t\tProperty: \"CurrentTextureBlendMode\", \"enum\", \"\",1\n\t\t\tProperty: \"UVSet\", \"KString\", \"\", \"UVChannel_1\"\n\t\t}\n\t\tModelUVTranslation: 0,0\n\t\tModelUVScaling: 1,1\n\t\tTexture_Alpha_Source: \"None\"\n\t\tCropping: 0,0,0,0\n\t}\n";
					}
				}
			}
			return text;
		}

		private static int TextureNodeCount(ref Renderer renderer)
		{
			int num = 0;
			Material[] sharedMaterials = renderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (material.HasProperty("_MainTex"))
				{
					num++;
				}
				if (material.HasProperty("_BumpMap"))
				{
					num++;
				}
			}
			return num;
		}

		private static string DeformationNodes(ref Mesh mesh, ref SkinnedMeshRenderer renderer)
		{
			string empty = string.Empty;
			FbxContainerNode fbxContainerNode = new FbxContainerNode(1, "Deformer", "\"Deformer::Skin " + mesh.name + "\", \"Skin\"", new FbxNode[4]);
			fbxContainerNode.subnodes[0] = new FbxNode(2, "Version", "100");
			fbxContainerNode.subnodes[1] = new FbxNode(2, "Multilayer", "0");
			fbxContainerNode.subnodes[2] = new FbxNode(2, "Type", "\"Skin\"");
			fbxContainerNode.subnodes[3] = new FbxNode(2, "Link_DeformAcuracy", "50");
			empty += fbxContainerNode.ToString();
			for (int i = 0; i < renderer.bones.Length; i++)
			{
				FbxContainerNode fbxContainerNode2 = new FbxContainerNode(1, "Deformer", "\"SubDeformer::Cluster " + mesh.name + " " + renderer.bones[i].name + "\", \"Cluster\"", new FbxNode[6]);
				fbxContainerNode2.subnodes[0] = new FbxNode(2, "Version", "100");
				fbxContainerNode2.subnodes[1] = new FbxNode(2, "MultiLayer", "0");
				fbxContainerNode2.subnodes[2] = new FbxNode(2, "Type", "Cluster");
				fbxContainerNode2.subnodes[3] = new FbxNode(2, "Indexes", string.Empty);
				fbxContainerNode2.subnodes[4] = new FbxNode(2, "Weights", string.Empty);
				for (int j = 0; j < mesh.vertexCount; j++)
				{
					BoneWeight boneWeight = mesh.boneWeights[j];
					if (boneWeight.boneIndex0 == i)
					{
						FbxNode obj = fbxContainerNode2.subnodes[3];
						obj.val = obj.val + j + ",";
						FbxNode obj2 = fbxContainerNode2.subnodes[4];
						obj2.val = obj2.val + boneWeight.weight0 + ",";
					}
					else if (boneWeight.boneIndex1 == i)
					{
						FbxNode obj3 = fbxContainerNode2.subnodes[3];
						obj3.val = obj3.val + j + ",";
						FbxNode obj4 = fbxContainerNode2.subnodes[4];
						obj4.val = obj4.val + boneWeight.weight1.ToString("n6") + ",";
					}
					else if (boneWeight.boneIndex2 == i)
					{
						FbxNode obj5 = fbxContainerNode2.subnodes[3];
						obj5.val = obj5.val + j + ",";
						FbxNode obj6 = fbxContainerNode2.subnodes[4];
						obj6.val = obj6.val + boneWeight.weight2.ToString("n6") + ",";
					}
					else if (boneWeight.boneIndex3 == i)
					{
						FbxNode obj7 = fbxContainerNode2.subnodes[3];
						obj7.val = obj7.val + j + ",";
						FbxNode obj8 = fbxContainerNode2.subnodes[4];
						obj8.val = obj8.val + boneWeight.weight3.ToString("n6") + ",";
					}
				}
				fbxContainerNode2.subnodes[5] = new FbxNode(2, "Transform", string.Empty);
				int k;
				for (k = 0; k < 16; k++)
				{
					int num = ((!Array.Exists(new int[6] { 1, 2, 3, 4, 8, 12 }, (int p) => p == k)) ? 1 : (-1));
					FbxNode obj9 = fbxContainerNode2.subnodes[5];
					obj9.val = obj9.val + ((float)num * mesh.bindposes[i][k]).ToString("n15") + ",";
				}
				fbxContainerNode2.subnodes[3].val = fbxContainerNode2.subnodes[3].val.TrimEnd(',');
				fbxContainerNode2.subnodes[4].val = fbxContainerNode2.subnodes[4].val.TrimEnd(',');
				fbxContainerNode2.subnodes[5].val = fbxContainerNode2.subnodes[5].val.TrimEnd(',');
				empty += fbxContainerNode2.ToString();
			}
			return empty;
		}

		private static FbxNode GlobalSettingsNode()
		{
			return new FbxNode(0, "GlobalSettings", string.Empty, "\t\tVersion: 1000\n\t\tProperties60:  {\n\t\t\tProperty: \"UpAxis\", \"int\", \"\",1\n\t\t\tProperty: \"UpAxisSign\", \"int\", \"\",1\n\t\t\tProperty: \"FrontAxis\", \"int\", \"\",2\n\t\t\tProperty: \"FrontAxisSign\", \"int\", \"\",1\n\t\t\tProperty: \"CoordAxis\", \"int\", \"\",0\n\t\t\tProperty: \"CoordAxisSign\", \"int\", \"\",1\n\t\t\tProperty: \"UnitScaleFactor\", \"double\", \"\",100\n\t\t}\n");
		}
	}
}
