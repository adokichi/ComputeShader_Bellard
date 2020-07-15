using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackImg : MonoBehaviour
{
    SpriteRenderer MainSpriteRenderer;
    // publicで宣言し、inspectorで設定可能にする
    public Sprite Sprite_Waitbench;
    public Sprite Sprite_NowOnbench;

    void Start()
    {
        // このobjectのSpriteRendererを取得
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // 何かしらのタイミングで呼ばれる
    public void SetNowbench()
    {
        MainSpriteRenderer.sprite = Sprite_NowOnbench;
    }

    public void SetWaitbench()
    {
        MainSpriteRenderer.sprite = Sprite_Waitbench;
    }
}
