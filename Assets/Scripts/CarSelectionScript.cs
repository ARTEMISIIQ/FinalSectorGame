using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CarSelectionScript : MonoBehaviour
{
    [SerializeField]
    private Slider RedVal;
    [SerializeField]
    private Slider GreenVal;
    [SerializeField]
    private Slider BlueVal;
    [SerializeField]
    private Slider SoftVal;
    [SerializeField]
    private Slider SpanVal;
    [SerializeField]
    private Slider GripVal;

    [SerializeField]
    private Image carColor;

    [SerializeField]
    private TextMeshProUGUI tireNumText;

    [SerializeField]
    private List<float> tireStats;

    [SerializeField]
    private Material carMaterial;

    public int tireNum;
    public int totalTires;

    // Start is called before the first frame update
    void Start()
    {
        tireNum = PlayerPrefs.GetInt("Tire");
        RedVal.value = PlayerPrefs.GetFloat("Red");
        GreenVal.value = PlayerPrefs.GetFloat("Green");
        BlueVal.value = PlayerPrefs.GetFloat("Blue");
    }

    // Update is called once per frame
    void Update()
    {
        tireNumText.text = "Type C" + tireNum.ToString();
        GripVal.value = tireStats[tireNum * 3];
        SoftVal.value = tireStats[tireNum * 3 + 1];
        SpanVal.value = tireStats[tireNum * 3 + 2];

        PlayerPrefs.SetInt("Tire", tireNum);
        PlayerPrefs.SetFloat("Red", RedVal.value);
        PlayerPrefs.SetFloat("Green", GreenVal.value);
        PlayerPrefs.SetFloat("Blue", BlueVal.value);
        carMaterial.color = new Color(RedVal.value, GreenVal.value, BlueVal.value, 255);
        carColor.color = new Color (RedVal.value, GreenVal.value, BlueVal.value, 255);
    }

    public void rightButton()
    {
        tireNum++;
        if (tireNum >= totalTires)
        {
            tireNum = 0;
        }
    }

    public void leftButton()
    {
        tireNum--;
        if (tireNum < 0)
        {
            tireNum = totalTires - 1;
        }
    }
}
