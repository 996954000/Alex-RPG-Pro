using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Category
{
    Equipment,
    Material
}

[CreateAssetMenu(fileName = "new Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Category category;
}
