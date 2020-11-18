using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    #region Variables
    public InventorySlot[] slots = new InventorySlot[0];   // 인벤토리 슬롯 배열
    #endregion Variables

    #region Main Methods
    /// <summary>
    /// 슬롯 전체를 비워주는 함수
    /// </summary>
    public void Clear()
    {
        foreach (InventorySlot slot in slots)
        {
            //slot.UpdateSlot(new Item(), 0);
            slot.RemoveItem();
        }
    }

    /// <summary>
    /// 슬롯 내 아이템과 동일한지 검사하는 함수
    /// </summary>
    /// <param name="itemObject">아이템 오브젝트</param>
    /// <returns>동일 여부</returns>
    public bool IsContain(ItemObject itemObject)
    {
        // System.Array를 이용
        return System.Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
    }

    /// <summary>
    /// 슬롯 내 아이템과 동일한지 검사하는 함수
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns>동일 여부</returns>
    public bool IsContain(int id)
    {
        // System.Linq를 이용
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
    #endregion Main Methods
}
