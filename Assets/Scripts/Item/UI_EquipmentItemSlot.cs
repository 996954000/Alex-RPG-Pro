using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
