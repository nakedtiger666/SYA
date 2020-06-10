using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum FieldStance
{
    None,
    NotReady,
    Tillaging,
    Ready,
    Planting,
    Growing,
    Grown,
    Harvesting,
    Sold,
}

public class Field : MonoBehaviour
{
    public FieldStance stance;
    public ItemContainer yieldCont;
    FieldStance setStance;
    MeshRenderer meshRenderer;
    bool isSelected;
    bool isSelectedSet;
    Vector3 localPos;
    float onSelectedTime;

    public float processDone;
    public int speed => items.Where(i => i.itemType == ItemType.Tractor).Count();

    public List<Item> items = new List<Item>();

    public int price
    {
        get; private set;
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        setStance = FieldStance.None;
        stance = FieldStance.Sold;

        localPos = transform.localPosition;

        price = Mathf.RoundToInt(transform.localScale.magnitude) * 100;
    }

    void Update()
    {
        UpdateStance();
        UpdateSelectability();
        AnimateSelectability();
        ProceedStance();
    }

    void ProceedStance()
    {
        if (processDone >= 1)
        {
            processDone = 0;

            if (stance == FieldStance.Tillaging)
            {
                stance = FieldStance.Ready;
            }
            else if (stance == FieldStance.Planting)
            {
                stance = FieldStance.Growing;
            }
            else if (stance == FieldStance.Growing)
            {
                stance = FieldStance.Grown;
            }
            else if (stance == FieldStance.Harvesting)
            {
                stance = FieldStance.NotReady;
                EmitYield();
            }
        }
    }

    void EmitYield()
    {
        Item yield = GameObject.Instantiate(References.Instance.yield);
        yield.field = this;
        items.Add(yield);

        yield. transform.SetParent(yieldCont.transform);
        yield. transform.localPosition = Vector3.zero;
        yield.container = yieldCont;
        yieldCont.items.Add(yield);
    }

    void UpdateStance()
    {
        if (setStance != stance)
        {
            setStance = stance;

            if (setStance == FieldStance.NotReady || setStance == FieldStance.Tillaging)
            {
                meshRenderer.material = References.Instance.notReady;
            }
            else if (setStance == FieldStance.Ready)
            {
                meshRenderer.material = References.Instance.ready;
            }
            else if (setStance == FieldStance.Planting || setStance == FieldStance.Growing)
            {
                meshRenderer.material = References.Instance.planted;
            }
            else if (setStance == FieldStance.Grown || setStance == FieldStance.Harvesting)
            {
                meshRenderer.material = References.Instance.grown;
            }
            else if (setStance == FieldStance.Sold)
            {
                meshRenderer.material = References.Instance.sold;
            }
            else
            {
                meshRenderer.material = References.Instance.none;
            }
        }
    }

    void UpdateSelectability()
    {
        if (Now.SelectedField != this)
        {
            isSelected = false;
        }
        else
        {
            isSelected = true;
        }

        if (isSelectedSet != isSelected)
        {
            isSelectedSet = isSelected;
            onSelectedTime = Time.timeSinceLevelLoad;
        }
    }

    void AnimateSelectability()
    {
        if (isSelectedSet)
        {
            gameObject.transform.localPosition = localPos - Mathf.Sin(Time.timeSinceLevelLoad - onSelectedTime) * Vector3.forward * .2f - Vector3.forward * .6f;
        }
        else
        {
            gameObject.transform.localPosition = localPos;
        }
    }

    private void OnMouseDown()
    {
        Now.SelectedField = this;
    }
}
