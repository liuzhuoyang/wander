using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 遗物星级组件
/// </summary>
public class RelicStar : MonoBehaviour
{
    [SerializeField] Image[] imgStar;
    public void Init(int star)
    {
        string path;
        int showCount;
        if (star <= 5)
        {
            path = "relic_star_1";
            showCount = star;
        }
        else
        {
            path = "relic_star_2";
            showCount = star - 5;
        }
        for (int i = 0; i < showCount; i++)
        {
            GameAssetControl.AssignIcon(path, imgStar[i]);
            imgStar[i].gameObject.SetActive(true);
        }
        for (int i = showCount; i < imgStar.Length; i++)
        {
            imgStar[i].gameObject.SetActive(false);
        }
    }
}
