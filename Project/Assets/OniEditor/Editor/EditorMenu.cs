using UnityEngine;
using UnityEditor;

public class OniToolBar : MonoBehaviour
{
    [MenuItem("Onicore/更新CSV")]
    static void OnUpdateCSV()
    {
        CSVWriter.OnUpdateCSV();
    }
}
