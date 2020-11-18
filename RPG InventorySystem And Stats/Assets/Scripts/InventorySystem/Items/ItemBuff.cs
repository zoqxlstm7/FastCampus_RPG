using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemBuff : IModifier
{
    #region Variables
    public CharacterAttribute stat;     // 캐릭터 스탯
    public int value = 0;               // 능력치

    [SerializeField] int min = 0;       // 최소 능력치
    [SerializeField] int max = 0;       // 최대 능력치
    #endregion Variables

    #region Property
    public int Min => min;
    public int Max => max;
    #endregion Property

    #region Main Methods
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="min">최솟값</param>
    /// <param name="max">최댓값</param>
    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;

        // 최소, 최대값을 이용하여 랜덤 능력치 부여
        GenerateValue();
    }

    /// <summary>
    /// 랜덤한 능력치를 부여하는 함수
    /// </summary>
    public void GenerateValue()
    {
        value = Random.Range(min, max);
    }
    #endregion Main Methods

    #region IModifier Interface
    /// <summary>
    /// 기본 능력치에 버프 능력치를 더해주는 함수
    /// </summary>
    /// <param name="baseValue">기본 능력치</param>
    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }
    #endregion IModifier Interface
}
