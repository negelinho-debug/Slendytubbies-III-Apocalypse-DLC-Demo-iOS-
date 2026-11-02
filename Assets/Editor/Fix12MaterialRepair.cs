using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Fix12MaterialRepair : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return -100; } }

    static string Normalize(string s)
    {
        s = s.ToLowerInvariant().Replace(" ", "").Replace("-", "");
        foreach (string suffix in new [] {"_material", "_mat", "_albedo", "_diffuse", "_basecolor", "_d"})
            if (s.EndsWith(suffix, StringComparison.Ordinal))
                return s.Substring(0, s.Length - suffix.Length);
        return s;
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        var textures = new Dictionary<string, List<Texture2D>>();
        foreach (string guid in AssetDatabase.FindAssets("t:Texture2D", new [] { "Assets" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D t = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (t == null) continue;
            string key = Normalize(Path.GetFileNameWithoutExtension(path));
            if (!textures.ContainsKey(key)) textures[key] = new List<Texture2D>();
            textures[key].Add(t);
        }

        int repaired = 0, unresolved = 0;
        foreach (string guid in AssetDatabase.FindAssets("t:Material", new [] { "Assets" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.EndsWith(".mat", StringComparison.OrdinalIgnoreCase)) continue;
            Material m = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (m == null) continue;

            if (m.shader == null || m.shader.name == "Hidden/InternalErrorShader")
            {
                Debug.LogWarning("[FIX12] Missing shader: " + path);
                unresolved++;
                continue;
            }

            string property = m.HasProperty("_MainTex") ? "_MainTex" :
                              (m.HasProperty("_BaseMap") ? "_BaseMap" : null);
            if (property == null || m.GetTexture(property) != null) continue;

            List<Texture2D> candidates;
            if (textures.TryGetValue(Normalize(m.name), out candidates) && candidates.Count == 1)
            {
                m.SetTexture(property, candidates[0]);
                EditorUtility.SetDirty(m);
                repaired++;
                Debug.Log("[FIX12] Restored texture " + candidates[0].name + " on " + path);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("[FIX12] Texture bindings restored=" + repaired + "; missing shaders=" + unresolved);
    }
}
