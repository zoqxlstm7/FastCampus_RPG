using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    #region Variables
    public InventoryObject inventory = null;    // 인벤토리 오브젝트
    public StatsObject playerStats = null;      // 스탯 오브젝트

    Transform target = null;                    // 타겟

    Camera camera = null;                       // 카메라
    NavMeshAgent agent = null;                  // 네브메쉬 에이전트
    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        // 아이템 사용시 호출될 이벤트 함수 등록
        inventory.OnUseItem += OnUseItem;      

        // 초기화
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // 좌클릭 시 체력과 마나 감소
        if (Input.GetMouseButtonDown(0))
        {
            playerStats.AddHealth(-10);
            playerStats.AddMana(-3);
        }

        // 우측 마우스 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("We hit " + hit.collider.name + " " + hit.point);

                // 히트된 오브젝트가 IInteractable 인터페이스가 가지고 있다면 타겟으로 지정
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    SetTarget(hit.collider.transform, interactable.Distance);
                }
            }
        }

        // 타겟이 null이 아니고
        if(target != null)
        {
            // 타겟이 IInteractable 인터페이스를 가지고 있다면
            if (target.GetComponent<IInteractable>() != null)
            {
                // 인터랙트 될수 있는지 검사하여
                // 인터랙트 되었다면 타겟 제거
                IInteractable interactable = target.GetComponent<IInteractable>();
                if (interactable.Interact(gameObject))
                {
                    RemoveTarget();
                }
            }
        }
    }

    /// <summary>
    /// 트리거되어 획득되는 아이템을 처리하는 함수
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // 충돌체에 GroundItem 컴포넌트가 있는 경우
        GroundItem item = other.GetComponent<GroundItem>();
        if(item != null)
        {
            // 아이템을 추가하고 삭제
            if(inventory.AddItem(new Item(item.itemObject), item.amount))
            {
                Destroy(other.gameObject);
            }
        }
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 타겟을 설정하는 함수
    /// </summary>
    /// <param name="newTarget">새로운 타겟</param>
    /// <param name="stoppingDistance">정지거리</param>
    void SetTarget(Transform newTarget, float stoppingDistance)
    {
        // 타겟 설정
        target = newTarget;

        // 에이전트 설정
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(newTarget.position);
    }

    /// <summary>
    /// 타겟을 제거하는 함수
    /// </summary>
    public void RemoveTarget()
    {
        target = null;
    }

    /// <summary>
    /// 아이템을 클릭하여 획득하는 함수
    /// </summary>
    /// <param name="pickupItem">픽업 아이템</param>
    /// <param name="amount">수량</param>
    /// <returns>획득 여부</returns>
    public bool PickupItem(PickupItem pickupItem, int amount)
    {
        // 아이템 오브젝트가 null이 아니고, 아이템이 정상적으로 추가되었다면
        if(pickupItem.itemObject != null && inventory.AddItem(new Item(pickupItem.itemObject), amount))
        {
            // 픽업 아이템 제거
            Destroy(pickupItem.gameObject);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 아이템을 사용하는 함수
    /// </summary>
    /// <param name="itemObject">아이템 오브젝트</param>
    void OnUseItem(ItemObject itemObject)
    {
        // 아이템에 적용되어있는 버프들을 검색
        foreach (ItemBuff buff in itemObject.data.buffs)
        {
            // 버프 유형에 따른 처리
            if(buff.stat == CharacterAttribute.Health)
            {
                ApplyBuff(buff);
            }
        }
    }

    /// <summary>
    /// 속성에 따른 버프 수치를 적용하는 함수
    /// </summary>
    /// <param name="buff">버프</param>
    void ApplyBuff(ItemBuff buff)
    {
        switch (buff.stat)
        {
            case CharacterAttribute.Agility:
                break;
            case CharacterAttribute.Intellect:
                break;
            case CharacterAttribute.Stamina:
                break;
            case CharacterAttribute.Strength:
                break;
            case CharacterAttribute.Health:
                playerStats.Health += buff.value;
                break;
            case CharacterAttribute.Mana:
                break;
        }
    }
    #endregion Main Methods
}
