using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트 타입 정의
/// </summary>
public enum QuestType : int
{
    DestroyEnemy,   // 적을 처지하는 퀘스트
    AcquireItem,    // 아이템을 획득하는 퀘스트
}

/// <summary>
/// 퀘스트 진행 상태 정의
/// </summary>
public enum QuestStatus : int
{
    None,       // 아무것도 하지 않은 상태
    Accepted,   // 퀘스트를 수락한 상태
    Completed,  // 퀘스트를 완료한 상태
    Rewarded,   // 보상을 수령한 상태 (완전한 퀘스트 완료)
}
