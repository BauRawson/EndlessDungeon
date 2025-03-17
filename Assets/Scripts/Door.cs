using UnityEngine;

public class Door : MonoBehaviour {
    public bool isOpen = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private BoxCollider2D boxCollider2D;

    public void Open()
    {
        boxCollider2D.enabled = false;
        spriteRenderer.sprite = openSprite;
        Debug.Log("Open Door!");
        isOpen = true;
    }

    public void Close()
    {
        boxCollider2D.enabled = true;
        spriteRenderer.sprite = closedSprite;
        Debug.Log("Close Door!");
        isOpen = false;
    }
}