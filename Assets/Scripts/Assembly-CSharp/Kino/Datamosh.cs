using System.Collections;
using UnityEngine;

namespace Kino
{
	[RequireComponent(typeof(Camera))]
	public class Datamosh : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Size of compression macroblock.")]
		private int _blockSize = 16;

		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("Entropy coefficient. The larger value makes the stronger noise.")]
		private float _entropy = 0.5f;

		[SerializeField]
		[Range(0.5f, 4f)]
		[Tooltip("Contrast of stripe-shaped noise.")]
		private float _noiseContrast = 1f;

		[SerializeField]
		[Range(0f, 2f)]
		[Tooltip("Scale factor for velocity vectors.")]
		private float _velocityScale = 0.8f;

		[SerializeField]
		[Range(0f, 2f)]
		[Tooltip("Amount of random displacement.")]
		private float _diffusion = 0.4f;

		[SerializeField]
		private Shader _shader;

		private Material _material;

		private RenderTexture _workBuffer;

		private RenderTexture _dispBuffer;

		private int _sequence;

		private int _lastFrame;

		public int blockSize
		{
			get
			{
				return Mathf.Max(4, _blockSize);
			}
			set
			{
				_blockSize = value;
			}
		}

		public float entropy
		{
			get
			{
				return _entropy;
			}
			set
			{
				_entropy = value;
			}
		}

		public float noiseContrast
		{
			get
			{
				return _noiseContrast;
			}
			set
			{
				_noiseContrast = value;
			}
		}

		public float velocityScale
		{
			get
			{
				return _velocityScale;
			}
			set
			{
				_velocityScale = value;
			}
		}

		public float diffusion
		{
			get
			{
				return _diffusion;
			}
			set
			{
				_diffusion = value;
			}
		}

		public void Glitch()
		{
			_sequence = 1;
		}

		public void Reset()
		{
			_sequence = 0;
		}

		private RenderTexture NewWorkBuffer(RenderTexture source)
		{
			return RenderTexture.GetTemporary(source.width, source.height);
		}

		private RenderTexture NewDispBuffer(RenderTexture source)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / _blockSize, source.height / _blockSize, 0, RenderTextureFormat.ARGBHalf);
			temporary.filterMode = FilterMode.Point;
			return temporary;
		}

		private void ReleaseBuffer(RenderTexture buffer)
		{
			if (buffer != null)
			{
				RenderTexture.ReleaseTemporary(buffer);
			}
		}

		private void OnEnable()
		{
			_material = new Material(Shader.Find("Hidden/Kino/Datamosh"));
			_material.hideFlags = HideFlags.DontSave;
			GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
			_sequence = 0;
			StartCoroutine(GlitchNow());
		}

		private IEnumerator GlitchNow()
		{
			yield return new WaitForEndOfFrame();
			Glitch();
		}

		private void OnDisable()
		{
			ReleaseBuffer(_workBuffer);
			_workBuffer = null;
			ReleaseBuffer(_dispBuffer);
			_dispBuffer = null;
			Object.DestroyImmediate(_material);
			_material = null;
			Reset();
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			_material.SetFloat("_BlockSize", _blockSize);
			_material.SetFloat("_Quality", 1f - _entropy);
			_material.SetFloat("_Contrast", _noiseContrast);
			_material.SetFloat("_Velocity", _velocityScale);
			_material.SetFloat("_Diffusion", _diffusion);
			if (_sequence == 0)
			{
				ReleaseBuffer(_workBuffer);
				_workBuffer = NewWorkBuffer(source);
				Graphics.Blit(source, _workBuffer);
				Graphics.Blit(source, destination);
				return;
			}
			if (_sequence == 1)
			{
				ReleaseBuffer(_dispBuffer);
				_dispBuffer = NewDispBuffer(source);
				Graphics.Blit(null, _dispBuffer, _material, 0);
				Graphics.Blit(_workBuffer, destination);
				_sequence++;
				return;
			}
			if (Time.frameCount != _lastFrame)
			{
				RenderTexture renderTexture = NewDispBuffer(source);
				Graphics.Blit(_dispBuffer, renderTexture, _material, 1);
				ReleaseBuffer(_dispBuffer);
				_dispBuffer = renderTexture;
				RenderTexture renderTexture2 = NewWorkBuffer(source);
				_material.SetTexture("_WorkTex", _workBuffer);
				_material.SetTexture("_DispTex", _dispBuffer);
				Graphics.Blit(source, renderTexture2, _material, 2);
				ReleaseBuffer(_workBuffer);
				_workBuffer = renderTexture2;
				_lastFrame = Time.frameCount;
			}
			Graphics.Blit(_workBuffer, destination);
		}
	}
}
