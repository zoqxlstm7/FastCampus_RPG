using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    #region Variables
    public int id;              // 퀘스트 식별 ID

    public QuestType type;      // 퀘스트 타입
    public int targetId;        // 타겟 ID (ex. 몇번 ID를 가진 적을 죽여라)

    public int count;           // 전체 완료 갯수
    public int completedCount;  // 현재 완료한 갯수

    /// <summary>
    /// Todo: 여러 보상에 대한 처리는 배열로 처리
    /// </summary>
    public int rewardExp;       // 보상 경험치
    public int rewardGold;      // 보상 골드
    public int rewardItemId;    // 보상 아이템 ID

    public string title;        // 퀘스트 타이틀
    public string description;  // 퀘스트 설명
    #endregion Variables
}
