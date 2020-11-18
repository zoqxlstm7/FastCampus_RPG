using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    #region Variables
    public CharacterAttribute type; // 캐릭터 버프 속성 타입
    public ModifiableInt value;     // 수치 정보
    #endregion Variables    
}
