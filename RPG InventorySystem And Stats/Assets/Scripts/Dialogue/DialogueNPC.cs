using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    #region Variables
    [SerializeField]
    Dialogue dialogue = null;       // 다이얼로그

    bool isStartDialogue = false;   // 대화 시작 여부

    GameObject interactGo = null;   // 상호작용을 시도한 오브젝트
    #endregion Variables

    #region IInteractable Interface
    [SerializeField]
    float distance = 2.0f;

    public float Distance => distance;

    /// <summary>
    /// 상호작용 시 로직 처리 함수
    /// </summary>
    /// <param name="other">상호작용을 시도한 오브젝트</param>
    /// <returns>상호작용 여부</returns>
    public bool Interact(GameObject other)
    {
        // 거리 계산
        float calcDistance = Vector3.Distance(other.transform.position, transform.position);
        // 상호작용 거리내에 없다면 리턴
        if (calcDistance > distance)
            return false;
        // 대화가 시작되었다면 리턴
        if (isStartDialogue)
            return false;

        // 상호작용 오브젝트 캐싱
        interactGo = other;

        // 대화 종료시 호출될 이벤트 함수 설정
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartDialogue = true;

        // 대화 시작
        DialogueManager.Instance.StartDialogue(dialogue);

        return true;
    }

    /// <summary>
    /// 상호작용 종료시 로직 처리 함수
    /// </summary>
    /// <param name="other">상호작용을 시도한 오브젝트</param>
    public void StopInteract(GameObject other)
    {
        isStartDialogue = false;

        // 플레이어라면 타겟 리셋
        Player player = other?.GetComponent<Player>();
        if (player)
            player.RemoveTarget();
    }
    #endregion IInteractable Interface

    #region Main Methods
    /// <summary>
    /// 대화 종료시 호출될 이벤트 함수
    /// </summary>
    void OnEndDialogue()
    {
        StopInteract(interactGo);
    }
    #endregion Main Methods
}
