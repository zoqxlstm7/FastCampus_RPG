using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats System/New Character Stats")]
public class StatsObject : ScriptableObject
{
    #region Variables
    public Attribute[] attributes = null;   // 속성 배열

    public int level = 1;                   // 레벨
    public int exp = 0;                     // 경험치

    // 스탯이 변경되었을 때 변경된 정보를
    // 다른 오브젝트(플레이어 캐릭터, UI)가 처리할 수 있도록 하는 이벤트
    public event System.Action<StatsObject> OnChangedStats = null;

    [System.NonSerialized]
    bool isInitialized = false;             // 초기화 여부
    #endregion Variables

    #region Property
    // Serialized하지 않기 위해 프로퍼티로 선언
    public int Health { get; set; }
    public int Mana { get; set; }

    // 체력 퍼센트를 반환
    public float HealthPercentage
    {
        get
        {
            int health = Health;
            int maxHealth = health;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == CharacterAttribute.Health)
                {
                    maxHealth = attribute.value.ModifiedValue;
                }                
            }

            return (maxHealth > 0 ? ((float)health / (float)maxHealth) : 0);
        }
    }

    // 마나 퍼센트를 반환
    public float ManaPercentage
    {
        get
        {
            int mana = Mana;
            int maxMana = mana;

            foreach (Attribute attribute in attributes)
            {
                if(attribute.type == CharacterAttribute.Mana)
                {
                    maxMana = attribute.value.ModifiedValue;
                }
            }

            return (maxMana > 0 ? ((float)mana / (float)maxMana) : 0);
        }
    }
    #endregion Property

    #region Unity Methods
    /// <summary>
    /// ScriptableObject이므로 OnEnable에서 초기화
    /// </summary>
    private void OnEnable()
    {
        // 속성 초기화
        InitializeAttributes();
    }

    // 임의적 추가
    private void OnDisable()
    {
        foreach (Attribute attribute in attributes)
        {
            attribute.value.UnRegisterModEvent(OnModifiedValue);
        }
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 속성을 초기화하는 함수
    /// 현재는 로컬로 데이터 초기화
    /// (실제 사용시 서버나 저장된 데이터를 사용하여 초기화)
    /// </summary>
    public void InitializeAttributes()
    {
        // 초기화가 진행된 상태라면 리턴
        if (isInitialized)
            return;

        isInitialized = true;
        Debug.Log("InitializeAttributes");

        // 속성 생성
        foreach (Attribute attribute in attributes)
        {
            attribute.value = new ModifiableInt(OnModifiedValue);
        }

        level = 1;
        exp = 0;

        // 각 속성들에 대한 기본값 초기화
        SetBaseValue(CharacterAttribute.Agility, 100);
        SetBaseValue(CharacterAttribute.Intellect, 100);
        SetBaseValue(CharacterAttribute.Stamina, 100);
        SetBaseValue(CharacterAttribute.Strength, 100);
        SetBaseValue(CharacterAttribute.Health, 100);
        SetBaseValue(CharacterAttribute.Mana, 100);

        // 설정된 기본값으로 초기화
        Health = GetModifiedValue(CharacterAttribute.Health);
        Mana = GetModifiedValue(CharacterAttribute.Mana);
    }

    /// <summary>
    /// 스탯 정보가 변경될 때 호출되는 이벤트 함수
    /// </summary>
    /// <param name="value">수치</param>
    void OnModifiedValue(ModifiableInt value)
    {
        // 갱신될 정보가 있다면 호출
        OnChangedStats?.Invoke(this);
    }

    /// <summary>
    /// 속성의 기본 수치를 설정하는 함수
    /// </summary>
    /// <param name="type">속성 타입</param>
    /// <param name="value">수치</param>
    public void SetBaseValue(CharacterAttribute type, int value)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
            {
                attribute.value.BaseValue = value;
            }
        }
    }

    /// <summary>
    /// 속성의 기본 수치를 반환하는 함수
    /// </summary>
    /// <param name="type">속성 타입</param>
    /// <returns>기본값</returns>
    public int GetBaseValue(CharacterAttribute type)
    {
        foreach (Attribute attribute in attributes)
        {
            if(attribute.type == type)
            {
                return attribute.value.BaseValue;
            }
        }

        // 오류시 -1 반환
        return -1;
    }

    /// <summary>
    /// 속성의 최종 수치를 반환하는 함수
    /// </summary>
    /// <param name="type">속성 타입</param>
    /// <returns>최종 수치</returns>
    public int GetModifiedValue(CharacterAttribute type)
    {
        foreach (Attribute attribute in attributes)
        {
            if(attribute.type == type)
            {
                return attribute.value.ModifiedValue;
            }
        }

        // 오류시 -1 반환
        return -1;
    }

    /// <summary>
    /// Attribute에 속하지 않는 현재 체력을 별도 처리
    /// 현재 체력을 회복하는 함수
    /// </summary>
    /// <param name="value">수치</param>
    /// <returns>체력 수치</returns>
    public int AddHealth(int value)
    {
        Health += value;

        OnChangedStats?.Invoke(this);
        return Health;
    }

    /// <summary>
    /// Attribute에 속하지 않는 현재 마나를 별도 처리
    /// 현재 마나를 회복하는 함수
    /// </summary>
    /// <param name="value">수치</param>
    /// <returns>마나 수치</returns>
    public int AddMana(int value)
    {
        Mana += value;

        OnChangedStats?.Invoke(this);
        return Mana;
    }
    #endregion Main Methods
}
