using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[Flags]
public enum ItemType
{
    None = 0,
    Tractor = 1 << 0,
    Crop = 1 << 1,
    Yield = 1 << 2,
    Chemicals = 1 << 3,
    Ferts = 1 << 4,
}

//TODO: make it as entry not MonoBehaviour
public class Item : MonoBehaviour, IPointerDownHandler
{
    public ItemType itemType;
    public int amount;
    public bool access;
    public Image icon;
    LayoutElement layoutElement;
    public bool IsPlayer;

    public int price;

    public ItemContainer container;
    public Field field;
    public bool isIsntantiated;

    private void Start()
    {
        if (container == null || (container.allowedType & itemType) != itemType)
        {
            Destroy(gameObject);
        }

        icon = GetComponent<Image>();
        layoutElement = GetComponent<LayoutElement>();

        isIsntantiated = true;

        price = 500;

        if (itemType == ItemType.Yield)
        {
            price = 1000;
        }

        if (itemType == ItemType.Yield)
        {
            Hide();
        }
    }

    public void Hide()
    {
        if (icon)
        {
            icon.enabled = false;
        }
        if (layoutElement)
        {
            layoutElement.ignoreLayout = true;
        }
    }

    public void Unhide()
    {
        if (icon)
        {
            icon.enabled = true;
        }
        if (layoutElement)
        {
            layoutElement.ignoreLayout = false;
        }
    }

    public void ForceReposition()
    {
       
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!access || PointerController.Instance.Limbo.items.Count > 0)
        {
            return;
        }

        if (Transit(PointerController.Instance.Limbo))
        {
            icon.raycastTarget = false;
            PointerController.Instance.onItemDeltaClick = Input.mousePosition - transform.position;
        }
    }

    public bool Transit(ItemContainer to)
    {
        if (!to.canBePlaced || !container.items.Contains(this) || to.items.Contains(this) || (to.allowedType & itemType) != itemType)
        {
            return false;
        }

        transform.SetParent(to.transform);
        transform.localPosition = Vector3.zero;
        container.items.Remove(this);

        if (container.isFieldPanel)
        {
            Now.SelectedField.items.Remove(this);
            field = null;
        }

        if (to.isFieldPanel)
        {
            Now.SelectedField.items.Add(this);
            field = Now.SelectedField;
        }

        container = to;
        to.items.Add(this);

        return true;
    }

    public void Instantiate(ItemContainer to)
    {
        if (isIsntantiated)
        {
            return;
        }


    }

    private void OnDisable()
    {

    }
}
