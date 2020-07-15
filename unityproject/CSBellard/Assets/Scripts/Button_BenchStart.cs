using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Button_BenchStart : MonoBehaviour
{
    int mode = 0;//0はこれから解析するところ,1は解析中でキャンセル待ち
    Text cancel_ui_text;
    BackImg backImg;
    Hoge0 hoge;

    private void Start()
    {
        //cancel_ui_text = GameObject.Find("TapToCancel_Text").GetComponent<Text>();
        //cancel_ui_text.text = " ";
        backImg = GameObject.Find("backimg").GetComponent<BackImg>();
        hoge = GetComponent<Hoge0>();
        
    }
    public void OnClick()
    {
        mode = 1 - mode;
        //これから解析するとき
        if (mode == 1)
        {
            //cancel_ui_text.text = "Tap to cancel";
            backImg.SetNowbench();
            //string digits = GameObject.Find("TextDigits").GetComponent<Text>().text;
            //GetComponent<Hoge0>().ButtonPush(ulong.Parse(digits));
            int digits_ = GameObject.Find("Dropdown_digit").GetComponent<Dropdown>().value;
            int groupsize_ = GameObject.Find("Dropdown_GroupSize").GetComponent<Dropdown>().value;
            int iterpf_ = GameObject.Find("Dropdown_Ryuudo").GetComponent<Dropdown>().value;

            ulong digits = 0;
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

            hoge.gridn = 1 << (groupsize_ + 2);
            hoge.iterationsPerFrame = 1UL << (iterpf_);
            hoge.ButtonPush(((digits - 1) * 2 - 3) / 5);
            //GameObject.Find("Button_BenchStart").GetComponent<Button>().interactable = false;
        }
        else
        //解析を中断するとき
        {
            Debug.Log("Canseled");
            hoge.EndStep7();//ここからSetinteract()へジャンプする
        }
    }


    //自動で解析が終了したとき
    public void Setinteract() {
        //GameObject.Find("Button_BenchStart").GetComponent<Button>().interactable = true;
        mode = 0;
        //cancel_ui_text.text = " ";
        backImg.SetWaitbench();
    }

}