using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 타입 정의
/// </summary>
public enum ItemType : int
{
    Helmet = 0,     // 헬멧
    Chest,          // 갑옷
    Pants,          // 바지
    Boots,          // 신발
    Pauldrons,      // 견갑
    Gloves,         // 장갑
    LeftWeapon,     // 왼쪽 무기
    RightWeapon,    // 오른쪽 무기
    Food,           // 음식
    Default,        // 기본
}
