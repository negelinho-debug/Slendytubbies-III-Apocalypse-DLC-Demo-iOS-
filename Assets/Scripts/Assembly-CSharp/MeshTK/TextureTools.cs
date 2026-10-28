using System.IO;
using UnityEngine;

namespace MeshTK
{
	public class TextureTools
	{
		public static void DrawLine(Texture2D tex, int x0, int y0, int x1, int y1, Color col)
		{
			int num = y1 - y0;
			int num2 = x1 - x0;
			int num3;
			if (num < 0)
			{
				num = -num;
				num3 = -1;
			}
			else
			{
				num3 = 1;
			}
			int num4;
			if (num2 < 0)
			{
				num2 = -num2;
				num4 = -1;
			}
			else
			{
				num4 = 1;
			}
			num <<= 1;
			num2 <<= 1;
			float num5 = 0f;
			tex.SetPixel(x0, y0, col);
			if (num2 > num)
			{
				num5 = num - (num2 >> 1);
				while (Mathf.Abs(x0 - x1) > 1)
				{
					if (num5 >= 0f)
					{
						y0 += num3;
						num5 -= (float)num2;
					}
					x0 += num4;
					num5 += (float)num;
					tex.SetPixel(x0, y0, col);
				}
				return;
			}
			num5 = num2 - (num >> 1);
			while (Mathf.Abs(y0 - y1) > 1)
			{
				if (num5 >= 0f)
				{
					x0 += num4;
					num5 -= (float)num;
				}
				y0 += num3;
				num5 += (float)num2;
				tex.SetPixel(x0, y0, col);
			}
		}

		public static void SaveTexture(Texture2D tex, string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				byte[] bytes = tex.EncodeToPNG();
				File.WriteAllBytes(path, bytes);
			}
		}
	}
}
