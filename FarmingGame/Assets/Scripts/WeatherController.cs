using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

class WeatherController : MonoBehaviour
{
    public static DateTime todayDate
    {
        get; private set;
    }

    [SerializeField]
    TMPro.TextMeshProUGUI temperatureLabel;
    [SerializeField]
    Button nextDateButton;


    private void Start()
    {
        todayDate = new DateTime(2020, 1, 1);
        UpdateLabels();

        nextDateButton.onClick.AddListener(delegate { ForwardTime(); });
    }

    void ForwardTime()
    {
        todayDate = todayDate.AddDays(1);
        //int days = todayDate.Day;

        //days += 7;

        //if (days > DateTime.DaysInMonth(todayDate.Year, todayDate.Month))
        //{
        //    todayDate = todayDate.AddMonths(1);
        //    todayDate = new DateTime(todayDate.Year, todayDate.Month, 1);
        //}
        //else
        //{
        //    todayDate = todayDate.AddDays(7);
        //}

        UpdateLabels();

        foreach (var item in Now.Farm.fields)
        {
            if (item.stance == FieldStance.Growing)
            {
                item.processDone += .02f;
            }
            else if (item.stance != FieldStance.NotReady && item.stance != FieldStance.Grown && item.stance != FieldStance.Ready)
            {
                item.processDone += .3f * item.speed/item.gameObject.transform.lossyScale.magnitude;
            }
        }
    }

    void UpdateLabels()
    {
        temperatureLabel.text = todayDate.ToString("dd MMMM yyyy") + ", " + GetTemperature(todayDate).ToString() + "°C" + ", " + GetHumidity(todayDate).ToString() + "%";
        //humidityLabel.text = GetHumidity(todayDate).ToString() + "%";
        //dateLabel.text = todayDate.ToString("dd MMMM yyyy");
    }

    int GetHumidity(DateTime date)
    {
        return Mathf.RoundToInt(Mathf.Sin(todayDate.DayOfYear * 1f / 365 * Mathf.PI) * 40) + 30 + Mathf.RoundToInt(UnityEngine.Random.value * 15);
    }

    int GetTemperature(DateTime date)
    {

        return Mathf.RoundToInt(Mathf.Sin(todayDate.DayOfYear * 1f / 365 * Mathf.PI) * 28) + Mathf.RoundToInt(UnityEngine.Random.value * 3);
    }
}

