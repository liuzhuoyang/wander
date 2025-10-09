using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebuggerItemGroup : MonoBehaviour
{
    public GameObject prefabItem;

    public Transform container;

    public void Init()
    {
        gameObject.SetActive(true);

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in AllItem.dictData)
        {
            var go = Instantiate(prefabItem, container);
            go.name = item.Value.itemName;
            go.transform.Find("icon").GetComponent<Image>().sprite = GameAsset.GetAssetEditor<Sprite>("icon_" + item.Value.itemName);
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClose();
                Debugger.Instance.OnCloseDebug();
                int num = 500;
                if (item.Value.itemName == ConstantItem.COIN || item.Value.itemName == ConstantItem.GEM)
                {
                    num = 999999;
                }
                RewardSystem.Instance.OnReward(new List<RewardArgs> { new RewardArgs { reward = item.Value.itemName, num = num } });
            });
        }
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
