using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class PointerController : MonoBehaviour
{
    static PointerController instance;
    public static PointerController Instance => instance;

    public ItemContainer Limbo;
    RectTransform limboRect;
    public Vector3 onItemDeltaClick;

    private void OnEnable()
    {
        instance = this;
        limboRect = Limbo.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        UnselectField();
        MoveLimbo();
    }

    void UnselectField()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition)))
        {
            Now.SelectedField = null;
        }
    }

    void MoveLimbo()
    {
        limboRect.position = onItemDeltaClick + Input.mousePosition;
    }
}
