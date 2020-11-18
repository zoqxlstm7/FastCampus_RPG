using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 인벤토리의 아이템을 이용하여 실제적으로 
/// 장착을 수행하는 클래스
/// </summary>
public class PlayerEquipment : MonoBehaviour
{
    #region Variables
    // 장비창에 대한 인벤토리 오브젝트
    public InventoryObject equiment;
    // combiner 유틸리티
    EquipmentCombiner combiner;

    // 아이템 오브젝트를 관리하기 위한 클래스
    ItemInstances[] itemInstances = new ItemInstances[8];

    // 기본적으로 장착되어야할 아이템 오브젝트
    [Header("Default Equipment: H = 0, C = 1, P = 2, B = 3, Pa = 4, G = 5, LW = 6, RW = 7")]
    public ItemObject[] defaultItemObjects = new ItemObject[8];
    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        // 게임 오브젝트의 본정보를 저장
        combiner = new EquipmentCombiner(gameObject);

        // 장비 슬롯에 아이템이 장착되거나 빠질 때 호출될 이벤트 함수 설정
        for (int i = 0; i < equiment.Slots.Length; i++)
        {
            equiment.Slots[i].OnPreUpdate += OnRemoveItem;
            equiment.Slots[i].OnPostUpdate += OnEquipItem;
        }
    }

    private void Start()
    {
        // 장비슬롯의 정보를 가져와 기본적인 장비를 착용하도록 설정
        foreach (InventorySlot slot in equiment.Slots)
        {
            OnEquipItem(slot);
        }
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 장비슬롯 정보를 이용하여 아이템을 장착하는 함수
    /// </summary>
    /// <param name="slot">장비 슬롯</param>
    void OnEquipItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        // 장비 슬롯이 비어있다면 기본 아이템을 장착
        if(itemObject == null)
        {
            EquipDefaultItemBy(slot.allowedItems[0]);
            return;
        }

        // 장착 아이템 타입을 인덱스로 변환
        int index = (int)slot.allowedItems[0];
        // 타입에 맞는 아이템의 형식대로 아이템 장착 실행
        // Skinned Mesh와 Static Mesh가 조합되어 있는 형태
        switch (slot.allowedItems[0])
        {
            case ItemType.Helmet:
            case ItemType.Chest:
            case ItemType.Pants:
            case ItemType.Boots:
            case ItemType.Gloves:
                itemInstances[index] = EquipSkinnedItem(itemObject);
                break;

            case ItemType.Pauldrons:
            case ItemType.LeftWeapon:
            case ItemType.RightWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
            default:
                break;
        }

        // 옵션: 
        // 장착이 정상적으로 이루어졌다면 이름 변경
        if(itemInstances[index] != null)
        {
            itemInstances[index].name = slot.allowedItems[0].ToString();
        }
    }

    /// <summary>
    /// Skinned 아이템을 장착하는 함수
    /// </summary>
    /// <param name="itemObject">아이템 오브젝트</param>
    /// <returns>아이템 인스턴스</returns>
    ItemInstances EquipSkinnedItem(ItemObject itemObject)
    {
        // 아이템 오브젝트가 존재하지 않는다면 리턴
        if (itemObject == null)
            return null;

        // 아이템 오브젝트의 모델프리팹과, 본정보를 이용하여 Skinned Mesh 생성
        Transform itemTransform = combiner.AddLimb(itemObject.modelPrefab, itemObject.boneNames);
        if(itemTransform != null)
        {
            // 새로 생성된 Skinned Mesh 오브젝트에 ItemInstances 컴포넌트 추가
            ItemInstances instance = itemTransform.gameObject.AddComponent<ItemInstances>();
            // 아이템 인스턴스 추가
            instance.itemTransforms.Add(itemTransform);

            return instance;
        }

        return null;
    }

    /// <summary>
    /// Static Mesh 아이템을 장착하는 함수
    /// </summary>
    /// <param name="itemObject">아이템 오브젝트</param>
    /// <returns>아이템 인스턴스</returns>
    ItemInstances EquipMeshItem(ItemObject itemObject)
    {
        // 아이템 오브젝트가 존재하지 않는다면 리턴
        if (itemObject == null)
            return null;

        // 아이템 오브젝트의 모델프리팹 정보를 이용하여 Static Mesh 생성
        Transform[] itemTransforms = combiner.AddMesh(itemObject.modelPrefab);
        if(itemTransforms.Length > 0)
        {
            // Static Mesh 정보 저장
            ItemInstances instance = new GameObject().AddComponent<ItemInstances>();
            instance.itemTransforms.AddRange(itemTransforms.ToList<Transform>());
            //foreach (Transform t in itemTransforms)
            //{
            //    instance.itemTransforms.Add(t);
            //}

            // 부모 설정
            instance.transform.parent = transform;

            return instance;
        }

        return null;
    }

    /// <summary>
    /// 장비슬롯이 비어있을 때 기본 아이템을 장착하는 함수
    /// </summary>
    /// <param name="type">아이템 타입</param>
    void EquipDefaultItemBy(ItemType type)
    {
        int index = (int)type;

        // 설정된 기본장비에서 아이템 오브젝트를 설정
        ItemObject itemObject = defaultItemObjects[index];
        // 유형에 따라 아이템 장착
        switch (type)
        {
            case ItemType.Helmet:
            case ItemType.Chest:
            case ItemType.Pants:
            case ItemType.Boots:
            case ItemType.Gloves:
                itemInstances[index] = EquipSkinnedItem(itemObject);
                break;

            case ItemType.Pauldrons:
            case ItemType.LeftWeapon:
            case ItemType.RightWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 장비슬롯의 아이템을 제거하는 함수
    /// </summary>
    /// <param name="slot">장착 슬롯</param>
    void OnRemoveItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        // 아이템 오브젝트가 비어있다면 아이템 삭제
        if(itemObject == null)
        {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }

        // 아이템 오브젝트의 모델프리팹이 존재한다면 아이템 삭제
        if(slot.ItemObject.modelPrefab != null)
        {
            RemoveItemBy(slot.allowedItems[0]);
        }
    }

    /// <summary>
    /// 아이템 타입에 따라 장착부위의 아이템을 제거하는 함수
    /// </summary>
    /// <param name="type">아이템 타입</param>
    void RemoveItemBy(ItemType type)
    {
        int index = (int)type;
        // 아이템이 장착되어있다면 오브젝트 제거
        if (itemInstances[index] != null)
        {
            Destroy(itemInstances[index].gameObject);
            itemInstances[index] = null;
        }
    }
    #endregion Main Methods
}
