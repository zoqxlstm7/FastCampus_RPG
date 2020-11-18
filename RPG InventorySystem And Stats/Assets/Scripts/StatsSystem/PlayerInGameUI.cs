using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameUI : MonoBehaviour
{
    #region Variables
    public StatsObject playerStats = null;  // 스탯 오브젝트

    public Text levelText = null;           // 레벨 텍스트
    public Image healthSlider = null;       // 체력 슬라이더
    public Image manaSlider = null;         // 마나 슬라이더
    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        // UI 정보 초기화
        levelText.text = playerStats.level.ToString("n0");

        healthSlider.fillAmount = playerStats.HealthPercentage;
        manaSlider.fillAmount = playerStats.ManaPercentage;
    }

    private void OnEnable()
    {
        // 이벤트 등록
        playerStats.OnChangedStats += OnChangedStats;
    }

    private void OnDisable()
    {
        // 이벤트 해제
        playerStats.OnChangedStats -= OnChangedStats;
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 스탯 정보가 변경될 때 호출될 이벤트 함수
    /// </summary>
    /// <param name="statsObject">스탯 오브젝트</param>
    void OnChangedStats(StatsObject statsObject)
    {
        // 체력, 마나 슬라이더 정보 갱신
        healthSlider.fillAmount = statsObject.HealthPercentage;
        manaSlider.fillAmount = statsObject.ManaPercentage;
    }
    #endregion Main Methods
}
