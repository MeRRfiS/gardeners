using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropItem : MonoBehaviour
{
    private ItemsModel item;

    public ItemsModel Item
    {
        set { item = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        SoundController.GetInctanse().PlayCollect();

        switch (item.Type)
        {
            case ItemsTypeEnum.Material:
                InventarController.GetInstance().AddNewItemMaterial(item);
                break;
            case ItemsTypeEnum.Bonus:
                switch (item.Index)
                {
                    case ((int)ItemIds.Heal):
                        PlayerController.GetInstance().healthBar.value += 100;
                        break;
                }
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
