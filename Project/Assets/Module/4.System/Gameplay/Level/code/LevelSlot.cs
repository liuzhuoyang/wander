using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlot : MonoBehaviour
{
    public Transform container;
    public async void Init(string themeName, int themeVarient)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        GameObject obj = await GameAsset.GetPrefabAsync($"level_slot_{themeName}_{themeVarient}");
        Instantiate(obj, container);

    }
}