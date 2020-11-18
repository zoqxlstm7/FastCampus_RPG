using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifiableInt
{
    #region Variables
    [System.NonSerialized]
    int baseValue = 0;      // 기본 수치

    [SerializeField]    
    int modifiedValue;      // 최종 수치

    // 수치 정보 리스트
    List<IModifier> modifiers = new List<IModifier>();

    // 스탯이 변경되었을 때 변경된 정보를
    // 다른 오브젝트(플레이어 캐릭터, UI)가 처리할 수 있도록 하는 이벤트
    private event System.Action<ModifiableInt> OnModifiedValue;
    #endregion Variables

    #region Property
    // BaseValue 반환 및 설정
    public int BaseValue
    {
        get => baseValue;
        set
        {
            baseValue = value;
            // 수치 정보 갱신
            UpdateModifiedValue();
        }
    }

    // 최종 수치 변경 및 반환
    public int ModifiedValue
    {
        get => modifiedValue;
        set => modifiedValue = value;
    }
    #endregion Property

    #region Main Methods
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="method"></param>
    public ModifiableInt(System.Action<ModifiableInt> method = null)
    {
        // 최종값 및 이벤트 초기화
        modifiedValue = baseValue;
        RegisterModEvent(method);
    }

    /// <summary>
    /// 이벤트를 등록하는 함수
    /// </summary>
    /// <param name="method">이벤트 함수</param>
    public void RegisterModEvent(System.Action<ModifiableInt> method)
    {
        if (method != null)
        {
            OnModifiedValue += method;
        }
    }

    /// <summary>
    /// 이벤트를 해제하는 함수
    /// </summary>
    /// <param name="method">이벤트 함수</param>
    public void UnRegisterModEvent(System.Action<ModifiableInt> method)
    {
        if (method != null)
        {
            OnModifiedValue -= method;
        }
    }

    /// <summary>
    /// Modifiers에 수치가 추가, 삭제될 때 최종수치를 갱신하는 함수
    /// </summary>
    void UpdateModifiedValue()
    {
        // 변경된 수치 합산
        int valueToAdd = 0;
        foreach (IModifier modifier in modifiers)
        {
            modifier.AddValue(ref valueToAdd);
        }

        // 최종 수치 적용
        modifiedValue = baseValue + valueToAdd;
        // UI 갱신 이벤트가 있다면 이벤트 호출
        OnModifiedValue?.Invoke(this);
    }

    /// <summary>
    /// 수치를 추가하는 함수
    /// </summary>
    /// <param name="modifier">수치 정보</param>
    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
        UpdateModifiedValue();
    }

    /// <summary>
    /// 수치를 제거하는 함수
    /// </summary>
    /// <param name="modifier">수치 정보</param>
    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);
        UpdateModifiedValue();
    }
    #endregion Main Methods
}
