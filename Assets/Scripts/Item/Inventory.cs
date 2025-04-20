
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

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
    public void AddItem(ItemData _item, int num = 1)
    {
        if (_item != null)
        {
            if (_item.category == Category.Equipment)
            {
                AddInventoryItem(_item, num);
            }
            else if (_item.category == Category.Material)
            {
                AddStashItem(_item, num);
            }
        }
    }
    /* 添加材料  AddItem*/
    private void AddStashItem(ItemData _item, int num = 1)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStackByNum(num);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItems.Add(newItem);
            stashDictionary.Add(_item, newItem);
            newItem.AddStackByNum(num);
        }
        UpdateStashItemSlots();
    }
    /* 添加装备  AddItem*/
    private void AddInventoryItem(ItemData _item, int num = 1)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStackByNum(num);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
            newItem.AddStackByNum(num);
        }
        UpdateInventoryItemSlots();
    }
    /* 背包移除物品  */
    public void RemoveItem(ItemData _item, int num = 1)
    {
        if (_item != null)
        {
            if (_item.category == Category.Equipment)
            {
                RemoveInventoryItem(_item, num);
            }
            else if (_item.category == Category.Material)
            {
                RemoveStashItem(_item, num);
            }
        }
    }
    /* 装备背包移除装备 RemoveItem*/
    public void RemoveInventoryItem(ItemData _item, int num = 1)
    {
        
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize > num)
                value.RemoveStackByNum(num);
            else
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
        }
        UpdateInventoryItemSlots();
    }
    /* 材料背包移除材料 RemoveItem*/
    public void RemoveStashItem(ItemData _item, int num = 1)
    {

        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize > num)
                value.RemoveStackByNum(num);
            else
            {
                stashItems.Remove(value);
                stashDictionary.Remove(_item);
            }
        }
        UpdateStashItemSlots();
    }

    /* 装备物品 */
    public void EquipItem(ItemData _item, int num = 1)
    {
        ItemData_Equipment equipingItem = _item as ItemData_Equipment;
        
        /*  */
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (equipmentItems.Count > 0)
            {
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
                    /* 装备栏中移除该装备 并在背包中添加该装备，Remove函数中已经自动加了，其实感觉叫收回装备更好
                     UnEquipItem*/
                    UnEquipItem((ItemData_Equipment)tempItem.itemData, num);
                }

            }
            /* 装备栏添加装备 */
            EquipItem((ItemData_Equipment)value.itemData, num);
            /* 装备背包移除装备 */
            RemoveInventoryItem(value.itemData, num);
        }
        
    }

    /* 添加至装备栏并应用修改器 */
    public void EquipItem(ItemData_Equipment _item, int num = 1)
    {
        /* 装备我想不进行堆叠，后面再写吧*/
        InventoryItem inventoryItem = new InventoryItem(_item);
        inventoryItem.AddStackByNum(num);
        equipmentDictionary.Add(_item, inventoryItem);
        equipmentItems.Add(inventoryItem);
        
        /* 应用修改器 */
        if (_item != null)
        {
            _item.AddModifier();
        }

        /* 更新装备栏信息 */
        UpdateEquipmentItemSlots();
    }
    /* 移除装备并应用修改器 */
    public void UnEquipItem(ItemData_Equipment _item, int num = 1)
    {
        if (equipmentDictionary.TryGetValue(_item, out InventoryItem inventoryItem))
        {
            inventoryItem.RemoveStackByNum(num);
            equipmentDictionary.Remove(_item);
            equipmentItems.Remove(inventoryItem);
        }

        /* 应用修改器 */
        if (_item != null)
        {
            _item.RemoveModifier();
        }

        /* 装备返回背包 */
        AddInventoryItem(_item);

        /* 更新装备栏信息 */
        UpdateEquipmentItemSlots();
    }

    /* 更新装备背包 */
    public void UpdateInventoryItemSlots() {
        for (int i = 0; i < uI_InventorySlots.Length; i++)
        {
            if (i < inventoryItems.Count)
                uI_InventorySlots[i].UpdateItemSlot(inventoryItems[i]);
            else
                uI_InventorySlots[i].UpdateItemSlot(null);
        }
    }

    /* 更新物品仓库 */
    public void UpdateStashItemSlots()
    {
        for (int i = 0; i < uI_StashSlots.Length; i++)
        {
            if (i < stashItems.Count)
                uI_StashSlots[i].UpdateItemSlot(stashItems[i]);
            else
                uI_StashSlots[i].UpdateItemSlot(null);
        }
    }

    /* 更新装备栏 */
    public void UpdateEquipmentItemSlots()
    {
        Debug.Log("UpdateEquipmentItemSlots");
        if (equipmentItems.Count > 0)
        {
            foreach (UI_EquipmentItemSlot slot in uI_EquipSlots)
            {
                bool flag = false;
                foreach (InventoryItem inventoryItem in equipmentItems)
                {
                    ItemData_Equipment itemData_Equipment = inventoryItem.itemData as ItemData_Equipment;
                    /* 对号入座 */
                    if (slot.equipmentType == itemData_Equipment.equipmentType)
                    {
                        Debug.Log("equipmentType right");
                        slot.itemData = itemData_Equipment;
                        slot.UpdateItemSlot(inventoryItem);
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    slot.itemData = null;
                    slot.UpdateItemSlot(null);
                }
            }
        }
        else
        {
            foreach (UI_EquipmentItemSlot slot in uI_EquipSlots)
            {
                slot.itemData = null;
                slot.UpdateItemSlot(null);
            }
        }
    }
    
    /* 工艺材料判断是否充足，这里的num就是批量制作的标志了吧
     * 先关注做单个的*/
    public bool CanCraft(ItemData _item, int count = 1)
    {
        bool flag = true;
        foreach(CraftMaterial craftMaterial in _item.craftMaterials)
        {
            if (stashDictionary.TryGetValue(craftMaterial.itemData, out InventoryItem inventoryItem))
            {
                Debug.Log("CanCraft getValue");
                if (inventoryItem.stackSize < craftMaterial.num * count)
                    flag = false;
            }else
            {
                flag = false;
            }
        }
        return flag;
    }

    /* 进行制作，并扣除材料，将物品置入背包 */
    public void CraftByMaterial(ItemData _item, int count = 1)
    {
        foreach (CraftMaterial craftMaterial in _item.craftMaterials)
        {
            RemoveStashItem(craftMaterial.itemData, craftMaterial.num * count);
        }
        AddInventoryItem(_item, count);
    }
}

