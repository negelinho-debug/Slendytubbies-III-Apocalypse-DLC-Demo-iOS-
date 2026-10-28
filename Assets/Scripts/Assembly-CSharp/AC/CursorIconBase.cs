using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class CursorIconBase
	{
		public Texture2D texture;

		public bool isAnimated;

		public int numFrames = 1;

		public int numRows = 1;

		public int numCols = 1;

		public float size = 0.015f;

		public float animSpeed = 4f;

		public bool endAnimOnLastFrame;

		public Vector2 clickOffset;

		private string uniqueIdentifier;

		private float frameIndex;

		private float frameWidth = -1f;

		private float frameHeight = -1f;

		public CursorIconBase()
		{
			texture = null;
			isAnimated = false;
			numFrames = (numRows = (numCols = 1));
			size = 0.015f;
			frameIndex = 0f;
			frameWidth = (frameHeight = -1f);
			animSpeed = 4f;
			endAnimOnLastFrame = false;
			clickOffset = Vector2.zero;
		}

		public void Copy(CursorIconBase _icon)
		{
			texture = _icon.texture;
			isAnimated = _icon.isAnimated;
			numFrames = _icon.numFrames;
			animSpeed = _icon.animSpeed;
			endAnimOnLastFrame = _icon.endAnimOnLastFrame;
			clickOffset = _icon.clickOffset;
			numRows = _icon.numRows;
			numCols = _icon.numCols;
		}

		public void DrawAsInteraction(Rect _rect, bool isActive)
		{
			if (texture == null)
			{
				return;
			}
			if (isAnimated && numFrames > 0)
			{
				if (Application.isPlaying)
				{
					if (isActive)
					{
						GUI.DrawTextureWithTexCoords(_rect, texture, GetAnimatedRect());
						return;
					}
					GUI.DrawTextureWithTexCoords(_rect, texture, new Rect(0f, 1f - frameHeight, frameWidth, frameHeight));
					frameIndex = 0f;
				}
				else
				{
					Reset();
					GUI.DrawTextureWithTexCoords(_rect, texture, new Rect(0f, 1f - frameHeight, frameWidth, 1f - frameHeight));
					frameIndex = 0f;
				}
			}
			else
			{
				GUI.DrawTexture(_rect, texture, ScaleMode.StretchToFill, true, 0f);
			}
		}

		public Texture2D GetAnimatedTexture()
		{
			if (texture == null)
			{
				return null;
			}
			if (isAnimated)
			{
				Rect animatedRect = GetAnimatedRect();
				int x = Mathf.FloorToInt(animatedRect.x * (float)texture.width);
				int y = Mathf.FloorToInt(animatedRect.y * (float)texture.height);
				int blockWidth = Mathf.FloorToInt(animatedRect.width * (float)texture.width);
				int blockHeight = Mathf.FloorToInt(animatedRect.height * (float)texture.height);
				Color[] pixels = texture.GetPixels(x, y, blockWidth, blockHeight);
				Texture2D texture2D = new Texture2D((int)(frameWidth * (float)texture.width), (int)(frameHeight * (float)texture.height));
				texture2D.SetPixels(pixels);
				texture2D.Apply();
				return texture2D;
			}
			return texture;
		}

		public string GetName()
		{
			return uniqueIdentifier;
		}

		public void Draw(Vector2 centre)
		{
			if (!(texture == null))
			{
				Rect position = AdvGame.GUIBox(centre, size);
				position.x -= clickOffset.x * position.width;
				position.y -= clickOffset.y * position.height;
				if (isAnimated && numFrames > 0 && Application.isPlaying)
				{
					GUI.DrawTextureWithTexCoords(position, texture, GetAnimatedRect());
				}
				else
				{
					GUI.DrawTexture(position, texture, ScaleMode.ScaleToFit, true, 0f);
				}
			}
		}

		public Rect GetAnimatedRect()
		{
			int num = 1;
			int num2 = 1;
			if (frameIndex < 0f)
			{
				frameIndex = 0f;
			}
			else if (frameIndex < (float)numFrames && (!endAnimOnLastFrame || !(frameIndex >= (float)(numFrames - 1))))
			{
				if (Time.deltaTime == 0f)
				{
					frameIndex += 0.02f * animSpeed;
				}
				else
				{
					frameIndex += Time.deltaTime * animSpeed;
				}
			}
			num2 = Mathf.FloorToInt(frameIndex) + 1;
			while (num2 > numCols)
			{
				num2 -= numCols;
				num++;
			}
			if (frameIndex >= (float)numFrames)
			{
				if (!endAnimOnLastFrame)
				{
					frameIndex = 0f;
					num2 = 1;
					num = 1;
				}
				else
				{
					frameIndex = numFrames - 1;
					num2--;
				}
			}
			if (texture != null)
			{
				uniqueIdentifier = texture.name + num2 + num;
			}
			return new Rect(frameWidth * (float)(num2 - 1), frameHeight * (float)(numRows - num), frameWidth, frameHeight);
		}

		public Rect GetAnimatedRect(int _frameIndex)
		{
			int num = _frameIndex + 1;
			int num2 = 1;
			while (num > numCols)
			{
				num -= numCols;
				num2++;
			}
			if (_frameIndex >= numFrames)
			{
				num = 1;
				num2 = 1;
			}
			return new Rect(frameWidth * (float)(num - 1), frameHeight * (float)(numRows - num2), frameWidth, frameHeight);
		}

		public void Reset()
		{
			if (isAnimated)
			{
				if (numFrames > 0)
				{
					frameWidth = 1f / (float)numCols;
					frameHeight = 1f / (float)numRows;
					frameIndex = 0f;
				}
				else
				{
					Debug.LogWarning("Cannot have an animated cursor with less than one frame!");
				}
				if (animSpeed < 0f)
				{
					animSpeed = 0f;
				}
			}
		}
	}
}
