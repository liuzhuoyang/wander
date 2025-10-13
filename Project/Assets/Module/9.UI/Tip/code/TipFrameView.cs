using UnityEngine;
using TMPro;

public class TipFrameView : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Init(float posX, float posY, string textKey)
    {
        transform.localPosition = new Vector2(posX, posY);
        text.text = UtilityLocalization.GetLocalization(textKey);
    }
}
