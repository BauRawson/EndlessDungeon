using UnityEngine;

public class Door : MonoBehaviour {
    public bool isOpen = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private BoxCollider2D boxCollider2D;

    private void Start()
    {
        if (isOpen) // The first level door will be open
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        boxCollider2D.enabled = false;
        spriteRenderer.sprite = openSprite;
        //spriteRenderer.sortingOrder = 3;
        isOpen = true;
    }

    public void Close()
    {
        boxCollider2D.enabled = true;
        spriteRenderer.sprite = closedSprite;
        //spriteRenderer.sortingOrder = 2;
        isOpen = false;
    }
}