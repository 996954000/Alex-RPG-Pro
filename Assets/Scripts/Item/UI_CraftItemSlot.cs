using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftItemSlot : UI_ItemSlot
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (itemData != null)
            UpdateItemSlot(new InventoryItem(itemData));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemData != null)
        {
            if (Inventory.instance.CanCraft(itemData))
            {
                Debug.Log("CanCraft true");
                Inventory.instance.CraftByMaterial(itemData);
            }
            else
            {
                Debug.Log("CanCraft false");
            }
        }
    }
}
