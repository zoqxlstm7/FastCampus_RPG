using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 추상화 클래스, EventSystem 네임스페이스
/// 전반적인 마우스 이벤트 등록과 UI 업데이트 처리
/// EventTrigger 컴포넌트가 있어야 마우스 이벤트 처리 가능
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    #region Variables
    public InventoryObject inventoryObject = null; // 인벤토리 오브젝트
    InventoryObject previousInventory = null;      // 이전 선택된 인벤토리 오브젝트

    // UI 슬롯 오브젝트
    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();
    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        // 슬롯 UI 오브젝트 생성
        CreateSlotUIs();

        // 슬롯 초기화
        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            // 인벤토리 오브젝트 부모 설정 및 이후 처리 이벤트 함수 연결
            inventoryObject.Slots[i].Parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
        }
        
        // 마우스 포인터가 인벤토리 UI안에 들어왔을 때 다음 함수 호출
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        // 마우스 포인터가 인벤토리 UI안에서 나갔을 때 다음 함수 호출
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    protected virtual void Start()
    {
        // 인벤토리 오브젝트의 모든 슬롯 갱신
        // OnPostUpdate() 함수에서 아이콘과 갯수에 대한 UI를 갱신해주도록 구성
        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 슬롯 UI 오브젝트를 생성하는 함수
    /// </summary>
    public abstract void CreateSlotUIs();

    /// <summary>
    /// 슬롯을 아이콘, 갯수등의 UI를 갱신해주는 함수
    /// </summary>
    /// <param name="slot">슬롯</param>
    public void OnPostUpdate(InventorySlot slot)
    {
        // 아이콘 및 색상 UI 초기화
        slot.SlotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject?.icon;
        slot.SlotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        // 수량 텍스트 UI 초기화
        slot.SlotUI.GetComponentInChildren<Text>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString("n0"));
    }

    /// <summary>
    /// 이벤트를 등록하는 함수
    /// 이벤트가 발생할 때 UnityAction의 BaseEventData를 받아오도록함
    /// UnityAction : using UnityEngine.Events; 네임 스페이스 추가
    /// </summary>
    /// <param name="go">주체 오브젝트</param>
    /// <param name="type">이벤트 타입</param>
    /// <param name="action">액션 이벤트</param>
    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // 이벤트 트리거 획득
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger)
        {
            Debug.LogWarning("No EventTrigger Component Found!");
            return;
        }

        // 이벤트 타입에 대한 액션을 이벤트 트리거에 등록
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        // 이벤트 트리거를 트리거에 추가
        trigger.triggers.Add(eventTrigger);
    }
    #endregion Main Methods

    #region Mouse Events
    /// <summary>
    /// 마우스가 인벤토리에 들어왔을 때
    /// </summary>
    /// <param name="go">인벤토리 UI 오브젝트</param>
    public void OnEnterInterface(GameObject go)
    {
        // 오버된 인벤토리 데이터 저장
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }

    /// <summary>
    /// 마우스가 인벤토리에서 나갔을 때
    /// </summary>
    /// <param name="go">인벤토리 UI 오브젝트</param>
    public void OnExitInterface(GameObject go)
    {
        // 이전 인벤토리 오브젝트 저장
        previousInventory = MouseData.interfaceMouseIsOver.inventoryObject;
        // 오버된 인벤토리 데이터 초기화
        MouseData.interfaceMouseIsOver = null;
    }

    /// <summary>
    /// 마우스가 슬롯에 들어왔을 때
    /// </summary>
    /// <param name="go">슬롯 UI 오브젝트</param>
    public void OnEnterSlot(GameObject go)
    {
        // 머무는 슬롯 데이터 저장
        MouseData.slotHoveredOver = go;
    }

    /// <summary>
    /// 마우스가 슬롯에서 나갔을 때
    /// </summary>
    /// <param name="go">슬롯 UI 오브젝트</param>
    public void OnExitSlot(GameObject go)
    {
        // 머무는 슬롯 데이터 초기화
        MouseData.slotHoveredOver = null;
    }

    /// <summary>
    /// 드래그 시작 
    /// </summary>
    /// <param name="go">슬롯 UI 오브젝트</param>
    public void OnStartDrag(GameObject go)
    {
        // 드래그되는 아이템 이미지 생성
        MouseData.tempItemBeginDragged = CreateDragImage(go);
    }

    /// <summary>
    /// 드래그되는 아이템 이미지를 생성하는 함수
    /// </summary>
    /// <param name="go">슬롯 UI 오브젝트</param>
    /// <returns>드래그되는 아이템 오브젝트</returns>
    GameObject CreateDragImage(GameObject go)
    {
        // 빈슬롯이라면 리턴
        if (slotUIs[go].item.id < 0)
            return null;

        // 게임 오브젝트 생성
        GameObject dragImage = new GameObject();

        // RectTransform 컴포넌트 추가 및 사이즈 지정
        RectTransform rectTransform = dragImage.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        // 부모 지정
        dragImage.transform.SetParent(transform.parent);

        // Image 컴포넌트 추가 및 아이콘 지정
        Image image = dragImage.AddComponent<Image>();
        image.sprite = slotUIs[go].ItemObject.icon;
        // 레이캐스트 타겟 비활성화
        image.raycastTarget = false;

        // 이름 지정
        dragImage.name = "Drag Image";

        return dragImage;
    }

    /// <summary>
    /// 드래그 중
    /// </summary>
    /// <param name="go">드래그 오브젝트</param>
    public void OnDrag(GameObject go)
    {
        // 드래그 오브젝트가 없다면 리턴
        if (MouseData.tempItemBeginDragged == null)
            return;

        // 드래그 오브젝트의 위치를 마우스 위치로 업데이트
        MouseData.tempItemBeginDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    /// <summary>
    /// 드래그 끝
    /// </summary>
    /// <param name="go">드래그 오브젝트</param>
    public void OnEndDrag(GameObject go)
    {
        // 드래그 오브젝트 제거
        Destroy(MouseData.tempItemBeginDragged);

        // 인벤토리 UI 오브젝트안에 마우스가 없다면 삭제 행동 처리
        if(MouseData.interfaceMouseIsOver == null)
        {
            slotUIs[go].RemoveItem();
        }
        // 인벤토리 UI 오브젝트 내부의 슬롯에 마우스가 있다면
        else if (MouseData.slotHoveredOver)
        {
            // 슬롯을 서로 스왑
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotData);
        }
    }

    /// <summary>
    /// 클릭 이벤트 로직 처리 함수
    /// </summary>
    /// <param name="go">슬롯 UI 오브젝트</param>
    /// <param name="data">클릭된 정보를 처리하기 위한 이벤트 데이터</param>
    public void OnClick(GameObject go, PointerEventData data)
    {
        // 슬롯이 비어있다면 리턴
        InventorySlot slot = slotUIs[go];
        if (slot == null)
            return;

        // 왼쪽 클릭인 경우 
        if(data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }
        // 오른쪽 클릭인 경우
        else if(data.button == PointerEventData.InputButton.Right)
        {
            OnRightClick(slot);
        }
    }

    /// <summary>
    /// 장비창이나 인벤토리 창에서 Override 하여 사용
    /// 슬롯을 왼쪽 클릭한 경우의 로직 처리 함수
    /// </summary>
    /// <param name="slot">슬롯</param>
    protected virtual void OnLeftClick(InventorySlot slot)
    {

    }

    /// <summary>
    /// 장비창이나 인벤토리 창에서 Override 하여 사용
    /// 슬롯을 오른쪽 클릭한 경우의 로직 처리 함수
    /// </summary>
    /// <param name="slot">슬롯</param>
    protected virtual void OnRightClick(InventorySlot slot)
    {

    }
    #endregion Mouse Events
}
