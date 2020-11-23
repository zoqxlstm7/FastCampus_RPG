using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour, IInteractable
{
    #region Variables
    public QuestObject questObject = null;      // 퀘스트 오브젝트

    public Dialogue readyDialogue = null;       // 퀘스트 준비 문장
    public Dialogue acceptedDialogue = null;    // 퀘스트 수락 문장
    public Dialogue completedDialogue = null;   // 퀘스트 완료 문장

    bool isStartQuestDialogue = false;          // 대화 시작 여부
    GameObject interactGo = null;               // 상호작용을 시도한 오브젝트

    [SerializeField]
    GameObject questEffectGo = null;            // 퀘스트 이펙트
    [SerializeField]
    GameObject questRewardGo = null;            // 퀘스트 보상 이펙트
    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        // 이펙트 비활성화
        questEffectGo.SetActive(false);
        questRewardGo.SetActive(false);

        // 퀘스트를 진행할 수 있다면 퀘스트 이펙트 활성화
        // 퀘스트를 완료햇다면 퀘스트 보상 이펙트 활성화
        if(questObject.status == QuestStatus.None)
        {
            questEffectGo.SetActive(true);
        }
        else if(questObject.status == QuestStatus.Completed)
        {
            questRewardGo.SetActive(true);
        }

        // 퀘스트 완료 이벤트 함수 등록
        QuestManager.Instance.OnCompletedQuest += OnCompletedQuest;
    }
    #endregion Unity Methods

    #region IInteractable Interface
    public float distance = 2.0f;
    public float Distance => distance;

    /// <summary>
    /// 상호작용 처리 함수
    /// </summary>
    /// <param name="other">상호작용을 시도한 오브젝트</param>
    /// <returns>상호작용 성공 여부</returns>
    public bool Interact(GameObject other)
    {
        // 거리 검사
        float calcDistance = Vector3.Distance(other.transform.position, transform.position);
        if(calcDistance > distance)
        {
            return false;
        }
        // 대화가 시작되었다면 리턴
        if (isStartQuestDialogue)
        {
            return false;
        }

        // 상호작용을 시도한 오브젝트 캐싱
        interactGo = other;

        // 대화 시작 플래그 설정 및 대화 종료 이벤트 함수 등록
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartQuestDialogue = true;

        // 퀘스트를 받지 않은 상태라면 퀘스트 준비 문장 노출 후 퀘스트 수락 상태로 변경
        if (questObject.status == QuestStatus.None)
        {
            DialogueManager.Instance.StartDialogue(readyDialogue);
            questObject.status = QuestStatus.Accepted;
        }
        // 퀘스트를 수락한 상태라면 퀘스트 수락 문장 노출
        else if(questObject.status == QuestStatus.Accepted)
        {
            DialogueManager.Instance.StartDialogue(acceptedDialogue);
        }
        // 퀘스트를 완료한 상태라면 퀘스트 완료 문장 노출
        else if(questObject.status == QuestStatus.Completed)
        {
            DialogueManager.Instance.StartDialogue(completedDialogue);

            /*
             * Todo: Process Reward (보상 지급)
             */

            // 퀘스트 보상 상태로 변경 및 퀘스트 관련 이펙트 비활성화
            questObject.status = QuestStatus.Rewarded;
            questEffectGo.SetActive(false);
            questRewardGo.SetActive(false);
        }

        return true;
    }

    /// <summary>
    /// 상호작용 종료시 처리 함수
    /// </summary>
    /// <param name="other">상호작용을 시도한 오브젝트</param>
    public void StopInteract(GameObject other)
    {
        isStartQuestDialogue = false;

        Player player = other?.GetComponent<Player>();
        if (player)
        {
            player.RemoveTarget();
        }
    }
    #endregion IInteractable Interface

    #region Main Methods
    /// <summary>
    /// 대화 종료 이벤트 함수
    /// </summary>
    void OnEndDialogue()
    {
        // 상호작용 종료 함수 호출
        StopInteract(interactGo);
    }

    /// <summary>
    /// 퀘스트 완료시 호출되는 이벤트 함수
    /// </summary>
    /// <param name="questObject">퀘스트 오브젝트</param>
    void OnCompletedQuest(QuestObject questObject)
    {
        // 퀘스트 오브젝트 ID가 같고, 퀘스트 완료 상태라면
        if(questObject.data.id == this.questObject.data.id && questObject.status == QuestStatus.Completed)
        {
            // 퀘스트 이펙트는 비활성화하고 퀘스트 보상 이펙트 활성화
            questEffectGo.SetActive(false);
            questRewardGo.SetActive(true);
        }
    }
    #endregion Main Methods
}
