using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITip : MonoBehaviour
{
    public GameObject prefabTipText;
    public GameObject prefabTipFrame;
    public GameObject prefabTipCustom;
    public Transform containerTipText;
    public Transform containerTipFrame;
    public Transform containerTipCustom;

    int maxTip = 1;
    List<GameObject> listTip;

    void Start()
    {
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
            case TipType.Generic:
                if (listTip.Count >= maxTip)
                {
                    go = listTip[0];
                }
                else
                {
                    go = Instantiate(prefabTipText, containerTipText);
                    listTip.Add(go);
                }

                go.GetComponent<TipGenericView>().Init(args.textTip);
                go.transform.localPosition = Vector2.zero;
                break;
            case TipType.Frame:
                foreach (Transform child in containerTipFrame)
                {
                    Destroy(child.gameObject);
                }
                UITipFrameArgs frameArgs = args as UITipFrameArgs;
                go = Instantiate(prefabTipText, containerTipFrame);
                go.GetComponent<TipFrameView>().Init(frameArgs.posX, frameArgs.posY, frameArgs.textTip);
                break;
            case TipType.Custom:
                foreach (Transform child in containerTipCustom)
                {
                    Destroy(child.gameObject);
                }
                UITipCustomArgs customArgs = args as UITipCustomArgs;
                go = Instantiate(customArgs.customTipPrefab, containerTipCustom);
                go.GetComponent<TipCustomView>().Init(customArgs);
                break;
        }
    }
}
