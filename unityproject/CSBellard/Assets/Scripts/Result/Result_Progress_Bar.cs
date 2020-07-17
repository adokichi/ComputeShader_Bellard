using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Progress_Bar : MonoBehaviour
{
    Texture2D tex;
    int wx = 256;
    int wy = 256;
    int cnt = 0;
    int maxrad = 290;//度数表記

    [SerializeField]
    Text scoretext;

    [SerializeField]
    GameObject hoge0obj;

    Hoge0 hoge0;

    long score = 550;
    void Start()
    {
        tex = new Texture2D(wx, wy);
        GetComponent<SpriteRenderer>().sprite= Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1.0f);
        TexCntInit();
        hoge0 = hoge0obj.GetComponent<Hoge0>();
    }

    public void TexCntInit()
    {
        for (int ix = 0; ix < wx; ix++)
        {
            for (int iy = 0; iy < wy; iy++)
            {
                tex.SetPixel(ix, iy, new Color(0f, 0f, 0f));
            }
        }
        tex.Apply();
        cnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cnt == 0) //初期化みたいなもん
        {
            score = hoge0.lscore;
        }


        if (cnt <= maxrad) 
        {
            float leng = 103.0f;
            var sn = Mathf.Sin((90.0f - cnt * 1.0f) / 180.0f * 3.14159265359f);
            var cs = Mathf.Cos((90.0f - cnt * 1.0f) / 180.0f * 3.14159265359f);
            for (int i = 0; i < 40; i++) 
            {
                int ix = (int)(cs * (leng + 0.5f * i) + wx * 0.5f);
                int iy = (int)(sn * (leng + 0.5f * i) + wy * 0.5f);
                tex.SetPixel(ix, iy, new Color(36f / 255f, 154f / 255f, 129f / 255f));
            }
            tex.Apply();


            scoretext.text = ((int)(1.0f * score / maxrad * cnt)).ToString();
        }
        cnt++;

    }
}