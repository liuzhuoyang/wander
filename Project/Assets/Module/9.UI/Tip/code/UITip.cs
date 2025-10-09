using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITip : MonoBehaviour
{
    public GameObject prefabTipText;
    public GameObject prefabTipHubReward;
    public Transform containerTipText;
    public Transform containerTipHub;

    int maxTip = 1;
    List<GameObject> listTip;

    void Start()
    {
        containerTipHub.gameObject.SetActive(false);
        listTip = new List<GameObject>();
        EventManager.StartListening<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, OnTip);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, OnTip);
    }

    void OnTip(UITipArgs args)
    {
        GameObject go;
        switch (args.tipType)
        {
            case TipType.Text:
                if (listTip.Count >= maxTip)
                {
                    go = listTip[0];
                }
                else
                {
                    go = Instantiate(prefabTipText, containerTipText);
                    listTip.Add(go);
                }

                go.gameObject.SetActive(true);
                go.GetComponent<TipView>().Init(args.textTip);
                go.transform.localPosition = Vector2.zero;
                break;
        }
    }

    public void OnCloseTipHub()
    {
        foreach (Transform child in containerTipHub)
        {
            Destroy(child.gameObject); //TODO 对象池
        }

        containerTipHub.gameObject.SetActive(false);
    }
}
