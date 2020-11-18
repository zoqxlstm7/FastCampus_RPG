using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    #region Variables
    public float distance = 3.0f;           // 인터랙트 가능 거리

    public ItemObject itemObject = null;    // 아이템 오브젝트
    public int amount = 1;                  // 아이템 수량
    #endregion Variables

    #region Unity Methods
    /// <summary>
    /// 스프라이트 렌더러에 아이콘 설정 
    /// </summary>
    private void OnValidate()
    {
#if UNITY_EDITOR
        GetComponent<SpriteRenderer>().sprite = itemObject?.icon;
#endif 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
    #endregion Unity Methods

    #region IInteractable Interface
    public float Distance => distance;  // 거리 반환

    /// <summary>
    /// 인터랙트 시 처리하는 함수
    /// </summary>
    /// <param name="other">인터랙트한 오브젝트</param>
    /// <returns></returns>
    public bool Interact(GameObject other)
    {
        // 인터랙트 가능 거리보다 멀다면 리턴
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if (calcDistance > distance)
            return false;

        // Player 오브젝트라면 인터랙트 처리
        return other.GetComponent<Player>()?.PickupItem(this, amount) ?? false;
    }

    /// <summary>
    /// 인터랙트 정지 시 처리하는 함수
    /// </summary>
    /// <param name="other">인터랙트한 오브젝트</param>
    public void StopInteract(GameObject other)
    {

    }
    #endregion IInteractable Interface
}
