using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfSetRectPos_text : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    Copyrectpos copyrectpos;
    RectTransform rectTransform;
    public int no;
    void Start()
    {
        copyrectpos = GameObject.Find("Copyrectpos").GetComponent<Copyrectpos>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.offsetMax = new Vector2(0f, 0f - 45f * no + copyrectpos.y);
    }
}
