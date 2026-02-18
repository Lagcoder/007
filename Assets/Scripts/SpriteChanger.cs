using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite[] sprites;
    public Color[] colors;

    private SpriteRenderer _spriteRenderer;
    int spriteLoad = 0;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(string action)
    {
        spriteLoad = 0;
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null && sprites[i].name == action)
            {
                _spriteRenderer.sprite = sprites[i];
                spriteLoad = 1;

                if (colors != null && i < colors.Length)
                {
                    _spriteRenderer.color = colors[i];
                }
                return;
            }
        }
        if (spriteLoad == 0)
        {
            _spriteRenderer.sprite = sprites[0];
            _spriteRenderer.color = colors[0];
        }
    }
}
