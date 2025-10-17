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

        if (formationNode.HasItem())
        {
            FormationItem formationItem = formationNode.Item;
            FormationItemConfig itemConfig = formationItem.itemConfig;

            formationItem.Hide();
            // 将世界坐标转换为屏幕坐标
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(playerInput.m_pointerWorldPos);

            EventManager.TriggerEvent<FormationDragArgs>(FormationDragEvent.EVENT_FORMATION_DRAG_CREATE_WORLD_DRAG, new FormationDragArgs()
            {
                itemConfig = itemConfig,
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
            formationNode.Item.Show();
        }
        dragRoot.transform.position = formationNode.transform.position;
    }

}
