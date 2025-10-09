using UnityEngine;
using TMPro;

public class DebuggerFeatureUnlockerSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] GameObject objTick;

    public bool isSelected = false;
    
    [HideInInspector]
    public FeatureData featureData;

    public void Init(FeatureData featureData)
    {
        this.featureData = featureData;
        textName.text = featureData.featureType.ToString();

        objTick.SetActive(false);
    }

    public void OnClick()
    {
        isSelected = !isSelected;
        objTick.SetActive(isSelected);        
    }
}
