using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyGroupLeft : MonoBehaviour
{
    public GameObject prefabBtnPack;
    public GameObject prefabBtnPromo;

    public Transform containerPack;
    public Transform containerPromo;

    public void Init()
    {
        /*
        EventManager.StartListening<UILobbyPackArgs>(EventName.EVENT_LOBBY_ADD_PACK_UI, OnAddPack);
        EventManager.StartListening<UILobbyPromoArgs>(EventName.EVENT_LOBBY_ADD_PROMO_UI, OnAddPromo);
        */
    }

    private void OnDestroy()
    {
        /*
        EventManager.StopListening<UILobbyPackArgs>(EventName.EVENT_LOBBY_ADD_PACK_UI, OnAddPack);
        EventManager.StopListening<UILobbyPromoArgs>(EventName.EVENT_LOBBY_ADD_PROMO_UI, OnAddPromo);
        */
    }

    /*
    void OnAddPack(UILobbyPackArgs args)
    {
        foreach(Transform child in containerPack)
        {
            Destroy(child.gameObject);
        }

        foreach(string packName in args.dictPack.Keys)
        {
            UserPackArgs userPackArgs = null;
            args.dictPack.TryGetValue(packName, out userPackArgs);
            if (userPackArgs == null) continue;

            GameObject go = Instantiate(prefabBtnPack, containerPack);
            go.GetComponent<LobbyBtnPack>().Init(userPackArgs.packName, userPackArgs.packType, userPackArgs.remainSec);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(containerPack.GetComponent<RectTransform>());
     //   LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }

    /*
    void OnAddPromo(UILobbyPromoArgs args)
    {
        foreach (Transform child in containerPromo)
        {
            Destroy(child.gameObject);
        }

        foreach (string packName in args.dictPromo.Keys)
        {
            UserPromoArgs userPackArgs = null;
            args.dictPromo.TryGetValue(packName, out userPackArgs);
            if (userPackArgs == null) continue;
            if (args.buyDict.ContainsKey(packName)) continue;
          //  if (userPackArgs.buyNum > 0) continue;
            GameObject go = Instantiate(prefabBtnPromo, containerPromo);

            go.GetComponent<LobbyBtnPromo>().Init(userPackArgs.promoName);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(containerPromo.GetComponent<RectTransform>());
      //  LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }*/
}
