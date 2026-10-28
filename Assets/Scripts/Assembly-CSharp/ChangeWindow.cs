using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChangeWindow : MonoBehaviour
{
	[DllImport("user32.dll")]
	public static extern bool SetWindowText(IntPtr hwnd, string lpString);

	[DllImport("user32.dll")]
	public static extern IntPtr FindWindow(string className, string windowName);

	public void ChangeTitle(string newTitle)
	{
		IntPtr hwnd = FindWindow(null, "Slendytubbies 3");
		SetWindowText(hwnd, newTitle);
	}

	private void Start()
	{
		ChangeTitle("..--- ----- ..--- -----");
	}
}
