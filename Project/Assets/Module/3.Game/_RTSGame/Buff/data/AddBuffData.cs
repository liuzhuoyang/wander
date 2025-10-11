using UnityEngine;

using BattleBuff;

[CreateAssetMenu(fileName = "AddBuffData", menuName = "Assets/Buff/AddBuffData")]
public class AddBuffData : BuffData
{
    public BuffData buff;
    protected override Buff GetBuffInstance()
    {
        return new AddBuffBuff(m_buffID, buff.m_buffID);
    }
}
