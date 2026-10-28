using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using UnityEngine;

namespace AC
{
	public class AdvGame : ScriptableObject
	{
		public static List<Action> copiedActions = new List<Action>();

		private static References references = null;

		public static string GetSceneName(ChooseSceneBy chooseSceneBy, string sceneName)
		{
			if (chooseSceneBy == ChooseSceneBy.Number)
			{
				return string.Empty;
			}
			return sceneName;
		}

		public static int GetAnimLayerInt(AnimLayer animLayer)
		{
			int num = (int)animLayer;
			if (num > 4)
			{
				num++;
			}
			return num;
		}

		public static References GetReferences()
		{
			if (references == null)
			{
				references = (References)Resources.Load("References");
			}
			return references;
		}

		public static RuntimeActionList RunActionListAsset(ActionListAsset actionListAsset)
		{
			return RunActionListAsset(actionListAsset, null, 0);
		}

		public static RuntimeActionList RunActionListAsset(ActionListAsset actionListAsset, int i)
		{
			return RunActionListAsset(actionListAsset, null, i);
		}

		public static RuntimeActionList RunActionListAsset(ActionListAsset actionListAsset, Conversation endConversation)
		{
			return RunActionListAsset(actionListAsset, endConversation, 0);
		}

		public static RuntimeActionList RunActionListAsset(ActionListAsset actionListAsset, Conversation endConversation, int i)
		{
			if (actionListAsset != null && actionListAsset.actions.Count > 0)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("RuntimeActionList"));
				RuntimeActionList component = gameObject.GetComponent<RuntimeActionList>();
				component.DownloadActions(actionListAsset, endConversation, i);
				return component;
			}
			return null;
		}

		public static void DrawCubeCollider(Transform transform, Color color)
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = color;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
		}

		public static void DrawBoxCollider(Transform transform, Color color)
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = color;
			Gizmos.DrawLine(new Vector3(-0.5f, -0.5f), new Vector3(-0.5f, 0.5f));
			Gizmos.DrawLine(new Vector3(-0.5f, 0.5f), new Vector3(0.5f, 0.5f));
			Gizmos.DrawLine(new Vector3(0.5f, 0.5f), new Vector3(0.5f, -0.5f));
			Gizmos.DrawLine(new Vector3(0.5f, -0.5f), new Vector3(-0.5f, -0.5f));
		}

		public static void DrawPolygonCollider(Transform transform, PolygonCollider2D poly, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(transform.TransformPoint(poly.points[0]), transform.TransformPoint(poly.points[poly.points.Length - 1]));
			for (int i = 0; i < poly.points.Length - 1; i++)
			{
				Gizmos.DrawLine(transform.TransformPoint(poly.points[i]), transform.TransformPoint(poly.points[i + 1]));
			}
		}

		public static double CalculateFormula(string formula)
		{
			return (double)new XPathDocument(new StringReader("<r/>")).CreateNavigator().Evaluate(string.Format("number({0})", new Regex("([\\+\\-\\*])").Replace(formula, " ${1} ").Replace("/", " div ").Replace("%", " mod ")));
		}

		public static string ConvertTokens(string _text)
		{
			if (!Application.isPlaying)
			{
				return _text;
			}
			if (_text != null && _text.Contains("var:"))
			{
				if (_text.Contains("[var:"))
				{
					RuntimeVariables component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>();
					foreach (GVar globalVar in component.globalVars)
					{
						string text = "[var:" + globalVar.id + "]";
						if (_text.Contains(text))
						{
							globalVar.Download();
							_text = _text.Replace(text, globalVar.GetValue());
						}
					}
				}
				if (_text.Contains("[localvar:"))
				{
					LocalVariables component2 = GameObject.FindWithTag("GameEngine").GetComponent<LocalVariables>();
					foreach (GVar localVar in component2.localVars)
					{
						string text2 = "[localvar:" + localVar.id + "]";
						if (_text.Contains(text2))
						{
							_text = _text.Replace(text2, localVar.GetValue());
						}
					}
				}
			}
			return _text;
		}

		public static Vector3 GetScreenDirection(Vector3 originWorldPosition, Vector3 targetWorldPosition)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(originWorldPosition);
			Vector3 vector2 = Camera.main.WorldToScreenPoint(targetWorldPosition);
			Vector3 result = vector2 - vector;
			result.z = result.y;
			result.y = 0f;
			return result;
		}

		public static Vector3 GetScreenNavMesh(Vector3 targetWorldPosition)
		{
			SettingsManager settingsManager = GetReferences().settingsManager;
			Vector3 position = Camera.main.WorldToScreenPoint(targetWorldPosition);
			Ray ray = Camera.main.ScreenPointToRay(position);
			RaycastHit hitInfo = default(RaycastHit);
			if ((bool)settingsManager && Physics.Raycast(ray, out hitInfo, settingsManager.navMeshRaycastLength, 1 << LayerMask.NameToLayer(settingsManager.navMeshLayer)))
			{
				return hitInfo.point;
			}
			return targetWorldPosition;
		}

		public static Vector2 GetMainGameViewSize()
		{
			if (Application.isPlaying)
			{
				return new Vector2(Screen.width, Screen.height);
			}
			Type type = Type.GetType("UnityEditor.GameView, UnityEditor");
			MethodInfo method = type.GetMethod("GetSizeOfMainGameView", BindingFlags.Static | BindingFlags.NonPublic);
			object obj = method.Invoke(null, null);
			return (Vector2)obj;
		}

		public static Matrix4x4 SetVanishingPoint(Camera _camera, Vector2 perspectiveOffset)
		{
			Matrix4x4 projectionMatrix = _camera.projectionMatrix;
			float num = 2f * _camera.nearClipPlane / projectionMatrix.m00;
			float num2 = 2f * _camera.nearClipPlane / projectionMatrix.m11;
			float num3 = 0f - num / 2f + perspectiveOffset.x;
			float right = num3 + num;
			float num4 = 0f - num2 / 2f + perspectiveOffset.y;
			float top = num4 + num2;
			return PerspectiveOffCenter(num3, right, num4, top, _camera.nearClipPlane, _camera.farClipPlane);
		}

		private static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
		{
			float value = 2f * near / (right - left);
			float value2 = 2f * near / (top - bottom);
			float value3 = (right + left) / (right - left);
			float value4 = (top + bottom) / (top - bottom);
			float value5 = (0f - (far + near)) / (far - near);
			float value6 = (0f - 2f * far * near) / (far - near);
			float value7 = -1f;
			return new Matrix4x4
			{
				[0, 0] = value,
				[0, 1] = 0f,
				[0, 2] = value3,
				[0, 3] = 0f,
				[1, 0] = 0f,
				[1, 1] = value2,
				[1, 2] = value4,
				[1, 3] = 0f,
				[2, 0] = 0f,
				[2, 1] = 0f,
				[2, 2] = value5,
				[2, 3] = value6,
				[3, 0] = 0f,
				[3, 1] = 0f,
				[3, 2] = value7,
				[3, 3] = 0f
			};
		}

		public static string UniqueName(string name)
		{
			if ((bool)GameObject.Find(name))
			{
				string result = name;
				for (int i = 2; i < 20; i++)
				{
					result = name + i;
					if (!GameObject.Find(result))
					{
						break;
					}
				}
				return result;
			}
			return name;
		}

		public static string GetName(string resourceName)
		{
			int num = resourceName.IndexOf("/");
			if (num > 0)
			{
				return resourceName.Remove(0, num + 1);
			}
			return resourceName;
		}

		public static Rect GUIBox(float centre_x, float centre_y, float size)
		{
			return GUIRect(centre_x, centre_y, size, size);
		}

		public static Rect GUIRect(float centre_x, float centre_y, float width, float height)
		{
			return new Rect((float)Screen.width * centre_x - (float)Screen.width * width / 2f, (float)Screen.height * centre_y - (float)Screen.width * height / 2f, (float)Screen.width * width, (float)Screen.width * height);
		}

		public static Rect GUIBox(Vector2 posVector, float size)
		{
			return GUIRect(posVector.x / (float)Screen.width, ((float)Screen.height - posVector.y) / (float)Screen.height, size, size);
		}

		public static void AddAnimClip(Animation _animation, int layer, AnimationClip clip, AnimationBlendMode blendMode, WrapMode wrapMode, Transform mixingBone)
		{
			if (clip != null)
			{
				_animation.AddClip(clip, clip.name);
				if (mixingBone != null)
				{
					_animation[clip.name].AddMixingTransform(mixingBone);
				}
				_animation[clip.name].layer = layer;
				_animation[clip.name].normalizedTime = 0f;
				_animation[clip.name].blendMode = blendMode;
				_animation[clip.name].wrapMode = wrapMode;
				_animation[clip.name].enabled = true;
			}
		}

		public static void PlayAnimClipFrame(Animation _animation, int layer, AnimationClip clip, AnimationBlendMode blendMode, WrapMode wrapMode, float fadeTime, Transform mixingBone, float normalisedFrame)
		{
			if (clip != null)
			{
				AddAnimClip(_animation, layer, clip, blendMode, wrapMode, mixingBone);
				_animation[clip.name].normalizedTime = normalisedFrame;
				_animation[clip.name].speed *= 1f;
				_animation.Play(clip.name);
				CleanUnusedClips(_animation);
			}
		}

		public static void PlayAnimClip(Animation _animation, int layer, AnimationClip clip, AnimationBlendMode blendMode, WrapMode wrapMode, float fadeTime, Transform mixingBone, bool reverse)
		{
			if (clip != null)
			{
				AddAnimClip(_animation, layer, clip, blendMode, wrapMode, mixingBone);
				if (reverse)
				{
					_animation[clip.name].speed *= -1f;
				}
				_animation.CrossFade(clip.name, fadeTime);
				CleanUnusedClips(_animation);
			}
		}

		public static void CleanUnusedClips(Animation _animation)
		{
			List<string> list = new List<string>();
			foreach (AnimationState item in _animation)
			{
				if (!_animation[item.name].enabled)
				{
					int num = item.name.IndexOf(" - Queued Clone");
					if (num > 0)
					{
						list.Add(item.name.Substring(0, num));
					}
					else
					{
						list.Add(item.name);
					}
				}
			}
			foreach (string item2 in list)
			{
				_animation.RemoveClip(item2);
			}
		}

		public static float Lerp(float from, float to, float t)
		{
			if (t <= 1f)
			{
				return Mathf.Lerp(from, to, t);
			}
			return from + (to - from) * t;
		}

		public static Vector3 Lerp(Vector3 from, Vector3 to, float t)
		{
			if (t <= 1f)
			{
				return Vector3.Lerp(from, to, t);
			}
			return from + (to - from) * t;
		}

		public static Quaternion Lerp(Quaternion from, Quaternion to, float t)
		{
			if (t <= 1f)
			{
				return Quaternion.Lerp(from, to, t);
			}
			Vector3 eulerAngles = from.eulerAngles;
			Vector3 eulerAngles2 = to.eulerAngles;
			if (eulerAngles.x - eulerAngles2.x > 180f)
			{
				eulerAngles2.x -= 360f;
			}
			else if (eulerAngles.x - eulerAngles2.x > 180f)
			{
				eulerAngles2.x += 360f;
			}
			if (eulerAngles.y - eulerAngles2.y < -180f)
			{
				eulerAngles2.y -= 360f;
			}
			else if (eulerAngles.y - eulerAngles2.y > 180f)
			{
				eulerAngles2.y += 360f;
			}
			if (eulerAngles.z - eulerAngles2.z > 180f)
			{
				eulerAngles2.z -= 360f;
			}
			else if (eulerAngles.z - eulerAngles2.z > 180f)
			{
				eulerAngles2.z += 360f;
			}
			return Quaternion.Euler(Lerp(eulerAngles, eulerAngles2, t));
		}

		public static float Interpolate(float startT, float deltaT, MoveMethod moveMethod, AnimationCurve timeCurve)
		{
			switch (moveMethod)
			{
			case MoveMethod.Curved:
				moveMethod = MoveMethod.Smooth;
				break;
			case MoveMethod.Smooth:
				return -0.5f * (Mathf.Cos((float)Math.PI * (Time.time - startT) / deltaT) - 1f);
			case MoveMethod.EaseIn:
				return 1f - Mathf.Cos((Time.time - startT) / deltaT * ((float)Math.PI / 2f));
			case MoveMethod.EaseOut:
				return Mathf.Sin((Time.time - startT) / deltaT * ((float)Math.PI / 2f));
			case MoveMethod.CustomCurve:
			{
				if (timeCurve == null || timeCurve.length == 0)
				{
					return 1f;
				}
				float time = timeCurve[0].time;
				float time2 = timeCurve[timeCurve.length - 1].time;
				return timeCurve.Evaluate((time2 - time) * (Time.time - startT) / deltaT + time);
			}
			}
			return (Time.time - startT) / deltaT;
		}

		public static Rect Rescale(Rect _rect)
		{
			float f = (float)Screen.width / 884f;
			int num = Mathf.RoundToInt(f);
			return new Rect(_rect.x * (float)num, _rect.y * (float)num, _rect.width * (float)num, _rect.height * (float)num);
		}

		public static int Rescale(int _int)
		{
			float f = (float)Screen.width / 884f;
			int num = Mathf.RoundToInt(f);
			return _int * num;
		}

		public static void DrawTextEffect(Rect rect, string text, GUIStyle style, Color outColor, Color inColor, float size, TextEffects textEffects)
		{
			if (textEffects == TextEffects.Outline || textEffects == TextEffects.OutlineAndShadow)
			{
				DrawTextOutline(rect, text, style, outColor, inColor, size);
			}
			if (textEffects == TextEffects.Shadow || textEffects == TextEffects.OutlineAndShadow)
			{
				DrawTextShadow(rect, text, style, outColor, inColor, size);
			}
		}

		private static void DrawTextShadow(Rect rect, string text, GUIStyle style, Color outColor, Color inColor, float size)
		{
			GUIStyle gUIStyle = new GUIStyle(style);
			Color color = GUI.color;
			outColor.a = GUI.color.a;
			style.normal.textColor = outColor;
			GUI.color = outColor;
			rect.x += size;
			GUI.Label(rect, text, style);
			rect.y += size;
			GUI.Label(rect, text, style);
			rect.x -= size;
			rect.y -= size;
			style.normal.textColor = inColor;
			GUI.color = color;
			GUI.Label(rect, text, style);
			style = gUIStyle;
		}

		private static void DrawTextOutline(Rect rect, string text, GUIStyle style, Color outColor, Color inColor, float size)
		{
			float num = size * 0.5f;
			GUIStyle gUIStyle = new GUIStyle(style);
			Color color = GUI.color;
			outColor.a = GUI.color.a;
			style.normal.textColor = outColor;
			GUI.color = outColor;
			rect.x -= num;
			GUI.Label(rect, text, style);
			rect.x += size;
			GUI.Label(rect, text, style);
			rect.x -= num;
			rect.y -= num;
			GUI.Label(rect, text, style);
			rect.y += size;
			GUI.Label(rect, text, style);
			rect.y -= num;
			style.normal.textColor = inColor;
			GUI.color = color;
			GUI.Label(rect, text, style);
			style = gUIStyle;
		}
	}
}
