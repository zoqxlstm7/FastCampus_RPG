using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마우스가 오버랩된 UI 오브젝트를 구별하기 위해
/// 관련 컴포넌트를 임시적으로 가지고 있는 Static 클래스
/// </summary>
public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver; // 오버된 인벤토리 UI
    public static GameObject slotHoveredOver;       // 오버된 슬롯 UI 오브젝트
    public static GameObject tempItemBeginDragged;  // 드래그되는 아이템 UI 오브젝트
}
