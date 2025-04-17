
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class Inventory : MonoBehaviour 
{
    public static Inventory instance;

    /* 仓库 */
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    /* 物品储藏 */
    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    /* 装备列表 */
    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public UI_ItemSlot[] uI_InventorySlots;
    public UI_ItemSlot[] uI_StashSlots;
    public UI_EquipmentItemSlot[] uI_EquipSlots;

    /* 物品插槽通用预制件 */
    [SerializeField] private GameObject itemSlotPrefab;

    /* 装备背包父节点 */
    [SerializeField] private Transform itemSlotsParents;
    
    /* 材料背包父节点 */
    [SerializeField] private Transform stashSlotsParents;

    /* 装备栏父节点 */
    [SerializeField] private Transform equipmentSlotsParents;

    /* Slots数量 */
    [SerializeField] private int slotsCount;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }
    public void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipmentItems = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        /* 游戏中固定设置好的装备栏Slots */
        uI_EquipSlots = equipmentSlotsParents.GetComponentsInChildren<UI_EquipmentItemSlot>();
        
        /* 初始化Slots */
        iniSlots();

        /* 获取父节点下Slots */
        uI_InventorySlots = itemSlotsParents.GetComponentsInChildren<UI_ItemSlot>();
        uI_StashSlots = stashSlotsParents.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void iniSlots()
    {
        GameObject newItemSlot = new GameObject();
        /* 添加一定数量的背包Slots */
        for (int i = 0; i < slotsCount; i++)
        {
            newItemSlot = GameObject.Instantiate(itemSlotPrefab);
            newItemSlot.transform.parent = itemSlotsParents.transform;
            newItemSlot = GameObject.Instantiate(itemSlotPrefab);
            newItemSlot.transform.parent = stashSlotsParents.transform;
        }
    }

    /* 背包添加物品 */
    public void AddItem(ItemData _item)
    {
        if (_item != null)
        {
            if (_item.category == Category.Equipment)
            {
                AddInventoryItem(_item);
            }
            else if (_item.category == Category.Material)
            {
                AddStashItem(_item);
            }
        }
    }
    /* 添加材料  AddItem*/
    private void AddStashItem(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItems.Add(newItem);
            stashDictionary.Add(_item, newItem);
            newItem.AddStack();
        }
        UpdateStashItemSlots();
    }
    /* 添加装备  AddItem*/
    private void AddInventoryItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
            newItem.AddStack();
        }
        UpdateInventoryItemSlots();
    }
    /* 背包移除物品  */
    public void RemoveItem(ItemData _item)
    {
        if (_item != null)
        {
            if (_item.category == Category.Equipment)
            {
                RemoveInventoryItem(_item);
            }
            else if (_item.category == Category.Material)
            {
                RemoveStashItem(_item);
            }
        }
    }
    /* 装备背包移除装备 RemoveItem*/
    public void RemoveInventoryItem(ItemData _item)
    {
        
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize > 1)
                value.RemoveStack();
            else
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
        }
        UpdateInventoryItemSlots();
    }
    /* 材料背包移除材料 RemoveItem*/
    public void RemoveStashItem(ItemData _item)
    {

        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize > 1)
                value.RemoveStack();
            else
            {
                stashItems.Remove(value);
                stashDictionary.Remove(_item);
            }
        }
        UpdateStashItemSlots();
    }

    /* 装备物品 */
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment equipingItem = _item as ItemData_Equipment;
        
        /*  */
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (equipmentItems.Count > 0)
            {
                Debug.Log("EquipItem-Get>0");
                InventoryItem tempItem = null;
                /* 遍历已装备栏 */
                foreach (InventoryItem item in equipmentItems)
                {
                    /* 若已装备将要装备的装备类型，则进行替换 */
                    ItemData_Equipment equipedItem = item.itemData as ItemData_Equipment;
                    if (equipedItem.equipmentType == equipingItem.equipmentType)
                    {
                        // 记录下来在外面替换
                        tempItem = item;
                    }
                }/* 若有需要进行替换的装备 */
                if (tempItem != null)
                {
                    /* 装备栏中移除该装备 并在背包中添加该装备*/
                    equipmentItems.Remove(tempItem);
                    AddInventoryItem(tempItem.itemData);
                }
                
            }
            /* 装备栏添加装备 */
            equipmentItems.Add(value);
            /* 装备背包移除装备 */
            RemoveInventoryItem(value.itemData);
        }
        /* 更新装备栏信息 */
        UpdateEquipmentItemSlots();
    }

    /* 添加至装备栏并应用修改器 */
    public void AddEquipItem(ItemData _item)
    {
        
    }
    /* 移除装备并应用修改器 */

    /* 更新装备背包 */
    public void UpdateInventoryItemSlots() {
        if (inventoryItems.Count != 0 && uI_InventorySlots.Length != 0) {
            for (int i = 0; i < uI_InventorySlots.Length; i++)
            {
                if (i < inventoryItems.Count)
                    uI_InventorySlots[i].UpdateItemSlot(inventoryItems[i]);
                else
                    uI_InventorySlots[i].UpdateItemSlot(null);
            }
        }
    }

    /* 更新物品仓库 */
    public void UpdateStashItemSlots()
    {
        uI_StashSlots = stashSlotsParents.GetComponentsInChildren<UI_ItemSlot>();

        for (int i = 0; i < stashItems.Count; i++)
        {
            uI_StashSlots[i].UpdateItemSlot(stashItems[i]);
        }
    }

    /* 更新装备栏 */
    public void UpdateEquipmentItemSlots()
    {
        Debug.Log("UpdateEquipmentItemSlots");
        foreach (InventoryItem inventoryItem in equipmentItems)
        {
            ItemData_Equipment itemData_Equipment = inventoryItem.itemData as ItemData_Equipment;
            foreach (UI_EquipmentItemSlot slot in uI_EquipSlots)
            {
                /* 对号入座 */
                if (slot.equipmentType == itemData_Equipment.equipmentType)
                {
                    Debug.Log("equipmentType right");
                    slot.itemData = itemData_Equipment;
                    slot.UpdateItemSlot(inventoryItem);
                }
                else
                {
                    Debug.Log("equipmentType false");
                    slot.itemData = null;
                }
            }
        }
    }
    

}
