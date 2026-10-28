using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace AC
{
	public class Serializer : MonoBehaviour
	{
		public static T returnComponent<T>(int constantID) where T : Behaviour
		{
			if (constantID != 0)
			{
				T[] array = UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
				T[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					T result = array2[i];
					if (!result.GetComponent<ConstantID>())
					{
						continue;
					}
					ConstantID[] components = result.GetComponents<ConstantID>();
					ConstantID[] array3 = components;
					foreach (ConstantID constantID2 in array3)
					{
						if (constantID2.constantID == constantID)
						{
							return result;
						}
					}
				}
			}
			return (T)null;
		}

		public static int GetConstantID(GameObject _gameObject)
		{
			if ((bool)_gameObject.GetComponent<ConstantID>())
			{
				if (_gameObject.GetComponent<ConstantID>().constantID != 0)
				{
					return _gameObject.GetComponent<ConstantID>().constantID;
				}
				Debug.LogWarning("GameObject " + _gameObject.name + " was not saved because it does not have an ID.");
			}
			else
			{
				Debug.LogWarning("GameObject " + _gameObject.name + " was not saved because it does not have a constant ID script!");
			}
			return 0;
		}

		public static string SerializeObjectBinary(object pObject)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, pObject);
			return Convert.ToBase64String(memoryStream.GetBuffer());
		}

		public static T DeserializeObjectBinary<T>(string pString)
		{
			if (pString.Contains("<?xml"))
			{
				PlayerPrefs.DeleteKey("Options");
				return default(T);
			}
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(pString));
			return (T)binaryFormatter.Deserialize(serializationStream);
		}

		public static string SerializeObjectXML<T>(object pObject)
		{
			string text = null;
			MemoryStream stream = new MemoryStream();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlSerializer.Serialize(xmlTextWriter, pObject);
			stream = (MemoryStream)xmlTextWriter.BaseStream;
			return UTF8ByteArrayToString(stream.ToArray());
		}

		public static object DeserializeObjectXML<T>(string pXmlizedString)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			MemoryStream stream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
			return xmlSerializer.Deserialize(stream);
		}

		private static string UTF8ByteArrayToString(byte[] characters)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			return uTF8Encoding.GetString(characters, 0, characters.Length);
		}

		private static byte[] StringToUTF8ByteArray(string pXmlString)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			return uTF8Encoding.GetBytes(pXmlString);
		}

		public static List<SingleLevelData> DeserializeRoom(string pString)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(pString));
			return (List<SingleLevelData>)binaryFormatter.Deserialize(serializationStream);
		}

		public static void CreateSaveFile(string fullFileName, string _data)
		{
			FileInfo fileInfo = new FileInfo(fullFileName);
			StreamWriter streamWriter;
			if (!fileInfo.Exists)
			{
				streamWriter = fileInfo.CreateText();
			}
			else
			{
				fileInfo.Delete();
				streamWriter = fileInfo.CreateText();
			}
			streamWriter.Write(_data);
			streamWriter.Close();
			Debug.Log("File written: " + fullFileName);
		}

		public static string LoadSaveFile(string fullFileName)
		{
			string empty = string.Empty;
			StreamReader streamReader = File.OpenText(fullFileName);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			empty = text;
			Debug.Log("File Read: " + fullFileName);
			return empty;
		}

		public static Paths RestorePathData(Paths path, string pathData)
		{
			if (pathData == null)
			{
				return null;
			}
			path.affectY = true;
			path.pathType = AC_PathType.ForwardOnly;
			path.nodePause = 0f;
			path.nodes = new List<Vector3>();
			if (pathData.Length > 0)
			{
				string[] array = pathData.Split("|"[0]);
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(":"[0]);
					float result = 0f;
					float.TryParse(array3[0], out result);
					float result2 = 0f;
					float.TryParse(array3[1], out result2);
					float result3 = 0f;
					float.TryParse(array3[2], out result3);
					path.nodes.Add(new Vector3(result, result2, result3));
				}
			}
			return path;
		}

		public static string CreatePathData(Paths path)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Vector3 node in path.nodes)
			{
				stringBuilder.Append(node.x.ToString());
				stringBuilder.Append(":");
				stringBuilder.Append(node.y.ToString());
				stringBuilder.Append(":");
				stringBuilder.Append(node.z.ToString());
				stringBuilder.Append("|");
			}
			if (path.nodes.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		public static void SaveScreenshot(Texture2D screenshotTex, string fileName)
		{
			byte[] bytes = screenshotTex.EncodeToJPG();
			File.WriteAllBytes(fileName, bytes);
		}

		public static Texture2D LoadScreenshot(string fileName)
		{
			if (File.Exists(fileName))
			{
				byte[] data = File.ReadAllBytes(fileName);
				Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
				texture2D.LoadImage(data);
				return texture2D;
			}
			return null;
		}
	}
}
