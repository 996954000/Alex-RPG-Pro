using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*  物品对象挂载脚本 */
public class ItemObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] ItemData itemData;

    // Start is called before the first frame update

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.icon;
        gameObject.name = "Item Object" + itemData.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
