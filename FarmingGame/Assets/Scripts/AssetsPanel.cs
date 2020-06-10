using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsPanel : MonoBehaviour
{
    public ItemContainer EqCont;
    public ItemContainer CropsCont;
    public ItemContainer YieldsCont;
    public ItemContainer InputsCont;

    bool isFirstTime;
    [SerializeField]
    TMPro.TextMeshProUGUI capitalLabel;


    void Start()
    {
        isFirstTime = true;
    }

    void Update()
    {
        UpdateCapitalLabel();
    }

    void UpdateCapitalLabel()
    {
        capitalLabel.text = "$" + Now.Farm.capital.ToString();
    }
}
