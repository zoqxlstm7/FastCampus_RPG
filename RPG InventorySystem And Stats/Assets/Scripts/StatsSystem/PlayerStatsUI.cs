using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    #region Variables
    public InventoryObject equipment = null;    // 장비 인벤토리
    public StatsObject playerStats = null;      // 스탯 오브젝트

    public Text[] attributeTexts;               // 속성 텍스트 배열
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        // 이벤트 등록
        playerStats.OnChangedStats += OnChangedStats;

        // 슬롯 갱신시 호출될 이벤트 등록
        if(equipment != null && playerStats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTexts();
    }

    private void OnDisable()
    {
        // 이벤트 해제
        playerStats.OnChangedStats -= OnChangedStats;

        // 슬롯 갱신시 호출될 이벤트 해제
        if(equipment != null && playerStats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdate -= OnEquipItem;
            }
        }
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 속성 텍스트들을 업데이트하는 함수
    /// </summary>
    void UpdateAttributeTexts()
    {
        attributeTexts[0].text = playerStats.GetModifiedValue(CharacterAttribute.Agility).ToString("n0");
        attributeTexts[1].text = playerStats.GetModifiedValue(CharacterAttribute.Intellect).ToString("n0");
        attributeTexts[2].text = playerStats.GetModifiedValue(CharacterAttribute.Strength).ToString("n0");
        attributeTexts[3].text = playerStats.GetModifiedValue(CharacterAttribute.Stamina).ToString("n0");
    }

    /// <summary>
    /// 장비 아이템의 장착이 해제되었을 때 호출될 이벤트 함수
    /// </summary>
    /// <param name="slot">슬롯</param>
    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;

        Debug.Log("OnRemoveItem");

        // 슬롯의 장비아이템의 버프와 같은 속성을 제거
        if (slot.Parent.type == InterfaceType.Equipment)
        {
            foreach (ItemBuff buff in slot.item.buffs)
            {
                foreach (Attribute attribute in playerStats.attributes)
                {
                    if (attribute.type == buff.stat)
                    {
                        attribute.value.RemoveModifier(buff);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 장비 아이템이 장착되었을 때 호출될 이벤트 함수
    /// </summary>
    /// <param name="slot">슬롯</param>
    public void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;

        Debug.Log("OnEquipItem");

        // 슬롯의 장비아이템의 버프와 같은 속성을 추가
        if (slot.Parent.type == InterfaceType.Equipment)
        {
            foreach (ItemBuff buff in slot.item.buffs)
            {
                foreach (Attribute attribute in playerStats.attributes)
                {
                    if (attribute.type == buff.stat)
                    {
                        attribute.value.AddModifier(buff);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 스탯이 변경되었을 때 호출될 이벤트 함수
    /// </summary>
    /// <param name="statsObject">스탯 오브젝트</param>
    public void OnChangedStats(StatsObject statsObject)
    {
        // 속성 텍스트 갱신
        Debug.Log("OnChangedStats");
        UpdateAttributeTexts();
    }
    #endregion Main Methods
}
