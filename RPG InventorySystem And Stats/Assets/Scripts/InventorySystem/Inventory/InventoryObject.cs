using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    #region Variables
    public ItemObjectDatabase database = null;  // 아이템 오브젝트 데이터베이스
    public InterfaceType type;                  // 인터페이스 타입

    [SerializeField]
    Inventory container = new Inventory();      // 인벤토리
    #endregion Variables

    #region Property
    // 인벤토리 슬롯 반환
    public InventorySlot[] Slots => container.slots;

    // 비어있는 인벤토리 슬롯 갯수를 반환
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            foreach (InventorySlot slot in Slots)
            {
                // 슬롯에 아이템이 비어있다면
                if (slot.item.id <= -1)
                    counter++;
            }

            return counter;
        }
    }
    #endregion Property

    #region Action Event
    // 아이템이 사용됐을 때 처리하는 이벤트
    // 플레이어 클래스의 OnUseItem() 함수에 연결하여 소모아이템에 대한 동작 수행
    public System.Action<ItemObject> OnUseItem = null;
    #endregion Action Event

    #region Main Methods
    /// <summary>
    /// 인벤토리에 아이템을 추가하는 함수
    /// </summary>
    /// <param name="item">아이템</param>
    /// <param name="amount">수량</param>
    /// <returns>추가 여부</returns>
    public bool AddItem(Item item, int amount)
    {
        // 빈 슬롯이 없다면 추가 실패
        if (EmptySlotCount <= 0)
            return false;

        // 인벤토리 내에 같은 아이템을 가진 슬롯이 있는지 검사
        InventorySlot slot = FindItemInInventory(item);
        // 중첩할 수 없거나 같은 아이템을 가진 슬롯이 없다면
        if(!database.itemObjects[item.id].stackable || slot == null)
        {
            // 빈 슬롯을 찾아 슬롯에 아이템 추가
            slot = GetEmptySlot();
            slot.UpdateSlot(item, amount);
        }
        else
        {
            // 슬롯 아이템의 수량 증가
            slot.AddAmount(amount);
        }

        // 추가 성공 여부 반환
        return true;
    }

    /// <summary>
    /// 인벤토리 내에 같은 아이템을 가진 슬롯을 검사하는 함수
    /// </summary>
    /// <param name="item">아이템</param>
    /// <returns>슬롯</returns>
    public InventorySlot FindItemInInventory(Item item)
    {
        // 같은 ID를 가진 아이템이 있다면 해당 슬롯 반환
        return Slots.FirstOrDefault(i => i.item.id == item.id);
    }

    /// <summary>
    /// 인벤토리에 같은 아이템이 있는지 검사하는 함수
    /// </summary>
    /// <param name="itemObject">아이템 오브젝트</param>
    /// <returns>존재 여부</returns>
    public bool IsContainItem(ItemObject itemObject)
    {
        return container.IsContain(itemObject);
    }

    /// <summary>
    /// 빈 슬롯을 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public InventorySlot GetEmptySlot()
    {
        // 빈슬롯의 ID값은 -1
        return Slots.FirstOrDefault(i => i.item.id < 0);
    }

    /// <summary>
    /// 슬롯끼리 아이템을 교환하는 함수
    /// </summary>
    /// <param name="itemA">아이템 슬롯 A</param>
    /// <param name="itemB">아이템 슬롯 B</param>
    public void SwapItems(InventorySlot itemA, InventorySlot itemB)
    {
        // 두 슬롯이 같다면 리턴
        if(itemA == itemB)
            return;

        // 아이템을 서로의 슬롯에 놓을 수 있다면 교환
        if(itemB.CanPlaceInSlot(itemA.ItemObject) && itemA.CanPlaceInSlot(itemB.ItemObject))
        {
            InventorySlot tempSlot = new InventorySlot(itemB.item, itemB.amount);
            itemB.UpdateSlot(itemA.item, itemA.amount);
            itemA.UpdateSlot(tempSlot.item, tempSlot.amount);
        }
    }

    /// <summary>
    /// 아이템을 사용하는 함수
    /// </summary>
    /// <param name="slotToUse">사용할 슬롯</param>
    public void UseItem(InventorySlot slotToUse)
    {
        // 슬롯에 사용할 아이템이 없다면 리턴
        if (slotToUse.ItemObject == null || slotToUse.item.id < 0 || slotToUse.amount <= 0)
            return;

        // 아이템 사용 처리, 수량 감소
        slotToUse.UpdateSlot(slotToUse.item, slotToUse.amount - 1);

        // 사용한 아이템 정보를 매개변수로 이벤트 함수 호출
        ItemObject itemObject = slotToUse.ItemObject;
        OnUseItem?.Invoke(itemObject);
    }
    #endregion Main Methods

    [ContextMenu("Clear")]
    public void Clear()
    {
        container.Clear();
    }
}
