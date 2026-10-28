using System.Collections;
using UnityEngine;

namespace AC
{
	public class MainCamera : MonoBehaviour
	{
		public Texture2D fadeTexture;

		public _Camera attachedCamera;

		public _Camera lastNavCamera;

		public _Camera lastNavCamera2;

		public bool isSmoothChanging;

		private bool isCrossfading;

		private Texture2D crossfadeTexture;

		private bool cursorAffectsRotation;

		public Vector2 perspectiveOffset = new Vector2(0f, 0f);

		private Vector2 startPerspectiveOffset = new Vector2(0f, 0f);

		private float timeToFade;

		private int drawDepth = -1000;

		private float alpha;

		private FadeType fadeType;

		private float fadeStartTime;

		private bool hideSceneWhileLoading;

		private MoveMethod moveMethod;

		private float changeTime;

		private AnimationCurve timeCurve;

		private Vector3 startPosition;

		private Quaternion startRotation;

		private float startFOV;

		private float startOrtho;

		private float startTime;

		private float startFocalDistance;

		public Transform lookAtTransform;

		private Vector2 lookAtAmount;

		private float LookAtZ;

		private Vector3 lookAtTarget;

		private SettingsManager settingsManager;

		private StateHandler stateHandler;

		private PlayerInput playerInput;

		private Texture2D actualFadeTexture;

		private float shakeStartTime;

		private float shakeDuration;

		private float shakeStartIntensity;

		private bool shakeMove;

		private float shakeIntensity;

		private Vector3 shakePosition;

		private Vector3 shakeRotation;

		private Camera borderCam;

		public static MainCamera mainCam;

		public float borderWidth;

		public MenuOrientation borderOrientation;

		public bool isSplitScreen;

		public bool isTopLeftSplit;

		public MenuOrientation splitOrientation;

		public _Camera splitCamera;

		public float splitAmountMain = 0.49f;

		public float splitAmountOther = 0.49f;

		private float focalDistance = 10f;

		private Camera _camera;

		private void Awake()
		{
			AssignFadeTexture();
			hideSceneWhileLoading = true;
			if ((bool)GetComponent<Camera>())
			{
				_camera = GetComponent<Camera>();
			}
			if ((bool)transform.parent && transform.parent.name != "_Cameras")
			{
				if ((bool)GameObject.Find("_Cameras"))
				{
					transform.parent = GameObject.Find("_Cameras").transform;
				}
				else
				{
					transform.parent = null;
				}
			}
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
			{
				playerInput = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			}
			if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if (!GetComponent<Camera>())
			{
				Debug.LogError("The MainCamera script requires a Camera component.");
			}
			else if (settingsManager.forceAspectRatio)
			{
				settingsManager.landscapeModeOnly = false;
				if (SetAspectRatio())
				{
					CreateBorderCamera();
				}
				SetCameraRect();
			}
		}

		private void Start()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
			{
				stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			}
			if ((bool)lookAtTransform)
			{
				lookAtTransform.localPosition = new Vector3(0f, 0f, 10f);
				LookAtZ = lookAtTransform.localPosition.z;
				LookAtCentre();
			}
			StartCoroutine("ShowScene");
		}

		private IEnumerator ShowScene()
		{
			yield return new WaitForSeconds(0.1f);
			hideSceneWhileLoading = false;
		}

		public void HideScene()
		{
			hideSceneWhileLoading = true;
			StartCoroutine("ShowScene");
		}

		public void Shake(float _shakeIntensity, float _duration, bool _shakeMove)
		{
			shakePosition = Vector3.zero;
			shakeRotation = Vector3.zero;
			shakeMove = _shakeMove;
			shakeDuration = _duration;
			shakeStartTime = Time.time;
			shakeIntensity = _shakeIntensity;
			shakeStartIntensity = shakeIntensity;
		}

		public bool IsShaking()
		{
			if (shakeIntensity > 0f)
			{
				return true;
			}
			return false;
		}

		public void StopShaking()
		{
			shakeIntensity = 0f;
			shakePosition = Vector3.zero;
			shakeRotation = Vector3.zero;
		}

		public void PrepareForBackground()
		{
			if (_camera == null && (bool)GetComponent<Camera>())
			{
				_camera = GetComponent<Camera>();
			}
			if (settingsManager == null && (bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			_camera.clearFlags = CameraClearFlags.Depth;
			if (LayerMask.NameToLayer(settingsManager.backgroundImageLayer) != -1)
			{
				_camera.cullingMask = ~(1 << LayerMask.NameToLayer(settingsManager.backgroundImageLayer));
			}
		}

		private void RemoveBackground()
		{
			if (_camera == null && (bool)GetComponent<Camera>())
			{
				_camera = GetComponent<Camera>();
			}
			if (settingsManager == null && (bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			_camera.clearFlags = CameraClearFlags.Skybox;
			if (LayerMask.NameToLayer(settingsManager.backgroundImageLayer) != -1)
			{
				_camera.cullingMask = ~(1 << LayerMask.NameToLayer(settingsManager.backgroundImageLayer));
			}
		}

		public void SetFirstPerson()
		{
			if ((bool)settingsManager && settingsManager.IsInFirstPerson())
			{
				GameObject gameObject = GameObject.FindWithTag("FirstPersonCamera");
				if (gameObject != null)
				{
					if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
					{
						if (gameObject.GetComponent<_Camera>() == null)
						{
							gameObject.AddComponent(typeof(_Camera));
						}
						if ((bool)gameObject.GetComponent<AudioListener>())
						{
							gameObject.GetComponent<AudioListener>().enabled = false;
						}
					}
					if ((bool)gameObject.GetComponent<_Camera>())
					{
						gameObject.GetComponent<_Camera>()._camera = gameObject.GetComponent<Camera>();
						SetGameCamera(gameObject.GetComponent<_Camera>());
						SnapToAttached();
					}
				}
			}
			if ((bool)attachedCamera)
			{
				if (lastNavCamera != attachedCamera)
				{
					lastNavCamera2 = lastNavCamera;
				}
				lastNavCamera = attachedCamera;
			}
		}

		public void DrawCameraFade()
		{
			if (timeToFade > 0f)
			{
				alpha = (Time.time - fadeStartTime) / timeToFade;
				if (fadeType == FadeType.fadeIn)
				{
					alpha = 1f - alpha;
				}
				alpha = Mathf.Clamp01(alpha);
				if (Time.time > fadeStartTime + timeToFade)
				{
					if (fadeType == FadeType.fadeIn)
					{
						alpha = 0f;
					}
					else
					{
						alpha = 1f;
					}
					timeToFade = 0f;
					StopCrossfade();
				}
			}
			if (hideSceneWhileLoading && actualFadeTexture != null)
			{
				GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), actualFadeTexture);
			}
			else if (alpha > 0f)
			{
				Color color = GUI.color;
				color.a = alpha;
				GUI.color = color;
				GUI.depth = drawDepth;
				if (isCrossfading)
				{
					GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), crossfadeTexture);
				}
				else
				{
					GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), actualFadeTexture);
				}
			}
			else if (actualFadeTexture != fadeTexture)
			{
				ReleaseFadeTexture();
			}
		}

		public void ResetProjection()
		{
			if ((bool)_camera)
			{
				perspectiveOffset = Vector2.zero;
				_camera.projectionMatrix = AdvGame.SetVanishingPoint(_camera, perspectiveOffset);
				_camera.ResetProjectionMatrix();
			}
		}

		public void ResetMoving()
		{
			isSmoothChanging = false;
			startTime = 0f;
			changeTime = 0f;
		}

		public void _LateUpdate()
		{
			if ((bool)settingsManager && settingsManager.IsInLoadingScene())
			{
				return;
			}
			if (stateHandler != null && stateHandler.gameState == GameState.Normal)
			{
				SetFirstPerson();
			}
			if ((bool)attachedCamera && !(attachedCamera is GameCamera25D))
			{
				if (!isSmoothChanging)
				{
					transform.rotation = attachedCamera.transform.rotation;
					transform.position = attachedCamera.transform.position;
					focalDistance = attachedCamera.focalDistance;
					if (attachedCamera is GameCamera2D)
					{
						GameCamera2D gameCamera2D = (GameCamera2D)attachedCamera;
						perspectiveOffset = gameCamera2D.perspectiveOffset;
						if (!_camera.orthographic)
						{
							_camera.projectionMatrix = AdvGame.SetVanishingPoint(_camera, perspectiveOffset);
						}
						else
						{
							_camera.orthographicSize = attachedCamera._camera.orthographicSize;
						}
					}
					else
					{
						_camera.fieldOfView = attachedCamera._camera.fieldOfView;
						if (cursorAffectsRotation)
						{
							SetlookAtTransformation();
							transform.LookAt(lookAtTransform, attachedCamera.transform.up);
						}
					}
				}
				else if (Time.time < startTime + changeTime)
				{
					if (attachedCamera is GameCamera2D)
					{
						GameCamera2D gameCamera2D2 = (GameCamera2D)attachedCamera;
						perspectiveOffset.x = AdvGame.Lerp(startPerspectiveOffset.x, gameCamera2D2.perspectiveOffset.x, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
						perspectiveOffset.y = AdvGame.Lerp(startPerspectiveOffset.y, gameCamera2D2.perspectiveOffset.y, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
						_camera.ResetProjectionMatrix();
					}
					if (moveMethod == MoveMethod.Curved)
					{
						Vector3 position = Vector3.Slerp(startPosition, attachedCamera.transform.position, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
						position.y = Mathf.Lerp(startPosition.y, attachedCamera.transform.position.y, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
						transform.position = position;
						transform.rotation = Quaternion.Slerp(startRotation, attachedCamera.transform.rotation, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
					}
					else
					{
						transform.position = AdvGame.Lerp(startPosition, attachedCamera.transform.position, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
						transform.rotation = AdvGame.Lerp(startRotation, attachedCamera.transform.rotation, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
					}
					focalDistance = AdvGame.Lerp(startFocalDistance, attachedCamera.focalDistance, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
					_camera.fieldOfView = AdvGame.Lerp(startFOV, attachedCamera._camera.fieldOfView, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
					_camera.orthographicSize = AdvGame.Lerp(startOrtho, attachedCamera._camera.orthographicSize, AdvGame.Interpolate(startTime, changeTime, moveMethod, timeCurve));
					if (attachedCamera is GameCamera2D && !_camera.orthographic)
					{
						_camera.projectionMatrix = AdvGame.SetVanishingPoint(_camera, perspectiveOffset);
					}
				}
				else
				{
					LookAtCentre();
					isSmoothChanging = false;
				}
				if (cursorAffectsRotation)
				{
					lookAtTransform.localPosition = Vector3.Lerp(lookAtTransform.localPosition, lookAtTarget, Time.deltaTime * 3f);
				}
			}
			else if ((bool)attachedCamera && attachedCamera is GameCamera25D)
			{
				transform.position = attachedCamera.transform.position;
				transform.rotation = attachedCamera.transform.rotation;
			}
			if (shakeIntensity > 0f)
			{
				if (shakeMove)
				{
					shakePosition = Random.insideUnitSphere * shakeIntensity * 0.5f;
				}
				shakeRotation = new Vector3(Random.Range(0f - shakeIntensity, shakeIntensity) * 0.2f, Random.Range(0f - shakeIntensity, shakeIntensity) * 0.2f, Random.Range(0f - shakeIntensity, shakeIntensity) * 0.2f);
				shakeIntensity = Mathf.Lerp(shakeStartIntensity, 0f, AdvGame.Interpolate(shakeStartTime, shakeDuration, MoveMethod.Linear, null));
				transform.position += shakePosition;
				transform.localEulerAngles += shakeRotation;
			}
			else if (shakeIntensity < 0f)
			{
				StopShaking();
			}
		}

		private void LookAtCentre()
		{
			if ((bool)lookAtTransform)
			{
				lookAtTarget = new Vector3(0f, 0f, LookAtZ);
			}
		}

		private void SetlookAtTransformation()
		{
			if (stateHandler.gameState == GameState.Normal)
			{
				Vector2 mousePosition = playerInput.GetMousePosition();
				Vector2 vector = new Vector2(mousePosition.x / (float)(Screen.width / 2) - 1f, mousePosition.y / (float)(Screen.height / 2) - 1f);
				float magnitude = vector.magnitude;
				if (magnitude < 1.4f)
				{
					lookAtTarget = new Vector3(vector.x * lookAtAmount.x, vector.y * lookAtAmount.y, LookAtZ);
				}
			}
		}

		public void SnapToAttached()
		{
			if ((bool)attachedCamera && (bool)attachedCamera._camera)
			{
				LookAtCentre();
				isSmoothChanging = false;
				_camera.orthographic = attachedCamera._camera.orthographic;
				_camera.fieldOfView = attachedCamera._camera.fieldOfView;
				_camera.orthographicSize = attachedCamera._camera.orthographicSize;
				transform.position = attachedCamera.transform.position;
				transform.rotation = attachedCamera.transform.rotation;
				focalDistance = attachedCamera.focalDistance;
				if (attachedCamera is GameCamera2D)
				{
					GameCamera2D gameCamera2D = (GameCamera2D)attachedCamera;
					perspectiveOffset = gameCamera2D.perspectiveOffset;
				}
				else
				{
					perspectiveOffset = new Vector2(0f, 0f);
				}
			}
		}

		public void Crossfade(float _changeTime, _Camera _linkedCamera)
		{
			object[] value = new object[2] { _changeTime, _linkedCamera };
			StartCoroutine("StartCrossfade", value);
		}

		public void StopCrossfade()
		{
			StopCoroutine("StartCrossfade");
			if (isCrossfading)
			{
				isCrossfading = false;
				alpha = 0f;
			}
			crossfadeTexture = null;
			Object.DestroyObject(crossfadeTexture);
		}

		private IEnumerator StartCrossfade(object[] parms)
		{
			float _changeTime = (float)parms[0];
			_Camera _linkedCamera = (_Camera)parms[1];
			yield return new WaitForEndOfFrame();
			crossfadeTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			crossfadeTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);
			crossfadeTexture.Apply();
			isSmoothChanging = false;
			isCrossfading = true;
			SetGameCamera(_linkedCamera);
			FadeOut(0f);
			FadeIn(_changeTime);
			SnapToAttached();
		}

		public void _ExitSceneWithOverlay()
		{
			StartCoroutine("ExitSceneWithOverlay");
		}

		private IEnumerator ExitSceneWithOverlay()
		{
			yield return new WaitForEndOfFrame();
			Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			screenTex.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);
			screenTex.Apply();
			SetFadeTexture(screenTex);
			FadeOut(0f);
		}

		public void SmoothChange(float _changeTime, MoveMethod method, AnimationCurve _timeCurve)
		{
			LookAtCentre();
			moveMethod = method;
			isSmoothChanging = true;
			StopCrossfade();
			startTime = Time.time;
			changeTime = _changeTime;
			startPosition = transform.position;
			startRotation = transform.rotation;
			startFOV = _camera.fieldOfView;
			startOrtho = _camera.orthographicSize;
			startFocalDistance = focalDistance;
			startPerspectiveOffset = perspectiveOffset;
			if (method == MoveMethod.CustomCurve)
			{
				timeCurve = _timeCurve;
			}
			else
			{
				timeCurve = null;
			}
		}

		public void SetGameCamera(_Camera newCamera)
		{
			if (newCamera == null)
			{
				return;
			}
			if (attachedCamera != null && attachedCamera is GameCamera25D && !(newCamera is GameCamera25D))
			{
				RemoveBackground();
			}
			_camera.ResetProjectionMatrix();
			attachedCamera = newCamera;
			attachedCamera.SetCameraComponent();
			if ((bool)attachedCamera && (bool)attachedCamera._camera)
			{
				_camera.farClipPlane = attachedCamera._camera.farClipPlane;
				_camera.nearClipPlane = attachedCamera._camera.nearClipPlane;
				_camera.orthographic = attachedCamera._camera.orthographic;
			}
			if (attachedCamera is GameCamera)
			{
				GameCamera gameCamera = (GameCamera)attachedCamera;
				cursorAffectsRotation = gameCamera.followCursor;
				lookAtAmount = gameCamera.cursorInfluence;
			}
			else if (attachedCamera is GameCameraAnimated)
			{
				GameCameraAnimated gameCameraAnimated = (GameCameraAnimated)attachedCamera;
				if (gameCameraAnimated.animatedCameraType == AnimatedCameraType.SyncWithTargetMovement)
				{
					cursorAffectsRotation = gameCameraAnimated.followCursor;
					lookAtAmount = gameCameraAnimated.cursorInfluence;
				}
				else
				{
					cursorAffectsRotation = false;
				}
			}
			else
			{
				cursorAffectsRotation = false;
			}
			if (attachedCamera is GameCamera25D)
			{
				GameCamera25D gameCamera25D = (GameCamera25D)attachedCamera;
				gameCamera25D.SetActiveBackground();
			}
			if (attachedCamera is GameCamera2D)
			{
				_camera.transparencySortMode = TransparencySortMode.Orthographic;
			}
			else if ((bool)attachedCamera)
			{
				if (attachedCamera._camera.orthographic)
				{
					_camera.transparencySortMode = TransparencySortMode.Orthographic;
				}
				else
				{
					_camera.transparencySortMode = TransparencySortMode.Perspective;
				}
			}
		}

		private void SetFadeTexture(Texture2D tex)
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine"))
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().tempFadeTexture = tex;
			}
			AssignFadeTexture();
		}

		public void ReleaseFadeTexture()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine"))
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().tempFadeTexture = null;
			}
			AssignFadeTexture();
		}

		private void AssignFadeTexture()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().tempFadeTexture != null)
			{
				actualFadeTexture = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().tempFadeTexture;
			}
			else
			{
				actualFadeTexture = fadeTexture;
			}
		}

		public void FadeOut(float _timeToFade, Texture2D tempTex)
		{
			if (tempTex != null)
			{
				SetFadeTexture(tempTex);
			}
			FadeOut(_timeToFade);
		}

		public void FadeIn(float _timeToFade)
		{
			AssignFadeTexture();
			if (_timeToFade > 0f)
			{
				timeToFade = _timeToFade;
				alpha = 1f;
				fadeType = FadeType.fadeIn;
				fadeStartTime = Time.time;
			}
			else
			{
				alpha = 0f;
				timeToFade = 0f;
				ReleaseFadeTexture();
			}
		}

		public void FadeOut(float _timeToFade)
		{
			AssignFadeTexture();
			if (_timeToFade > 0f)
			{
				alpha = Mathf.Clamp01(alpha);
				timeToFade = _timeToFade;
				fadeType = FadeType.fadeOut;
				fadeStartTime = Time.time - alpha * timeToFade;
			}
			else
			{
				alpha = 1f;
				timeToFade = 0f;
			}
		}

		public bool isFading()
		{
			if (fadeType == FadeType.fadeOut && alpha < 1f)
			{
				return true;
			}
			if (fadeType == FadeType.fadeIn && alpha > 0f)
			{
				return true;
			}
			return false;
		}

		public void OnDeserializing()
		{
			FadeIn(0.5f);
		}

		public Vector3 PositionRelativeToCamera(Vector3 _position)
		{
			return _position.x * ForwardVector() + _position.z * RightVector();
		}

		public Vector3 RightVector()
		{
			return transform.right;
		}

		public Vector3 ForwardVector()
		{
			Vector3 forward = transform.forward;
			forward.y = 0f;
			return forward;
		}

		private bool SetAspectRatio()
		{
			float num = 0f;
			if (Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.LandscapeLeft)
			{
				num = (float)Screen.width / (float)Screen.height;
			}
			else
			{
				num = ((Screen.height <= Screen.width || !settingsManager.landscapeModeOnly) ? ((float)Screen.width / (float)Screen.height) : ((float)Screen.height / (float)Screen.width));
			}
			if (!settingsManager.forceAspectRatio || (float)(int)(num * 100f) / 100f == (float)(int)(settingsManager.wantedAspectRatio * 100f) / 100f)
			{
				borderWidth = 0f;
				borderOrientation = MenuOrientation.Horizontal;
				if ((bool)borderCam)
				{
					Object.Destroy(borderCam.gameObject);
				}
				return false;
			}
			if (num > settingsManager.wantedAspectRatio)
			{
				borderWidth = 1f - settingsManager.wantedAspectRatio / num;
				borderWidth /= 2f;
				borderOrientation = MenuOrientation.Vertical;
			}
			else
			{
				borderWidth = 1f - num / settingsManager.wantedAspectRatio;
				borderWidth /= 2f;
				borderOrientation = MenuOrientation.Horizontal;
			}
			return true;
		}

		public Rect GetSplitScreenRect(bool isMainCamera)
		{
			bool flag = isTopLeftSplit;
			float num = splitAmountMain;
			if (!isMainCamera)
			{
				flag = !isTopLeftSplit;
				num = splitAmountOther;
			}
			if (borderOrientation == MenuOrientation.Vertical)
			{
				if (splitOrientation == MenuOrientation.Horizontal)
				{
					if (!flag)
					{
						return new Rect(borderWidth, 0f, 1f - 2f * borderWidth, num);
					}
					return new Rect(borderWidth, 1f - num, 1f - 2f * borderWidth, num);
				}
				if (flag)
				{
					return new Rect(borderWidth, 0f, num - borderWidth, 1f);
				}
				return new Rect(1f - num, 0f, num - borderWidth, 1f);
			}
			if (splitOrientation == MenuOrientation.Horizontal)
			{
				if (flag)
				{
					return new Rect(0f, 1f - num, 1f, num - borderWidth);
				}
				return new Rect(0f, borderWidth, 1f, num - borderWidth);
			}
			if (flag)
			{
				return new Rect(0f, borderWidth, num, 1f - 2f * borderWidth);
			}
			return new Rect(1f - num, borderWidth, num, 1f - 2f * borderWidth);
		}

		public void SetCameraRect()
		{
			if (SetAspectRatio())
			{
				CreateBorderCamera();
			}
			if (isSplitScreen)
			{
				_camera.rect = GetSplitScreenRect(true);
			}
			else if (borderOrientation == MenuOrientation.Vertical)
			{
				_camera.rect = new Rect(borderWidth, 0f, 1f - 2f * borderWidth, 1f);
			}
			else if (borderOrientation == MenuOrientation.Horizontal)
			{
				_camera.rect = new Rect(0f, borderWidth, 1f, 1f - 2f * borderWidth);
			}
		}

		private void CreateBorderCamera()
		{
			if (!borderCam)
			{
				borderCam = new GameObject("BorderCamera", typeof(Camera)).GetComponent<Camera>();
				borderCam.transform.parent = transform;
				borderCam.depth = -2.1474836E+09f;
				borderCam.clearFlags = CameraClearFlags.Color;
				borderCam.backgroundColor = Color.black;
				borderCam.cullingMask = 0;
			}
		}

		public Vector2 LimitMouseToAspect(Vector2 mousePosition)
		{
			if (settingsManager == null || !settingsManager.forceAspectRatio)
			{
				return mousePosition;
			}
			if (borderOrientation == MenuOrientation.Horizontal)
			{
				int num = (int)((float)Screen.height * borderWidth);
				if (mousePosition.y < (float)num)
				{
					mousePosition.y = num;
				}
				else if (mousePosition.y > (float)(Screen.height - num))
				{
					mousePosition.y = Screen.height - num;
				}
			}
			else
			{
				int num2 = (int)((float)Screen.width * borderWidth);
				if (mousePosition.x < (float)num2)
				{
					mousePosition.x = num2;
				}
				else if (mousePosition.x > (float)(Screen.width - num2))
				{
					mousePosition.x = Screen.width - num2;
				}
			}
			return mousePosition;
		}

		public static Rect _LimitMenuToAspect(Rect rect)
		{
			if (mainCam == null)
			{
				mainCam = KickStarter.mainCamera;
			}
			if (mainCam != null)
			{
				return mainCam.LimitMenuToAspect(rect);
			}
			return rect;
		}

		public Rect LimitMenuToAspect(Rect rect)
		{
			if (settingsManager == null || !settingsManager.forceAspectRatio)
			{
				return rect;
			}
			if (borderOrientation == MenuOrientation.Horizontal)
			{
				int num = (int)((float)Screen.height * borderWidth);
				if (rect.y < (float)num)
				{
					rect.y = num;
					if (rect.height > (float)(Screen.height - num - num))
					{
						rect.height = Screen.height - num - num;
					}
				}
				else if (rect.y + rect.height > (float)(Screen.height - num))
				{
					rect.y = (float)(Screen.height - num) - rect.height;
				}
			}
			else
			{
				int num2 = (int)((float)Screen.width * borderWidth);
				if (rect.x < (float)num2)
				{
					rect.x = num2;
					if (rect.width > (float)(Screen.width - num2 - num2))
					{
						rect.width = Screen.width - num2 - num2;
					}
				}
				else if (rect.x + rect.width > (float)(Screen.width - num2))
				{
					rect.x = (float)(Screen.width - num2) - rect.width;
				}
			}
			return rect;
		}

		public void RemoveSplitScreen()
		{
			isSplitScreen = false;
			SetCameraRect();
			if ((bool)splitCamera)
			{
				splitCamera.RemoveSplitScreen();
				splitCamera = null;
			}
		}

		public void SetSplitScreen(_Camera _camera1, _Camera _camera2, MenuOrientation _splitOrientation, bool _isTopLeft, float splitAmountMain, float splitAmountOther)
		{
			splitCamera = _camera2;
			isSplitScreen = true;
			splitOrientation = _splitOrientation;
			isTopLeftSplit = _isTopLeft;
			SetGameCamera(_camera1);
			SnapToAttached();
			StartSplitScreen(splitAmountMain, splitAmountOther);
		}

		public void StartSplitScreen(float _splitAmountMain, float _splitAmountOther)
		{
			splitAmountMain = _splitAmountMain;
			splitAmountOther = _splitAmountOther;
			splitCamera.SetSplitScreen();
			SetCameraRect();
		}

		public float GetFocalDistance()
		{
			return focalDistance;
		}

		private void OnDestroy()
		{
			crossfadeTexture = null;
			settingsManager = null;
			stateHandler = null;
			playerInput = null;
		}
	}
}
