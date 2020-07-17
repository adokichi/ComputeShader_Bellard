using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GPUName : MonoBehaviour
{
    // Start is called before the first frame update
    string gname;
    void Start()
    {
        gname = SystemInfo.graphicsDeviceName;
        GetComponent<Text>().text = "GPUName : " + gname;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
