// Create a Sprite at start-up.
// Assign a texture to the sprite when the button is pressed.

using UnityEngine;

public class BackProgressBar : MonoBehaviour
{
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(14f / 255f, 149f / 255f, 120f / 255f, 1.0f);
        sr.transform.localScale = new Vector3(26.0f * 0.0f, 3.4f, 1f);
    }

    public void SetProgress(float progress)
    {
        sr.transform.localScale = new Vector3(26.0f * progress, 3.4f, 1f);
    }


}