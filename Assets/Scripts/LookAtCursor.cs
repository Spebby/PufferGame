using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtCursor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Update()
    {
        Vector2 mousePosition2D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		_spriteRenderer.flipX = mousePosition2D.x > transform.position.x;

        Vector2 lookDirection2D = mousePosition2D - (Vector2)transform.position;
        transform.right = mousePosition2D.x > transform.position.x ? lookDirection2D : -lookDirection2D;
    }
}
