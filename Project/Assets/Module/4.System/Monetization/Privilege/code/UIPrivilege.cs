using UnityEngine;

/// <summary>
/// 赛季卡UI
/// </summary>
public class UISeasonCard : UIBase
{
    [SerializeField] PrivilegeSlot privilegeSlot;
    [SerializeField] PrivilegePerpetualSlot privilegeBattle;

    void Awake()
    {
        EventManager.StartListening<UIPrivilegeArgs>(EventNamePrivilege.EVENT_PRIVILEGE_INIT_UI, OnInitUI);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIPrivilegeArgs>(EventNamePrivilege.EVENT_PRIVILEGE_INIT_UI, OnInitUI);
    }

    void OnInitUI(UIPrivilegeArgs args)
    {
        privilegeSlot.OnInit(AllPrivilege.dictData["privilege_normal"]);
        privilegeBattle.OnInit(AllPrivilege.dictData["privilege_battle"]);
    }
}