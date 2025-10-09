using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DebuggerControl : MonoBehaviour
{
    public GameObject objMain;

    public Image imgDebugTrigger;
    float counter;
    int debuggerTriggerCount;

    public TextMeshProUGUI textDebugMode;
    public Button btnDebugModeTurnOn;
    public Button btnDebugModeTurnOff;
   
    //全局Debug控制器
    public void Init()
    {
        objMain.SetActive(false);
        imgDebugTrigger.color = new Color(1, 1, 1, 0);

        Refresh();
    }

    public void OnTriggerDebugger()
    {
        imgDebugTrigger.color = new Color(1, 1, 1, 1);
        imgDebugTrigger.DOKill();
        imgDebugTrigger.DOFade(0, 0.1f);

        debuggerTriggerCount++;
        if(debuggerTriggerCount >= 3)
        {
            objMain.SetActive(true);
        }
    }

    void Refresh()
    {
        textDebugMode.text = "当前Debug Mode:" + GameConfig.debugToolRunTime.ToString();
        btnDebugModeTurnOn.interactable = GameConfig.debugToolRunTime == DebugTool.Off;
        btnDebugModeTurnOff.interactable = GameConfig.debugToolRunTime == DebugTool.On;
        //刷新所有debug菜单
        EventManager.TriggerEvent<UIBaseArgs>(DebuggerEventName.EVENT_DEBUGGER_MENU_REFRESH, null);
    }

    void Update()
    {
        //每3秒重置计数器
        counter += Time.deltaTime;
        if(counter >= 3)
        {
            counter = 0;
            debuggerTriggerCount = 0;
        }
    }

    public void OnClose()
    {
        objMain.SetActive(false);
    }

    public void OnDebugModeTurnOn()
    {
        GameConfig.debugToolRunTime = DebugTool.On;
        Refresh();
        Debugger.Instance.OnShowDebugger();
    }

    public void OnDebugModeTurnOff()
    {
        GameConfig.debugToolRunTime = DebugTool.Off;
        Refresh();
        Debugger.Instance.OnHideDebugger();
    }

    public void OnDebugBattleMergeOn()
    {
        string path = "mode/ui_mode_battle/active/ui_mode_battle_merge/top/debug";
        if(UIMain.Instance.transform.Find(path) == null)
        {
            textDebugMode.text = "当前页面不是战斗合成页面";
            return;
        }

        GameObject obj = UIMain.Instance.transform.Find(path).gameObject;
        obj.SetActive(true);
        OnClose();
        //UIMain.Instance.transform.Find()
    }

    public void OnDebugBattleFightOn()
    {
        string path = "mode/ui_mode_battle/active/ui_mode_battle_fight/top/debug";
        if(UIMain.Instance.transform.Find(path) == null)
        {
            textDebugMode.text = "当前页面不是战斗页面";
            return;
        }
        GameObject obj = UIMain.Instance.transform.Find(path).gameObject;
        obj.SetActive(true);
        OnClose();
    }
}
