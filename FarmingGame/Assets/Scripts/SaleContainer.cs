using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class SaleContainer : ItemContainer, IPointerUpHandler
{
    [SerializeField]
    Button sellBtn;
    [SerializeField]
    MarketPanel market;

    protected override void OnEnable()
    {
        base.OnEnable();

        sellBtn.onClick.AddListener(delegate { SellAll(); });
    }

    void SellAll()
    {
        var itemsCopy = new List<Item>(items);

        foreach (var item in itemsCopy)
        {
            if ((item.itemType & ItemType.Tractor) == ItemType.Tractor)
            {
                item.Transit(market.EqCont);
            }
            else if ((item.itemType & ItemType.Crop) == ItemType.Crop)
            {
                item.Transit(market.CropsCont);
            }
            else if ((item.itemType & ItemType.Yield) == ItemType.Yield)
            {
                item.Transit(market.YieldsCont);
            }

            Now.Farm.capital += item.price;
            item.IsPlayer = false;
        }
    }
}
