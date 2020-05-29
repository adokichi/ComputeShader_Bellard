using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Button_BenchStart : MonoBehaviour
{
    public void OnClick()
    {

        //HttpService h=GameObject.Find("HttpService").GetComponent<HttpService>();
        //StartCoroutine(HttpService.Post());
        
        string digits = GameObject.Find("TextDigits").GetComponent<Text>().text;
        GetComponent<Hoge0>().ButtonPush(ulong.Parse(digits));
        

    }
}