using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 이벤트 등록을 위한 using UnityEngine.EventSystems;
/// </summary>
public class DynamicInventoryUI : InventoryUI
{
    #region Variables
    [SerializeField]
    protected GameObject slotPrefabs = null;    // 슬롯 UI 오브젝트

    [SerializeField]
    protected Vector2 start = Vector2.zero;     // 격자형식의 시작 지점
    [SerializeField]
    protected Vector2 size = Vector2.zero;      // 인벤토리 사이즈

    [SerializeField]
    protected Vector2 space = Vector2.zero;     // 슬롯 사이의 간격

    [Min(1), SerializeField]
    protected int numberOfColum = 4;            // 한 행에 들어갈 슬롯 지정 (최소: 1)
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
            // 슬롯 오브젝트 생성
            GameObject slotGo = Instantiate(slotPrefabs, Vector3.zero, Quaternion.identity, transform);
            // 앵커에 맞는 계산된 위치로 설정
            slotGo.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

            // 슬롯에 대한 마우스, 드래그 이벤트 등록
            AddEvent(slotGo, EventTriggerType.PointerEnter, delegate { OnEnterSlot(slotGo); });
            AddEvent(slotGo, EventTriggerType.PointerExit, delegate { OnExitSlot(slotGo); });
            AddEvent(slotGo, EventTriggerType.BeginDrag, delegate { OnStartDrag(slotGo); });
            AddEvent(slotGo, EventTriggerType.Drag, delegate { OnDrag(slotGo); });
            AddEvent(slotGo, EventTriggerType.EndDrag, delegate { OnEndDrag(slotGo); });
            // 클릭에 대한 이벤트 함수 연결
            AddEvent(slotGo, EventTriggerType.PointerClick, (data) => { OnClick(slotGo, (PointerEventData)data); });

            // 실제 오브젝트와 관리 객체 연결
            inventoryObject.Slots[i].SlotUI = slotGo;
            slotUIs.Add(slotGo, inventoryObject.Slots[i]);

            // 이름 지정
            slotGo.name += ": " + i;
        }
    }

    /// <summary>
    /// 마우스 오른쪽 버튼이 눌렸을 때의 로직 처리 함수
    /// </summary>
    /// <param name="slot">슬롯</param>
    protected override void OnRightClick(InventorySlot slot)
    {
        // 아이템 사용
        inventoryObject.UseItem(slot);
    }
    #endregion Main Methods

    #region Helper Methods
    /// <summary>
    /// 위치를 계산하는 함수
    /// </summary>
    /// <param name="i">슬롯 인덱스</param>
    /// <returns>위치 벡터</returns>
    public Vector3 CalculatePosition(int i)
    {
        // 슬롯 위치 계산
        float x = start.x + ((space.x + size.x) * (i % numberOfColum));
        float y = start.y + (-(space.y + size.y)) * (i / numberOfColum);

        return new Vector3(x, y, 0.0f);
    }
    #endregion Helper Methods
}
