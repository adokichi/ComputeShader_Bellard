using UnityEngine;
using UnityEngine.UI;
using System.Text;



public class GPUName : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        Debug.Log(SystemInfo.graphicsDeviceType);
        text.text = "GPU name:" + SystemInfo.graphicsDeviceName;
    }


}