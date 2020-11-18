using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 이벤트 등록을 위한 using UnityEngine.EventSystems;
/// </summary>
public class StaticInventoryUI : InventoryUI
{
    #region Variables
    public GameObject[] staticSlots = null; // 고정된 슬롯 오브젝트
    #endregion Variables

    #region Main Methods
    /// <summary>
    /// 슬롯 UI 오브젝트를 생성하는 함수
    /// </summary>
    public override void CreateSlotUIs()
    {
        // 초기화
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            // 배치된 UI 오브젝트를 그대로 사용
            GameObject slotGo = staticSlots[i];

            // 슬롯에 대한 마우스, 드래그 이벤트 등록
            AddEvent(slotGo, EventTriggerType.PointerEnter, delegate { OnEnterSlot(slotGo); });
            AddEvent(slotGo, EventTriggerType.PointerExit, delegate { OnExitSlot(slotGo); });
            AddEvent(slotGo, EventTriggerType.BeginDrag, delegate { OnStartDrag(slotGo); });
            AddEvent(slotGo, EventTriggerType.Drag, delegate { OnDrag(slotGo); });
            AddEvent(slotGo, EventTriggerType.EndDrag, delegate { OnEndDrag(slotGo); });

            // 실제 오브젝트와 관리 객체 연결
            inventoryObject.Slots[i].SlotUI = slotGo;
            slotUIs.Add(slotGo, inventoryObject.Slots[i]);

            // 이름 지정
            slotGo.name += ": " + i;
        }
    }
    #endregion Main Methods
}
