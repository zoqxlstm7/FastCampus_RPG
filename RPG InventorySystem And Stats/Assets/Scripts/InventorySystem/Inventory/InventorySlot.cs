using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    #region Variables
    public ItemType[] allowedItems = new ItemType[0];   // 슬롯에 놓을 수 있는 아이템 타입 배열

    //[System.NonSerialized]
    //public InventoryObject parent = null;
    //[System.NonSerialized]
    //public GameObject slotUI = null;

    public Item item = new Item();                      // 아이템
    public int amount = 0;                              // 수량
    #endregion Variables

    #region Property
    public InventoryObject Parent { get; set; } = null; // 인벤토리 오브젝트
    public GameObject SlotUI { get; set; } = null;      // 슬롯 UI 오브젝트

    // 현재 슬롯안의 아이템의 정보를 반환
    public ItemObject ItemObject
    {
        get
        {
            return item.id >= 0 ? Parent.database.itemObjects[item.id] : null;
        }
    }
    #endregion Property

    #region Action Events
    // 슬롯이 업데이트될 때의 부가적인 처리를 위한 이벤트
    [System.NonSerialized]
    public System.Action<InventorySlot> OnPreUpdate = null;
    [System.NonSerialized]
    public System.Action<InventorySlot> OnPostUpdate = null;
    #endregion Action Events

    #region Generator
    /// <summary>
    /// 비어있는 슬롯을 생성하는 기본 생성자
    /// </summary>
    public InventorySlot() => UpdateSlot(new Item(), 0);
    /// <summary>
    /// 슬롯에 아이템을 설정하는 생성자
    /// </summary>
    /// <param name="item">아이템</param>
    /// <param name="amount">수량</param>
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);
    #endregion Generator

    #region Main Methods
    /// <summary>
    /// 슬롯에 아이템을 추가하는 함수
    /// </summary>
    /// <param name="item">아이템</param>
    /// <param name="amount">수량</param>
    public void AddItem(Item item, int amount) => UpdateSlot(item, amount);
    /// <summary>
    /// 슬롯의 아이템을 삭제하는 함수
    /// </summary>
    public void RemoveItem() => UpdateSlot(new Item(), 0);
    /// <summary>
    /// 아이템 수량을 증가시키는 함수
    /// </summary>
    /// <param name="value">증가량</param>
    public void AddAmount(int value) => UpdateSlot(item, amount += value);

    /// <summary>
    /// 슬롯을 업데이트하는 함수
    /// </summary>
    /// <param name="item">아이템</param>
    /// <param name="amount">수량</param>
    public void UpdateSlot(Item item, int amount)
    {
        // 수량이 없다면 아이템 삭제 처리
        if (amount <= 0)
            item = new Item();

        // 기존 아이템에 대한 부가적인 처리
        OnPreUpdate?.Invoke(this);

        // 아이템 및 수량 설정
        this.item = item;
        this.amount = amount;

        // 새로 설정된 아이템에 대한 부가적인 처리
        OnPostUpdate?.Invoke(this);
    }

    /// <summary>
    /// 슬롯에 위치할 수 있는지 검사하는 함수
    /// </summary>
    /// <param name="itemObject">아이템 오브젝트</param>
    /// <returns></returns>
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        // 아이템 타입의 제한이 없거나,
        // 아이템 오브젝트가 null이거나
        // 비어있는 아이템이라면 슬롯에 위치할 수 있음
        if (allowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            return true;

        // 제한된 아이템 타입과 타입이 같은 경우 슬롯에 위치할 수 있음
        foreach (ItemType type in allowedItems)
        {
            if (itemObject.type == type)
                return true;
        }

        // 슬롯에 위치할 수 없음을 반환
        return false;
    }
    #endregion Main Methods
}
