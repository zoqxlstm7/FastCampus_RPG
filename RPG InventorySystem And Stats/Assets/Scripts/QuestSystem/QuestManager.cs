using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singletone
    static QuestManager instance = null;
    public static QuestManager Instance => instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion Singletone

    #region Variables
    public QuestDatabaseObject questDatabase = null;    // 퀘스트 데이터베이스

    // 퀘스트 완료시 호출될 이벤트 함수
    public event System.Action<QuestObject> OnCompletedQuest = null;
    #endregion Variables

    #region Main Methods
    /// <summary>
    /// 플레이어의 퀘스트 진행을 처리해주기 위한 함수
    /// /* 에너미 컨트롤러의 에너미 사망시 TakeDamage에서 ProcessQuest()함수를 호출*/
    /// /* QuestManager.Instance.ProcessQuest(QuestType.DestroyEnemy, 0) */
    /// /* 인벤토리 오브젝트에서 아이템을 얻을 때 AddItem에서 ProcessQuest()함수를 호출 */
    /// /* QuestManager.Instance.ProcessQuest(QuestType.AcquireItem, 1) */
    /// 적과 아이템 데이터베이스에서 아이디 값을 정의하여 아이디값을 넘겨주면 됨
    /// </summary>
    /// <param name="type">퀘스트 타입</param>
    /// <param name="targetId">타겟 ID</param>
    public void ProcessQuest(QuestType type, int targetId)
    {
        foreach (QuestObject questObject in questDatabase.questObjects)
        {
            // 현재 진행하고 있는 퀘스트라면
            // 퀘스트를 수락한 상태이면서 파라미터의 퀘스트 정보와 일치한다면
            if(questObject.status == QuestStatus.Accepted && questObject.data.type == type && questObject.data.targetId == targetId)
            {
                // 퀘스트 완료 갯수 증가
                questObject.data.completedCount++;
                // 퀘스트를 완료했다면
                if(questObject.data.completedCount >= questObject.data.count)
                {
                    // 퀘스트 상태를 완료 상태로 변경 및 퀘스트 완료 이벤트 함수 호출
                    questObject.status = QuestStatus.Completed;
                    OnCompletedQuest?.Invoke(questObject);
                }
            }
        }
    }
    #endregion Main Methods
}
