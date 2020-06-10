using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class BuyContainer : ItemContainer, IPointerUpHandler
{
    [SerializeField]
    Button buyBtn;
    [SerializeField]
    AssetsPanel assets;

    protected override void OnEnable()
    {
        base.OnEnable();

        buyBtn.onClick.AddListener(delegate { BuyAll(); });
    }

    void BuyAll()
    {
        var itemsCopy = new List<Item>(items);

        foreach (var item in itemsCopy)
        {
            if (item.itemType == ItemType.Tractor)
            {
                item.Transit(assets.EqCont);
            }
            else if (item.itemType == ItemType.Crop)
            {
                item.Transit(assets.CropsCont);
            }
            else if (item.itemType == ItemType.Yield)
            {
                item.Transit(assets.YieldsCont);
            }

            Now.Farm.capital -= item.price;
            item.IsPlayer = true;
        }
    }
}
