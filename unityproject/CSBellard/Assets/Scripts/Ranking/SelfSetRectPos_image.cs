using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfSetRectPos_image : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.localPosition = new Vector3(-467f, 550f - 45f * no + copyrectpos.y, 0f);
        //rectTransform.offsetMax = new Vector2(0f, 556f - 45f * no + copyrectpos.y);

    }
}
