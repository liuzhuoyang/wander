using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 遗物槽位
/// </summary>
public class RelicSlot : MonoBehaviour
{
    [SerializeField] Image icon, imgBg;
    [SerializeField] SlicedFilledImage fillBar;
    [SerializeField] TextMeshProUGUI textPro, textName;
    [SerializeField] GameObject objNew, objLock, objUnlock, objUnlockable;
    [SerializeField] RelicStar relicStar;
    [SerializeField] GameObject objPin;

    RelicSlotViewArgs slotArgs;
    public void Init(RelicSlotViewArgs args)
    {
        slotArgs = args;
        //图标
        GameAssetControl.AssignIcon(args.relicData.relicName, icon);
        //背景
        GameAssetControl.AssignSpriteUI("relic_slot_" + args.relicData.rarity.ToString().ToLower(), imgBg);
        //判断是否解锁
        if (args.star == -1)
        {
            //未解锁
            objLock.SetActive(true);
            objUnlock.SetActive(false);
            //进度
            textPro.text = $"{args.count}/{args.needCount}";
            fillBar.fillAmount = (float)args.count / args.needCount;
            //判断是否可解锁
            if (args.count >= args.needCount)
            {
                objUnlockable.SetActive(true);
            }
            else
            {
                objUnlockable.SetActive(false);
            }
            return;
        }
        //已解锁
        objLock.SetActive(false);
        objUnlock.SetActive(true);
        //名字
        textName.text = UtilityLocalization.GetLocalization(args.relicData.displayName);
        //判断是否新获得
        objNew.SetActive(args.isNew);
        //星级
        relicStar.Init(args.star);
        //判断是否可升星
        if(args.star == EventNameRelic.RELIC_STAR_MAX)
        {
            objPin.SetActive(false);
            return;
        }
        objPin.SetActive(args.count >= args.needCount);
    }
    public void OnClickSlot()
    {
        RelicSystem.Instance.OnClickSlot(slotArgs, () =>
        {
            //刷新状态
            objNew.SetActive(false);
        });
    }
}
