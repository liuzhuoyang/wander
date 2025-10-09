using UnityEngine;
using UnityEditor;

// 添加菜单项
public class GameConfigMenu
{
    [MenuItem("Onicore/Game Config")]
    public static void OpenGameConfig()
    {
        // 查找GameConfig的实例
        GameConfigAsset config = AssetDatabase.LoadAssetAtPath<GameConfigAsset>("Assets/Module/0.Base/Config/GameConfig.asset");

        if (config != null)
        {
            // 在Inspector中选中并打开
            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }
        else
        {
            Debug.LogError("GameConfig.asset not found. Please create a GameConfig asset first.");
        }
    }
}
