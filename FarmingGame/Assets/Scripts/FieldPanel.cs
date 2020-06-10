using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FieldPanel : MonoBehaviour
{
    Field currentField;
    FieldStance currentFieldStance;
    bool isFirstTime;

    [SerializeField]
    GameObject panelGo;
    [SerializeField]
    GameObject btnsGo;
    [SerializeField]
    Button buyBtn;
    [SerializeField]
    Button rentBtn;
    [SerializeField]
    Button sellBtn;
    [SerializeField]
    Button tillageBtn;
    [SerializeField]
    Button plantBtn;
    [SerializeField]
    Button harvestBtn;
    [SerializeField]
    Button applyBtn;
    [SerializeField]
    Button scoutBtn;

    [SerializeField]
    ItemContainer eqCont;
    [SerializeField]
    ItemContainer inputsCont;
    [SerializeField]
    ItemContainer cropCont;
    [SerializeField]
    ItemContainer yieldCont;

    [SerializeField]
    AssetsPanel assets;

    [SerializeField]
    Image growingLine;
    [SerializeField]
    Image growingLine1;
    [SerializeField]
    Image growingLine2;

    [SerializeField]
    Image tillageLine;
    [SerializeField]
    Image plantingLine;
    [SerializeField]
    Image harvestLine;

    void Start()
    {
        currentField = null;
        currentFieldStance = FieldStance.None;
        isFirstTime = true;

        buyBtn.onClick.AddListener(delegate { Buy(); });
        sellBtn.onClick.AddListener(delegate { Sell(); });
        tillageBtn.onClick.AddListener(delegate { Tillage(); });
        plantBtn.onClick.AddListener(delegate { Plant(); });
        harvestBtn.onClick.AddListener(delegate { Harvest(); });
    }

    void Update()
    {
        UpdateSelectedField();
        UpdateButtonInteractability();
        UpdatePerFieldContainers();
    }

    void UpdatePerFieldContainers()
    {
        foreach (var item in eqCont.items)
        {
            if (item.field == Now.SelectedField)
            {
                item.Unhide();
            }
            else
            {
                item.Hide();
            }
        }

        foreach (var item in inputsCont.items)
        {
            if (item.field == Now.SelectedField)
            {
                item.Unhide();
            }
            else
            {
                item.Hide();
            }
        }
        foreach (var item in cropCont.items)
        {
            if (item.field == Now.SelectedField)
            {
                item.Unhide();
            }
            else
            {
                item.Hide();
            }
        }
        foreach (var item in yieldCont.items)
        {
            if (item.field == Now.SelectedField)
            {
                item.Unhide();
            }
            else
            {
                item.Hide();
            }
        }
    }

    void UpdateButtonInteractability()
    {
        if (currentField == null)
        {
            return;
        }

        buyBtn.interactable = currentField.stance == FieldStance.Sold;
        sellBtn.interactable = currentField.stance != FieldStance.Sold;
        rentBtn.interactable = false;

        if (currentField.stance == FieldStance.Growing)
        {
            growingLine.fillAmount = currentField.processDone;
            growingLine.enabled = true;
            growingLine1.enabled = false;
            growingLine2.enabled = false;
        }
        else
        {
            if (currentField.stance == FieldStance.Grown)
            {
                growingLine.fillAmount = 1;
                growingLine.enabled = true;
                growingLine1.enabled = true;
                growingLine2.enabled = true;
            }
            else
            {
                growingLine.enabled = false;
                growingLine1.enabled = false;
                growingLine2.enabled = false;
            }
        }

        if (currentField.stance == FieldStance.Tillaging)
        {
            tillageLine.enabled = true;
            tillageLine.fillAmount = currentField.processDone;
        }
        else
        {
            tillageLine.enabled = false;
        }

        if (currentField.stance == FieldStance.Planting)
        {
            plantingLine.enabled = true;
            plantingLine.fillAmount = currentField.processDone;
        }
        else
        {
            plantingLine.enabled = false;
        }

        if (currentField.stance == FieldStance.Harvesting)
        {
            harvestLine.enabled = true;
            harvestLine.fillAmount = currentField.processDone;
        }
        else
        {
            harvestLine.enabled = false;
        }

        plantBtn.interactable = currentField.stance == FieldStance.Ready && cropCont.items.Where(i => i.field == currentField).Count() > 0/*HACK*/;
        harvestBtn.interactable = currentField.stance == FieldStance.Grown;
        tillageBtn.interactable = currentField.stance == FieldStance.NotReady;
        applyBtn.interactable = false;
        scoutBtn.interactable = false;
    }

    void UpdateSelectedField()
    {
        if (isFirstTime || currentField != Now.SelectedField || (currentField != null && currentFieldStance != currentField.stance))
        {
            isFirstTime = false;
            currentField = Now.SelectedField;

            if (currentField == null)
            {
                panelGo.SetActive(false);
                btnsGo.SetActive(false);
            }
            else
            {
                btnsGo.SetActive(true);
                currentFieldStance = currentField.stance;

                if (currentFieldStance == FieldStance.Sold)
                {
                    buyBtn.gameObject.SetActive(true);
                    rentBtn.gameObject.SetActive(true);
                    sellBtn.gameObject.SetActive(false);
                    panelGo.SetActive(false);
                }
                else
                {
                    buyBtn.gameObject.SetActive(false);
                    rentBtn.gameObject.SetActive(false);
                    sellBtn.gameObject.SetActive(true);
                    panelGo.SetActive(true);
                }
            }
        }
    }

    void Buy()
    {
        if (!Now.Farm.fields.Contains(currentField))
        {
            Now.Farm.fields.Add(currentField);
            Now.Farm.capital -= currentField.price;
            currentField.stance = FieldStance.NotReady;
        }
    }

    void Sell()
    {
        if (Now.Farm.fields.Contains(currentField))
        {
            Now.Farm.fields.Remove(currentField);
            Now.Farm.capital += currentField.price;
            currentField.stance = FieldStance.Sold;
        }
    }

    void Tillage()
    {
        if (currentField.stance == FieldStance.NotReady)
        {
            currentField.stance = FieldStance.Tillaging;
            var itemCopy = new List<Item>(yieldCont.items);

            foreach (var item in itemCopy.Where(i => i.field == currentField))
            {
                item.Transit(assets.YieldsCont);
            }
        }
    }

    void Plant()
    {
        if (currentField.stance == FieldStance.Ready && cropCont.items.Where(i => i.field == currentField).Count() > 0/*HACK*/)
        {
            currentField.stance = FieldStance.Planting;
            var itemsCopy = new List<Item>(cropCont.items);

            cropCont.items.Remove(cropCont.items.Where(i => i.field == currentField).FirstOrDefault());

            foreach (var item in itemsCopy.Where(i => i.field == currentField))
            {
                GameObject.Destroy(item.gameObject);
            }
        }
    }

    void Harvest()
    {
        if (currentField.stance == FieldStance.Grown)
        {
            currentField.stance = FieldStance.Harvesting;
        }
    }
}
