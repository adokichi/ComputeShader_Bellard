using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FantomLib { 

    public class FPSTextUI : MonoBehaviour
    {
        private Text text;
        int cnt;
        float ftime;

        //Inspector Settings
        public Text targetText;                  //Display UI-Text                   //表示する UI-Text
        public string format = "{0:F1} ℃";      //The display format (if it is 'F0', there are no decimal places)    //表示フォーマット（'F0' とすれば小数点以下は無くなる）

        //For work
        StringBuilder sb = new StringBuilder(8);


        // Start is called before the first frame update
        void Start()
        {
            text = this.GetComponent<Text>();
            cnt = 0;
            ftime = Time.time - 1.0f;
            Debug.Log(SystemInfo.graphicsDeviceType);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - ftime >= 1.0f) {
                ftime = Time.time;
                string fpstxt;
                fpstxt = "" + (cnt) + "";
                // BatteryInfo info = JsonUtility.FromJson<BatteryInfo>(json);
                text.text = "GPU name:" + SystemInfo.graphicsDeviceName + "\tFPS:" + fpstxt;// +"\tTemp"+ ReceiveBatteryStatus() + "℃";
                cnt = 0;
            }
            cnt++;
        }

        //Callback handler from 'BatteryStatusController.OnStatus'
        public string ReceiveBatteryStatus(BatteryInfo info)
        {
            string s = "";
            if (targetText != null)
            {
                sb.Length = 0;
                sb.AppendFormat(format, info.temperature);
                s = sb.ToString();
            }
            return s;
        }

    }


}