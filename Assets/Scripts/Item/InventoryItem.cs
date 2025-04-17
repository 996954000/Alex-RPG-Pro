
using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
    }

    // 物品堆叠数量管理，后面肯定要改成传参形式
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;


}
