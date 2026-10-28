using UnityEngine;

namespace AC
{
	public class MenuSlider : MenuElement
	{
		public string label;

		public TextEffects textEffects;

		public float amount;

		public TextAnchor anchor;

		public Texture2D sliderTexture;

		public SliderDisplayType sliderDisplayType;

		public AC_SliderType sliderType;

		public Vector2 blockSize = new Vector2(0.05f, 1f);

		public bool useFullWidth;

		public int varID;

		public int numberOfSteps;

		private Rect sliderRect;

		public override void Declare()
		{
			label = "Slider";
			isVisible = true;
			isClickable = true;
			numSlots = 1;
			amount = 1f;
			anchor = TextAnchor.MiddleLeft;
			sliderType = AC_SliderType.CustomScript;
			sliderDisplayType = SliderDisplayType.FillBar;
			blockSize = new Vector2(0.05f, 1f);
			useFullWidth = false;
			varID = 0;
			textEffects = TextEffects.None;
			numberOfSteps = 0;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuSlider menuSlider = ScriptableObject.CreateInstance<MenuSlider>();
			menuSlider.Declare();
			menuSlider.CopySlider(this);
			return menuSlider;
		}

		public void CopySlider(MenuSlider _element)
		{
			label = _element.label;
			isClickable = _element.isClickable;
			textEffects = _element.textEffects;
			amount = _element.amount;
			anchor = _element.anchor;
			sliderTexture = _element.sliderTexture;
			sliderType = _element.sliderType;
			sliderDisplayType = _element.sliderDisplayType;
			blockSize = _element.blockSize;
			useFullWidth = _element.useFullWidth;
			varID = _element.varID;
			numberOfSteps = _element.numberOfSteps;
			base.Copy(_element);
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display(_style, _slot, zoom, isActive);
			GUI.Label(ZoomRect(relativeRect, zoom), string.Empty, _style);
			if ((bool)sliderTexture)
			{
				DrawSlider(zoom);
			}
			_style.alignment = anchor;
			if (zoom < 1f)
			{
				_style.fontSize = (int)((float)_style.fontSize * zoom);
			}
			_style.normal.background = null;
			if (textEffects != TextEffects.None)
			{
				AdvGame.DrawTextEffect(ZoomRect(relativeRect, zoom), TranslateLabel(label), _style, Color.black, _style.normal.textColor, 2f, textEffects);
			}
			else
			{
				GUI.Label(ZoomRect(relativeRect, zoom), TranslateLabel(label), _style);
			}
		}

		private void DrawSlider(float zoom)
		{
			sliderRect = relativeRect;
			if (sliderDisplayType == SliderDisplayType.FillBar)
			{
				if (useFullWidth)
				{
					sliderRect.x = relativeRect.x;
					sliderRect.width = slotSize.x * amount;
				}
				else
				{
					sliderRect.x = relativeRect.x + relativeRect.width / 2f;
					sliderRect.width = slotSize.x * amount * 0.5f;
				}
				if (sizeType != AC_SizeType.AbsolutePixels)
				{
					sliderRect.width *= AdvGame.GetMainGameViewSize().x / 100f;
				}
			}
			else if (sliderDisplayType == SliderDisplayType.MoveableBlock)
			{
				sliderRect.width *= blockSize.x;
				sliderRect.height *= blockSize.y;
				sliderRect.y += (relativeRect.height - sliderRect.height) / 2f;
				if (useFullWidth)
				{
					sliderRect.x = slotSize.x * amount + relativeRect.x - sliderRect.width / 2f;
				}
				else
				{
					sliderRect.x = slotSize.x * amount * 0.5f + relativeRect.x + relativeRect.width / 2f - sliderRect.width / 2f;
				}
				if (sizeType != AC_SizeType.AbsolutePixels)
				{
					sliderRect.x *= AdvGame.GetMainGameViewSize().x / 100f;
				}
			}
			GUI.DrawTexture(ZoomRect(sliderRect, zoom), sliderTexture, ScaleMode.StretchToFill, true, 0f);
		}

		public override string GetLabel(int slot)
		{
			return TranslateLabel(label);
		}

		public void Change()
		{
			amount += 0.02f;
			if (amount > 1f)
			{
				amount = 0f;
			}
			UpdateValue();
		}

		public void Change(float mouseX)
		{
			if (useFullWidth)
			{
				mouseX -= relativeRect.x;
				amount = mouseX / relativeRect.width;
			}
			else
			{
				mouseX = mouseX - relativeRect.x - relativeRect.width / 2f;
				amount = mouseX / (relativeRect.width / 2f);
			}
			UpdateValue();
		}

		private void UpdateValue()
		{
			if (amount < 0f)
			{
				amount = 0f;
			}
			else if (amount > 1f)
			{
				amount = 1f;
			}
			if (numberOfSteps > 0)
			{
				float num = 1f / (float)numberOfSteps;
				float num2;
				for (num2 = 0f; num2 < amount; num2 += num)
				{
				}
				float num3 = num2 - num;
				if (amount - num3 > num2 - amount)
				{
					amount = num2;
				}
				else
				{
					amount = num3;
				}
			}
			if (sliderType == AC_SliderType.Speech || sliderType == AC_SliderType.SFX || sliderType == AC_SliderType.Music)
			{
				if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>())
				{
					Options component = GameObject.FindWithTag("PersistentEngine").GetComponent<Options>();
					if (sliderType == AC_SliderType.Speech)
					{
						component.optionsData.speechVolume = amount;
					}
					else if (sliderType == AC_SliderType.Music)
					{
						component.optionsData.musicVolume = amount;
						component.SetVolume(SoundType.Music);
					}
					else if (sliderType == AC_SliderType.SFX)
					{
						component.optionsData.sfxVolume = amount;
						component.SetVolume(SoundType.SFX);
					}
					component.SavePrefs();
				}
				else
				{
					Debug.LogWarning("Could not find Options data!");
				}
			}
			else if (sliderType == AC_SliderType.FloatVariable && varID >= 0)
			{
				GVar variable = RuntimeVariables.GetVariable(varID);
				if (variable.type == VariableType.Float)
				{
					variable.floatVal = amount;
					variable.Upload();
				}
			}
		}

		public override void RecalculateSize()
		{
			if (Application.isPlaying)
			{
				if (sliderType == AC_SliderType.Speech || sliderType == AC_SliderType.SFX || sliderType == AC_SliderType.Music)
				{
					if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>())
					{
						Options component = GameObject.FindWithTag("PersistentEngine").GetComponent<Options>();
						if (component.optionsData != null)
						{
							if (sliderType == AC_SliderType.Speech)
							{
								amount = component.optionsData.speechVolume;
							}
							else if (sliderType == AC_SliderType.Music)
							{
								amount = component.optionsData.musicVolume;
							}
							else if (sliderType == AC_SliderType.SFX)
							{
								amount = component.optionsData.sfxVolume;
							}
						}
					}
				}
				else if (sliderType == AC_SliderType.FloatVariable && varID >= 0)
				{
					GVar variable = RuntimeVariables.GetVariable(varID);
					if (variable != null)
					{
						if (variable.type != VariableType.Float)
						{
							Debug.LogWarning("Cannot link MenuSlider " + title + " to Variable " + varID + " as it is not a Float.");
						}
						else
						{
							amount = Mathf.Clamp(RuntimeVariables.GetFloatValue(varID), 0f, 1f);
						}
					}
					else
					{
						Debug.LogWarning("Slider " + label + " is referencing Gloval Variable " + varID + ", which does not exist.");
					}
				}
			}
			base.RecalculateSize();
		}

		protected override void AutoSize()
		{
			AutoSize(new GUIContent(TranslateLabel(label)));
		}
	}
}
