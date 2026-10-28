using UnityEngine;

namespace MeshTK
{
	public class ExportTools
	{
		public static string MeshToObjString(Mesh mesh, Renderer renderer, bool uselhcoords = true, bool separateSubmeshes = true)
		{
			Debug.LogWarning("WARNING: 'MeshTK.ExportTools.MeshToObjString()' is obsolete. Use MeshTK.ObjTools.ExportToString().");
			return ObjTools.ExportToString(mesh, renderer, uselhcoords, separateSubmeshes);
		}

		public static void MeshToFile(Mesh mesh, Renderer renderer, string filename, bool uselhcoords = true)
		{
			Debug.LogWarning("WARNING: 'MeshTK.ExportTools.MeshToFile()' is obsolete. Use MeshTK.ObjTools.ExportToFile().");
			ObjTools.ExportToFile(mesh, renderer, filename, uselhcoords);
		}
	}
}
