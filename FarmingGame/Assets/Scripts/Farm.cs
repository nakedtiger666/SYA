using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm
{
    public List<Field> fields;
    public int capital;

    public Farm()
    {
        fields = new List<Field>();
        capital = 1000;
    }
}
