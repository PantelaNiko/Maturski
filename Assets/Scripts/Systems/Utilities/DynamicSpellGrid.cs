using UnityEngine;
using UnityEngine.UI;

public class DynamicSpellGrid : MonoBehaviour
{
    [SerializeField] GridLayoutGroup grid;

    [SerializeField] float baseCellSize = 100f;

    void Update()
    {
        int itemCount = transform.childCount;

        int tier = 0;
        int capacity = 20;

        if(itemCount > capacity)
        {
            tier++;
            capacity *= 2;
        }

        float size = baseCellSize / Mathf.Pow(2, tier);

        grid.cellSize = new Vector2(size, size);
    }
}