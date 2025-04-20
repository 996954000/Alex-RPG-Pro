using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/* 装备栏格子 */
public class UI_EquipmentItemSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;
    protected override void Start()
    {
        base.Start();
        itemCount.text = "";
    }

    public void OnValidate()
    {
        name = "EquipmentSlot_" + equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemData !=  null)
        {
            Inventory.instance.UnEquipItem((ItemData_Equipment)itemData);
        }
    }
}
