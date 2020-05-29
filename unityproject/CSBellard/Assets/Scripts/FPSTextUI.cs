using UnityEngine;
using UnityEngine.UI;

public class FPSTextUI : MonoBehaviour
{
    private Text text;
    int cnt;
    float ftime;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        Debug.Log(SystemInfo.graphicsDeviceName);
        cnt = 0;
        ftime = Time.time - 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - ftime >= 1.0f) {
            ftime = Time.time;
            string fpstxt;
            fpstxt = "" + (cnt) + "";
            text.text = "GPU name:" + SystemInfo.graphicsDeviceName + "\t\tFPS:" + fpstxt;
            cnt = 0;
        }
        cnt++;
    }
}
