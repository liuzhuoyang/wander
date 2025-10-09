using TMPro;
using UnityEngine;

public class LevelWaveSlot : MonoBehaviour
{
    [SerializeField] GameObject objBoss, objElite;
    [SerializeField] TextMeshProUGUI textWave;

    public void Init(int wave, bool isBoss)
    {
        objBoss.SetActive(isBoss);
        objElite.SetActive(!isBoss);
        textWave.text = UtilityLocalization.GetLocalization("page/level/page_level_the_wave_x", wave.ToString());
    }

}
