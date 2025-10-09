using UnityEngine;
using BattleActor.Unit;
using Sirenix.OdinInspector;
using BattleBuff;

public class Buff_Demo_Controller : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private UnitModifiableAttributeType unitModifiableAttributeType;
    [SerializeField] private AttributeModifyType attributeModifyType;

    [SerializeField] private bool applyBuffToEnemy;
    
    [Button("应用加成")]
    public void ApplyModify()
    {
        var modifier = new UnitAttributeModifier(value, attributeModifyType, unitModifiableAttributeType);
        if (applyBuffToEnemy)
        {
            foreach (var unit in UnitManager.Instance.m_enemyUnitList)
            {
                unit.m_buffHandler.TryAddBuffRaw(new UnitAttributeTriggerBuff("unit_buff", modifier));
            }
        }
        else
        {
            foreach (var unit in UnitManager.Instance.m_playerUnitList)
            {
                unit.m_buffHandler.TryAddBuffRaw(new UnitAttributeTriggerBuff("unit_buff", modifier));
            }
        }
    }
}
