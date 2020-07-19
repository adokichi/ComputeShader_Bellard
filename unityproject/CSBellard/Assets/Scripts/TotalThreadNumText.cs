using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalThreadNumText : MonoBehaviour
{
    [SerializeField] Slider slider1;
    [SerializeField] Slider slider2;
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (1 << ((int)slider1.value + (int)slider2.value)).ToString();
    }
}

