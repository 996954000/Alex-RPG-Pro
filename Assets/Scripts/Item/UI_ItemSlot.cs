using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public Image itemIcon;
    public TextMeshProUGUI itemCount;

    public ItemData itemData;

    private Color alphaForOne;
    private Color alphaForZero;
    private void Awake()
    {
        itemIcon = GetComponentInChildren<Image>();
        itemCount = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        alphaForOne = itemIcon.color;
        alphaForOne.a = 1;
        alphaForZero = itemIcon.color;
        alphaForZero.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* 更新当前插槽物品信息 */
    public void UpdateItemSlot(InventoryItem _newInventoryItem)
    {
        if (_newInventoryItem == null)
        {
            itemIcon.sprite = null;
            itemCount.text = "";
            itemIcon.color = alphaForZero;
            itemData = null;
        }
        else
        {
            itemIcon.sprite = _newInventoryItem.itemData.icon;
            itemCount.text = _newInventoryItem.stackSize.ToString();

            itemData = _newInventoryItem.itemData;
            /* 将透明度设置为1 */
            itemIcon.color = alphaForOne;
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /* 当当前Slot不为空时才会触发点击事件 */
        if (itemData != null)
        {
            if (itemData.category == Category.Equipment)
            {
                Inventory.instance.EquipItem(itemData);
            }
        }
    }
}
