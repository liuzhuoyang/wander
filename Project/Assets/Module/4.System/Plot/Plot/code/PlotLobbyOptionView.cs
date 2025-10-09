using UnityEngine;
using TMPro;
public class PlotLobbyOptionView : MonoBehaviour
{
    public TextMeshProUGUI textContent;
    public void Reset()
    {
        gameObject.SetActive(false);
    }

    public void Init(string content)
    {
        gameObject.SetActive(true);
        textContent.text = content;
    }
}
