using System.IO;
using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR)
namespace Game.Architecture.Editor {
  // src: https://discussions.unity.com/t/single-instance-of-scriptable-object-without-adding-a-menu-entry/786987/8
  public static class SOCreatorWithHotkey {
    // puts new SO in rename mode. name it, don't press Enter, press hotkey again to save it and create a new one
    static bool isInRenameMode = true;

    // % = ctrl, # = shift, & = alt, _ = (no modifier key).
    [MenuItem("Assets/Create/SO _F10", false, priority = 65)]
    private static void CreateScriptableObject() {
      Object obj = Selection.activeObject;

      if (obj != null && obj.GetType() == typeof(MonoScript)) {
        var script = obj as MonoScript;

        if (script != null && script.GetClass().IsSubclassOf(typeof(ScriptableObject))) {
          var asset = ScriptableObject.CreateInstance(script.GetClass());
          string path = AssetDatabase.GetAssetPath(script);
          string directory = Path.GetDirectoryName(path);

          string targetPath = $"{directory}/{ObjectNames.NicifyVariableName(script.name)}.asset";
          Debug.Log(targetPath);

          if (isInRenameMode) {
            ProjectWindowUtil.CreateAsset(asset, targetPath);
            Selection.activeObject = obj;
          } else {
            AssetDatabase.CreateAsset(asset, targetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = asset;
          }
        }
      }
    }
  }
}
#endif
