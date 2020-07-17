using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using NCMB.Extensions;


public class MyRankingServer : MonoBehaviour
{
    void Start()
    {
    }

    // サーバーにハイスコアを保存 -------------------------
    public void save(long lscore,int GPUScore_no)
    {
        string NCMBClassname = "GPUScore" + GPUScore_no.ToString();
        // データストアの「HighScore」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(NCMBClassname);
        query.WhereEqualTo("name", SystemInfo.graphicsDeviceName);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            //検索成功したら
            if (e == null)
            {
                if (objList.Count > 0)
                {
                    if ((long)(objList[0]["score"]) < lscore)
                    {
                        objList[0]["score"] = lscore;
                        objList[0].SaveAsync();
                    }
                }
                else
                {
                    NCMBObject obj = new NCMBObject(NCMBClassname);
                    obj["name"] = SystemInfo.graphicsDeviceName;
                    obj["score"] = lscore;
                    obj.SaveAsync();
                }
            }
        });
    }


}
