using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ToggleController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ons = new List<GameObject>();
    [SerializeField]
    List<GameObject> offs = new List<GameObject>();

    Toggle toggle;

    private void OnEnable()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { OnToggleSwitch(toggle.isOn); });
        OnToggleSwitch(toggle.isOn);
    }

    void OnToggleSwitch(bool val)
    {
        foreach (var item in ons)
        {
            item.SetActive(val);
        }

        foreach (var item in offs)
        {
            item.SetActive(!val);
        }
    }
}
