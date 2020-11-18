using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    #region Variables
    public int id = -1;                 // 아이템 ID 정보
    public string name = null;          // 아이템 이름

    public ItemBuff[] buffs = null;     // 적용될 버프 배열
    #endregion Variables

    #region Main Methods
    /// <summary>
    /// 기본 생성자
    /// </summary>
    public Item()
    {
        // 아이템 데이터 초기화
        id = -1;
        name = null;
    }

    /// <summary>
    /// 아이템 오브젝트의 정보를 이용하여 초기화하는 생성자
    /// </summary>
    /// <param name="itemObject"></param>
    public Item(ItemObject itemObject)
    {
        id = itemObject.data.id;
        name = itemObject.data.name;

        // 아이템 오브젝트의 정보를 이용하여 버프 배열 초기화
        buffs = new ItemBuff[itemObject.data.buffs.Length];
        // 아이템 오브젝트의 정보를 이용하여 버프 초기화
        for (int i = 0; i < buffs.Length; i++)
        {
            // 버프를 초기화 하는 두가지 방법

            // 첫번째
            //buffs[i] = new ItemBuff(itemObject.data.buffs[i].Min, itemObject.data.buffs[i].Max);
            //buffs[i].stat = itemObject.data.buffs[i].stat;

            // 두번째
            buffs[i] = new ItemBuff(itemObject.data.buffs[i].Min, itemObject.data.buffs[i].Max)
            {
                stat = itemObject.data.buffs[i].stat
            };
        }
    }
    #endregion Main Methods
}
