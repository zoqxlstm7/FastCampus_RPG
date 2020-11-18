using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수치 변환 적용 인터페이스
/// </summary>
public interface IModifier
{
    void AddValue(ref int baseValue);
}
