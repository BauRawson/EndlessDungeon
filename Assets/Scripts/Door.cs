using UnityEngine;

public class Door : MonoBehaviour {
    public bool isOpen = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private BoxCollider2D boxCollider2D;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

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
        float distanceToCamera = transform.position.y - mainCamera.transform.position.y;
        if (distanceToCamera <= 6) // Hacky but beautiful!
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.DoorOpen);
        }
    }

    public void Close()
    {
        float distanceToCamera = transform.position.y - mainCamera.transform.position.y;
        if (distanceToCamera <= 6)
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.DoorClose);
        }

        boxCollider2D.enabled = true;
        spriteRenderer.sprite = closedSprite;
        //spriteRenderer.sortingOrder = 2;
        isOpen = false;
    }
}