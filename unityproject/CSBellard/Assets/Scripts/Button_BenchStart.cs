using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Button_BenchStart : MonoBehaviour
{
    int mode = 0;//0はこれから解析するところ,1は解析中でキャンセル待ち
    [SerializeField]
    Text cancel_ui_text;
    [SerializeField]
    BackImg backImg;
    [SerializeField]
    Hoge0 hoge;
    [SerializeField]
    UnderButtonManager underButtonManager;
    [SerializeField]
    Slider slider0;
    [SerializeField]
    Slider slider1;

    private void Start()
    {
        cancel_ui_text.text = " ";
    }
    public void OnClick()
    {
        mode = 1 - mode;
        //これから解析するとき
        if (mode == 1)
        {
            cancel_ui_text.text = "Tap to cancel";
            backImg.SetNowbench();
            underButtonManager.button0.interactable = false;
            underButtonManager.button1.interactable = false;
            underButtonManager.button2.interactable = false;
            slider0.interactable = false;
            slider1.interactable = false;

            //string digits = GameObject.Find("TextDigits").GetComponent<Text>().text;
            //GetComponent<Hoge0>().ButtonPush(ulong.Parse(digits));
            int digits_ = GameObject.Find("Dropdown_digit").GetComponent<Dropdown>().value;
            int groupsize_ = (int)GameObject.Find("GroupSizeSlider").GetComponent<Slider>().value;
            int threadnum_ = (int)GameObject.Find("ThreadSlider").GetComponent<Slider>().value;

            hoge.ranker_flag = 0;//ランキングに記録するかどうか
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
                    hoge.ranker_flag = 1;
                    digits = 100000000;
                    break;
                case 3:
                    digits = 100000000;
                    break;
                case 4:
                    digits = 500000000;
                    break;
                case 5:
                    digits = 1000000000;
                    break;
                case 6:
                    digits = 1500000000;
                    break;
                case 7:
                    hoge.ranker_flag = 20;
                    digits = 2000000000;
                    break;
                case 8:
                    digits = 2000000000;
                    break;
                case 9:
                    digits = 10000000000;
                    break;
                case 10:
                    digits = 20000000000;
                    break;
                case 11:
                    digits = 32000000000;
                    break;
                case 12:
                    digits = 50000000000;
                    break;
                case 13:
                    digits = 100000000000;
                    break;
                case 14:
                    hoge.ranker_flag = 2500 * 20;
                    digits = 250000000000;
                    break;
                case 15:
                    digits = 250000000000;
                    break;
                default:
                    digits = 250000000000;
                    break;
            }

            hoge.gridn = 1 << (groupsize_);
            hoge.blockn = 1 << (threadnum_);
            hoge.ButtonPush(((digits - 1) * 2 - 3) / 5);
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
        mode = 0;
        cancel_ui_text.text = " ";
        backImg.SetWaitbench();

        underButtonManager.button0.interactable = false;
        underButtonManager.button1.interactable = true;
        underButtonManager.button2.interactable = true;
        slider0.interactable = true;
        slider1.interactable = true;
    }
}