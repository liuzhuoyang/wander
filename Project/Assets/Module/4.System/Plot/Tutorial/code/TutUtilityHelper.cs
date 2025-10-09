using UnityEngine;
using UnityEngine.UI;

public class TutUtilityHelper : Singleton<TutUtilityHelper>
{
    public void Init()
    {

    }

    GameObject objHelper;

    public void OnCancelGridHelper()
    {
        Destroy(objHelper);
    }

    public void OnForceTutMaskOn()
    {
        UITut uITut = FindAnyObjectByType<UITut>();
        uITut.mask.SetActive(true);
    }
}
