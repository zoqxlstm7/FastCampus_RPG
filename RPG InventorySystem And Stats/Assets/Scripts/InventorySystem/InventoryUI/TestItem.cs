using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 테스트용 스크립트
/// </summary>
public class TestItem : MonoBehaviour
{
    public InventoryObject equipmentObject = null;
    public InventoryObject inventoryObject = null;
    public ItemObjectDatabase databaseObject = null;

    public void AddNewItem()
    {
        if(databaseObject.itemObjects.Length > 0)
        {
            ItemObject newItemObject = databaseObject.itemObjects[Random.Range(0, databaseObject.itemObjects.Length - 1)];
            Item newItem = new Item(newItemObject);

            inventoryObject.AddItem(newItem, 1);
        }
    }

    public void ClearInventory()
    {
        equipmentObject?.Clear();
        inventoryObject?.Clear();
    }
}
