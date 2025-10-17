using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBattleShopSlot : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textCost;
    public Transform cost;

    public GameObject objDrag;

    GameObject currentDrag;


    public void Init(FormationItemConfig itemConfig)
    {
        textName.gameObject.SetActive(true);
        textName.text = UtilityLocalization.GetLocalization(itemConfig.displayName);
        cost.gameObject.SetActive(true);
        textCost.text = itemConfig.coinCost.ToString();

        if (currentDrag != null)
        {
            Destroy(currentDrag);
        }
        currentDrag = Instantiate(objDrag, transform);

        FormationItem itemData = new FormationItem();
        itemData.Init(itemConfig);


        currentDrag.GetComponent<FormationitemDragHandlerUI>().SetitemData(itemData, OnEndDrag);
    }

    private void OnEndDrag()
    {
        //成功放置时，隐藏UI
        textName.gameObject.SetActive(false);
        cost.gameObject.SetActive(false);
    }
}
