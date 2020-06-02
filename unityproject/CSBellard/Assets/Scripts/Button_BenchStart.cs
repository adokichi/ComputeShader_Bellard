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

        //string digits = GameObject.Find("TextDigits").GetComponent<Text>().text;
        //GetComponent<Hoge0>().ButtonPush(ulong.Parse(digits));

        int digits_ = GameObject.Find("Dropdown_digit").GetComponent<Dropdown>().value;
        int groupsize_ = GameObject.Find("Dropdown_GroupSize").GetComponent<Dropdown>().value;
        int iterpf_ = GameObject.Find("Dropdown_Ryuudo").GetComponent<Dropdown>().value;

        ulong digits=0;
        switch (digits_)
        {
            case 0:
                digits = 1000000;
                break;
            case 1:
                digits = 10000000;
                break;
            case 2:
                digits = 100000000;
                break;
            case 3:
                digits = 500000000;
                break;
            case 4:
                digits = 1000000000;
                break;
            case 5:
                digits = 1500000000;
                break;
            case 6:
                digits = 2000000000;
                break;
            case 7:
                digits = 10000000000;
                break;
            case 8:
                digits = 20000000000;
                break;
            case 9:
                digits = 32000000000;
                break;
            case 10:
                digits = 50000000000;
                break;
            case 11:
                digits = 100000000000;
                break;
            default:
                digits = 250000000000;
                break;
        }

        Hoge0 hoge = GetComponent<Hoge0>();
        hoge.gridn = 1 << (groupsize_ + 2);
        hoge.iterationsPerFrame= 1UL << (iterpf_);
        hoge.ButtonPush(((digits - 1) * 2 - 3) / 5);
    }
}