using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]Slider slider;
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (1 << ((int)slider.value)).ToString();
    }
}
