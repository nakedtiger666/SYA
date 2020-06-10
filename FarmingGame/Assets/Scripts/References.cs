using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class References : MonoBehaviour
{
    static References instance;
    public static References Instance => instance;

    private void OnEnable()
    {
        instance = this;
    }

    public Material notReady;
    public Material ready;
    public Material planted;
    public Material grown;
    public Material sold;
    public Material none;

    public Item yield;
}
