using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*  物品对象挂载脚本 */
public class ItemObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] ItemData itemData;

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        if (itemData == null)
        {
            sr.sprite = null;
            gameObject.name = "Null Item Object";
        } else
        {
            sr.sprite = itemData.icon;
            gameObject.name = "Item Object" + itemData.name;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        //sr.sprite = itemData.icon;
        //gameObject.name = "Item Object" + itemData.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpItem(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        
    }
}
