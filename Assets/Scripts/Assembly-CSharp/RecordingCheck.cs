using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class RecordingCheck : MonoBehaviour
{
	public string[] processNames;

	public bool hasFound;

	public Text mytext;

	private void Start()
	{
		Process[] processes = Process.GetProcesses();
		for (int i = 0; i < processNames.Length; i++)
		{
			Process[] array = processes;
			foreach (Process process in array)
			{
				if (process.ProcessName.ToLower().Contains(processNames[i]) && !hasFound)
				{
					MonoBehaviour.print("found " + processNames[i]);
					hasFound = true;
					mytext.gameObject.SetActive(true);
				}
			}
		}
	}
}
