using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NocanvasObjects : MonoBehaviour
{
    // Start is called before the first frame update
    float sc = 1.0f;
    void Start()
    {
        Camera cam = Camera.main;
        cam.orthographicSize = 9.6f;

        float width = Screen.width / 1080.0f;
        float height = Screen.height / 1920.0f;
        if (Screen.height / Screen.width > 1920.0f / 1080.0f)
        {
            sc = Mathf.Min(width, height) / Mathf.Max(width, height);
        }
        else
        {
            sc = 1.0f;
        }

        transform.localScale = new Vector3(sc, sc, 1f);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
