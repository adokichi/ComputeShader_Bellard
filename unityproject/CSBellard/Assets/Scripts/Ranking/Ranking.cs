using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using NCMB.Extensions;


//ランキング描画画面のスクリプト
//ランキングにのるtextとimageはこいつが全部instanceする

public class Ranking : MonoBehaviour
{
    NCMBQuery<NCMBObject> query;

    [SerializeField]
    GameObject Panel;
    [SerializeField]
    GameObject name_text_prefab;
    [SerializeField]
    GameObject score_text_prefab;
    [SerializeField]
    GameObject score_image_prefab;
    [SerializeField]
    Copyrectpos copyrectpos;
    [SerializeField]
    Hoge0 hoge0;

    GameObject[] objs;

    public int cnt = 0;
    void Start()
    {
        cnt = 0;
        objs = new GameObject[512 * 3];
        
    }

    private void Update()
    {
        if (cnt == 0)
        {
            cnt++;
            RankingCreate();
        }
        cnt++;

    }

    void RankingCreate()
    {
        query = new NCMBQuery<NCMBObject>("GPUScore" + hoge0.last_ranker_number.ToString());
        query.OrderByDescending("score");
        query.Limit = 512;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
       {
           if (e != null)
           {
               //検索失敗
               Debug.Log("検索に失敗しました。\nErrorCode" + e.ErrorCode);
           }
           else 
           {
               long maxscore = 0;
               for (int i = 0; i < objList.Count; i++)
               {
                   maxscore = System.Math.Max(maxscore, (long)objList[i]["score"]);
                   
               }
               float bar_scale = 900.0f / Mathf.Log(Mathf.Max(2f, maxscore));


               copyrectpos.maxlistnum = objList.Count;
               for (int i=0;i < objList.Count; i++) 
               {
                   int idx = i % objList.Count;
                   GameObject obj;

                   //image
                   objs[i * 3] = obj = Instantiate(score_image_prefab, transform.position, Quaternion.identity);
                   obj.transform.parent = Panel.transform;
                   obj.GetComponent<SelfSetRectPos_image>().no = i;
                   //色チェン
                   int colflag = 0;
                   if (SystemInfo.graphicsDeviceName == (string)(objList[idx]["name"]))
                   {
                       colflag = 1;
                       obj.GetComponent<Image>().color = new Color(14f / 255f, 149f / 255f, 120f / 255f);
                   }
                   //スコア長さ
                   var t = obj.GetComponent<RectTransform>();
                   var sizeDelta = t.sizeDelta;
                   sizeDelta = new Vector2(
                       Mathf.Log((long)objList[idx]["score"] + 1) * bar_scale
                       , 34.0f);
                   t.sizeDelta = sizeDelta;

                   //GPU名
                   objs[i * 3 + 1] = obj = Instantiate(name_text_prefab, transform.position, Quaternion.identity);
                   obj.transform.parent = Panel.transform;
                   obj.GetComponent<Text>().text = (i + 1).ToString() + ":" + objList[idx]["name"];
                   obj.GetComponent<SelfSetRectPos_text>().no = i;
                   if (colflag == 1)
                       obj.GetComponent<Text>().color = new Color(1, 1, 1);

                   //socre
                   objs[i * 3 + 2] = obj = Instantiate(score_text_prefab, transform.position, Quaternion.identity);
                   obj.transform.parent = Panel.transform;
                   obj.GetComponent<Text>().text = "" + objList[idx]["score"];
                   obj.GetComponent<SelfSetRectPos_text>().no = i;
                   if (colflag == 1)
                       obj.GetComponent<Text>().color = new Color(1, 1, 1);
               }
           }
       });
    }


    public void AllDestroy() 
    {
        for (int i = 0; i < 512 * 3; i++) 
        { 
            if (objs[i] != null) 
            {
                Destroy(objs[i]);
                objs[i] = null;
            }
        }
    }


}
