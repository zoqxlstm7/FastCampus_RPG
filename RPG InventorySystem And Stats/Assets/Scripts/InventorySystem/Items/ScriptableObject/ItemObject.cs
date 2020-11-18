using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{
    #region Variables
    public ItemType type = ItemType.Default;            // 아이템 타입
    public bool stackable = false;                      // 쌓을 수 있는지 여부

    public Sprite icon = null;                          // 아이콘
    public GameObject modelPrefab = null;               // 아이템 모델

    public Item data = new Item();                      // 아이템 데이터

    public List<string> boneNames = new List<string>(); // 본이름 리스트

    [TextArea(15, 20)]
    public string description = null;                   // 설명문
    #endregion Variables

    #region Unity Methods
    /// <summary>
    /// 데이터를 변경하면 호출되는 함수
    /// </summary>
    private void OnValidate()
    {
        boneNames.Clear();

        // 아이템 모델이 없거나, 아이템 모델의 자식들 중 SkinnedMeshRenderer가 없다면 리턴
        if (modelPrefab == null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
            return;

        // 자식 오브젝트의 SkinnedMeshRenderer에서 본이름들을 저장
        SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;

        // 본이름을 리스트에 저장
        foreach (Transform boneTransform in bones)
        {
            boneNames.Add(boneTransform.name);
        }
    }
    #endregion Unity Methods

    #region Main Methods
    /// <summary>
    /// 아이템 오브젝트를 활용하여 아이템을 생성하는 함수
    /// </summary>
    /// <returns>아이템</returns>
    public Item CreateItem()
    {
        // 아이템 오브젝트의 정보를 이용하여 아이템 생성
        Item newItem = new Item(this);
        return newItem;
    }
    #endregion Main Methods
}
