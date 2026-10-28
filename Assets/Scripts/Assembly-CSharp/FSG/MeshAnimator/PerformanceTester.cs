using System.Collections.Generic;
using UnityEngine;

namespace FSG.MeshAnimator
{
	public class PerformanceTester : MonoBehaviour
	{
		public GameObject[] animObjects;

		public string[] options;

		public float cameraSpeed = 20f;

		public Vector3 spawnOffset = new Vector3(-10f, 0f, 5f);

		private int[] spawnedMeshes = new int[3];

		private List<GameObject> meshes = new List<GameObject>();

		private string fps;

		private int previousFrame;

		private Vector3 offset = new Vector3(-10f, 0f, 0f);

		private bool crossFade;

		private void Start()
		{
			InvokeRepeating("UpdateFPS", 0.0001f, 1f);
		}

		private void UpdateFPS()
		{
			fps = ((float)(Time.frameCount - previousFrame) / 1f).ToString("00.00");
			previousFrame = Time.frameCount;
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.W))
			{
				transform.position += Vector3.forward * Time.deltaTime * cameraSpeed;
			}
			if (Input.GetKey(KeyCode.A))
			{
				transform.position -= Vector3.right * Time.deltaTime * cameraSpeed;
			}
			if (Input.GetKey(KeyCode.S))
			{
				transform.position -= Vector3.forward * Time.deltaTime * cameraSpeed;
			}
			if (Input.GetKey(KeyCode.D))
			{
				transform.position += Vector3.right * Time.deltaTime * cameraSpeed;
			}
		}

		private void OnDisable()
		{
			for (int i = 0; i < options.Length; i++)
			{
				if ((bool)animObjects[i].GetComponent<MecanimMeshAnimatorController>())
				{
					animObjects[i].GetComponent<MecanimMeshAnimatorController>().crossFade = false;
				}
				if ((bool)animObjects[i].GetComponent<AnimatorStateMachine>())
				{
					animObjects[i].GetComponent<AnimatorStateMachine>().crossFade = false;
				}
			}
		}

		private void OnGUI()
		{
			GUI.skin.label.richText = true;
			GUILayout.BeginArea(new Rect((float)Screen.height * 0.1f, (float)Screen.width * 0.1f, (float)Screen.width * 0.3f, Screen.height));
			GUILayout.Label("<size=20><b>Test Scenes</b></size>");
			switch (Application.loadedLevel)
			{
			case 0:
				GUILayout.Label("With large amount's of SkinnedMeshRenderer's in the scene. Unity's built-in performance doesn't come close to Mesh Animator's");
				break;
			case 1:
				GUILayout.Label("SkinnedMeshRenderer = Worst Performance");
				GUILayout.Label("Mesh Animator + Mecanim Controller = Better Performance");
				GUILayout.Label("Mesh Animator + Event Callbacks = Best Performance");
				break;
			case 2:
				GUILayout.Label("Animated objects can be combined into a single object and draw call, greatly improving preformance.");
				break;
			}
			GUI.color = ((Application.loadedLevel != 0) ? Color.white : Color.green);
			if (GUILayout.Button("Stadium Test"))
			{
				Application.LoadLevel(0);
			}
			GUI.color = ((Application.loadedLevel != 1) ? Color.white : Color.green);
			if (GUILayout.Button("Skinned Mesh Test"))
			{
				Application.LoadLevel(1);
			}
			GUI.color = ((Application.loadedLevel != 2) ? Color.white : Color.green);
			if (GUILayout.Button("Mesh Filter Test"))
			{
				Application.LoadLevel(2);
			}
			GUI.color = Color.white;
			GUILayout.Label("<size=20><b>FPS: " + fps + "</b></size>");
			GUILayout.Label("WASD to move the camera");
			bool flag = crossFade;
			crossFade = GUILayout.Toggle(crossFade, "Crossfade", GUILayout.Height((float)Screen.height * 0.05f));
			if (flag != crossFade)
			{
				for (int i = 0; i < options.Length; i++)
				{
					if ((bool)animObjects[i].GetComponent<MecanimMeshAnimatorController>())
					{
						animObjects[i].GetComponent<MecanimMeshAnimatorController>().crossFade = crossFade;
					}
					if ((bool)animObjects[i].GetComponent<AnimatorStateMachine>())
					{
						animObjects[i].GetComponent<AnimatorStateMachine>().crossFade = crossFade;
					}
				}
				for (int j = 0; j < meshes.Count; j++)
				{
					if ((bool)meshes[j].GetComponent<MecanimMeshAnimatorController>())
					{
						meshes[j].GetComponent<MecanimMeshAnimatorController>().crossFade = crossFade;
					}
					if ((bool)meshes[j].GetComponent<AnimatorStateMachine>())
					{
						meshes[j].GetComponent<AnimatorStateMachine>().crossFade = crossFade;
					}
				}
			}
			for (int k = 0; k < options.Length; k++)
			{
				if (GUILayout.RepeatButton(options[k] + " Spawned: " + spawnedMeshes[k], GUILayout.Height((float)Screen.height * 0.05f)))
				{
					meshes.Add(Object.Instantiate(animObjects[k], offset, Quaternion.Euler(0f, 180f, 0f)));
					spawnedMeshes[k]++;
					offset.x += (0f - spawnOffset.x) / 10f;
					if (meshes.Count % 20 == 0)
					{
						offset.x = spawnOffset.x;
						offset.z += spawnOffset.z;
					}
				}
			}
			if (GUILayout.Button("Clear", GUILayout.Height((float)Screen.height * 0.05f)))
			{
				foreach (GameObject mesh in meshes)
				{
					Object.Destroy(mesh);
				}
				meshes.Clear();
				spawnedMeshes = new int[3];
				offset = new Vector3(-10f, 0f, 0f);
			}
			GUILayout.EndArea();
		}
	}
}
