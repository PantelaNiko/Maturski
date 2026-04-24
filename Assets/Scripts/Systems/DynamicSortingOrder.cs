using UnityEngine;
public class DynamicSortingOrder : MonoBehaviour
{
    public int sortingOffset = 0; 

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Lower y → in front, Higher y → behind
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100f) + sortingOffset;
    }
}