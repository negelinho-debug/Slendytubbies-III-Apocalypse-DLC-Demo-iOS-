using System.Collections.Generic;
using UnityEngine;

namespace FSG.MeshAnimator
{
	public class CrowdSpawner : MonoBehaviour
	{
		public GameObject[] options;

		public string[] optionsDesc;

		public int sizeOfCrowd = 1000;

		public int selectedOption;

		public int maxSize = 5000;

		private string fps;

		private int previousFrame;

		private List<GameObject> spawnedObjects = new List<GameObject>();

		private void Start()
		{
			SpawnCrowd();
			InvokeRepeating("UpdateFPS", 0.0001f, 1f);
		}

		private void UpdateFPS()
		{
			fps = ((float)(Time.frameCount - previousFrame) / 1f).ToString("00.00");
			previousFrame = Time.frameCount;
		}

		private void SpawnCrowd()
		{
			foreach (GameObject spawnedObject in spawnedObjects)
			{
				if ((bool)spawnedObject)
				{
					Object.Destroy(spawnedObject);
				}
			}
			spawnedObjects.Clear();
			List<int> list = new List<int>();
			for (int i = 0; i < sizeOfCrowd; i++)
			{
				if (i > transform.childCount)
				{
					list.Clear();
				}
				int num = Random.Range(0, transform.childCount);
				while (list.Contains(num))
				{
					num = Random.Range(0, transform.childCount);
				}
				GameObject gameObject = Object.Instantiate(options[selectedOption]);
				gameObject.transform.position = transform.GetChild(num).position + new Vector3(Random.value, 0f, Random.value) * 2f;
				gameObject.transform.LookAt(transform, Vector3.up);
				if ((bool)gameObject.GetComponent<Animator>())
				{
					gameObject.GetComponent<Animator>().speed = Random.Range(0.9f, 1.1f);
					gameObject.GetComponent<Animator>().SetInteger("Anim", Random.Range(0, 6));
				}
				else if ((bool)gameObject.GetComponent<MeshAnimator>())
				{
					MeshAnimator component = gameObject.GetComponent<MeshAnimator>();
					component.defaultAnimation = component.animations[Random.Range(0, component.animations.Length)];
					component.speed = Random.Range(0.9f, 1.1f);
				}
				spawnedObjects.Add(gameObject);
			}
		}

		private void OnGUI()
		{
			GUI.skin.label.richText = true;
			GUILayout.BeginArea(new Rect((float)Screen.height * 0.1f, (float)Screen.width * 0.1f, (float)Screen.width * 0.3f, Screen.height));
			if (Application.loadedLevelName != "PromoScene")
			{
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
				GUI.color = ((selectedOption != 0) ? Color.white : Color.green);
				if (GUILayout.Button("Skinned Mesh Crowd"))
				{
					selectedOption = 0;
					SpawnCrowd();
				}
				GUI.color = ((selectedOption != 1) ? Color.white : Color.green);
				if (GUILayout.Button("Mesh Animator Crowd"))
				{
					selectedOption = 1;
					SpawnCrowd();
				}
				GUI.color = Color.white;
				int num = sizeOfCrowd;
				GUILayout.Label("Crowd Size: " + sizeOfCrowd);
				sizeOfCrowd = (int)GUILayout.HorizontalSlider(sizeOfCrowd, 1f, maxSize);
				if (num != sizeOfCrowd)
				{
					CancelInvoke("SpawnCrowd");
					Invoke("SpawnCrowd", 1f);
				}
			}
			else
			{
				GUILayout.Label("<color=black><size=19><b>FPS: " + fps + "</b></size></color>");
			}
			GUILayout.EndArea();
		}
	}
}
