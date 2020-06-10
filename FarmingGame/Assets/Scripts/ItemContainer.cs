using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class ItemContainer : MonoBehaviour, IPointerUpHandler
{
    public List<Item> items = new List<Item>();
    public ItemType allowedType;
    public bool canBePlaced = true;
    public bool IsPlayerInit;
    public bool isFieldPanel;

    protected virtual void OnEnable()
    {
        items = GetComponentsInChildren<Item>().ToList();

        foreach (var item in items)
        {
            item.container = this;
            item.IsPlayer = IsPlayerInit;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PointerController.Instance.Limbo.items.Count > 0 && (PointerController.Instance.Limbo.items.All(i => i.IsPlayer == IsPlayerInit)))
        {
            var items = new List<Item>(PointerController.Instance.Limbo.items);

            foreach (var item in items)
            {
                if (item.Transit(this))
                {
                    item.icon.raycastTarget = true;
                }
            }
        }
    }
}
