using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
public enum Category
{
    Equipment,
    Material
}

/* 为了避免alex循环嵌套的问题，增加了一个配方结构体增加健壮性*/
[Serializable]
public struct CraftMaterial
{
    public ItemData itemData;
    public int num;
}

/* CreateAssetMenu设置新建方式 */
[CreateAssetMenu(fileName = "new Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Category category;
    /* 制作物品需要的清单列表 */
    [SerializeField] public List<CraftMaterial> craftMaterials = new List<CraftMaterial>();
}
