using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class RectScalerWithViewport : MonoBehaviour
{
    [SerializeField]
    RectTransform refRect = null;

    [SerializeField]
    Vector2 referenceResolution = new Vector2(1080, 1920);

    [Range(0, 1)]
    [SerializeField] float matchWidthOrHeight = 0;

    float m_width = -1;
    float m_height = -1;
    float sc;

    private const float kLogBase = 2;

    private void Awake()
    {
        if (refRect == null)
        {
            refRect = GetComponent<RectTransform>();
        }
        UpdateRect();
    }
    private void Start()
    {
        float width = Screen.width / 1080.0f;
        float height = Screen.height / 1920.0f;
        sc = Mathf.Min(width, height);
        //referenceResolution *= sc;
    }

    private void Update()
    {
        UpdateRectWithCheck();
    }

    private void OnValidate()
    {
        UpdateRect();
    }

    void UpdateRectWithCheck()
    {
        Camera cam = Camera.main;
        float width = cam.rect.width * Screen.width;
        float height = cam.rect.height * Screen.height;
        if (m_width == width && m_height == height)
        {
            return;
        }
        UpdateRect();
    }

    void UpdateRect()
    {
        Camera cam = Camera.main;
        float width = cam.rect.width * Screen.width;
        float height = cam.rect.height * Screen.height;
        m_width = width;
        m_height = height;

        // canvas scalerから引用
        float logWidth = Mathf.Log(width / referenceResolution.x, kLogBase);
        float logHeight = Mathf.Log(height / referenceResolution.y, kLogBase);
        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, matchWidthOrHeight);
        float scale = Mathf.Pow(kLogBase, logWeightedAverage);

        refRect.localScale = new Vector3(scale, scale, scale);

        // スケールで縮まるので領域だけ広げる
        float revScale = 1f / scale;
        refRect.sizeDelta = new Vector2(m_width * revScale, m_height * revScale);
    }
}