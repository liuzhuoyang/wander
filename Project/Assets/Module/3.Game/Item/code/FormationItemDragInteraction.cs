using UnityEngine;
using PlayerInteraction;
using System;

public class FormationItemDragInteraction : Draggable
{
    public FormationNode formationNode;

    void Start()
    {
        dragRoot = transform;
        formationNode = GetComponentInParent<FormationNode>();
    }
    override public void OnInteract(PlayerInputControl playerInput)
    {
        base.OnInteract(playerInput);
        //判断状态
        if (BattleSystem.Instance.GetCurrentBattleState() != BattleStates.PrepareRun)
        {
            return;
        }
        
        if (formationNode.HasItem())
        {
            FormationWorldItem formationItem = formationNode.WorldItem;
            FormationItem formationItemData = formationItem.item;

            formationItem.Hide();
            // 将世界坐标转换为屏幕坐标
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(playerInput.m_pointerWorldPos);

            EventManager.TriggerEvent<FormationDragArgs>(FormationDragEvent.EVENT_FORMATION_DRAG_CREATE_WORLD_DRAG, new FormationDragArgs()
            {
                formationItemData = formationItemData,
                originalNode = formationNode,
                mousePosition = screenPosition,
                onDragComplete = OnDragComplete
            });
        }
    }

    void OnDragComplete(bool success)
    {
        if (success)
        {
            formationNode.RemoveItem();
        }
        else
        {
            formationNode.WorldItem.Show();
        }
        dragRoot.transform.position = formationNode.transform.position;
    }

}
