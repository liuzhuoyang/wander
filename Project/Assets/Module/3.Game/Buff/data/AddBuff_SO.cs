using UnityEngine;

using BattleBuff;

[CreateAssetMenu(fileName = "AddBuff_SO", menuName = "Assets/Buff/AddBuff_SO")]
public class AddBuff_SO : BuffData
{
    public BuffData buff;
    protected override Buff GetBuffInstance()
    {
        return new AddBuffBuff(m_buffID, buff.m_buffID);
    }
}
