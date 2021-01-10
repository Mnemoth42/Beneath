using TMPro;
using UnityEngine;

namespace TkrainDesigns.Tiles.Grids
{

    public class GridNamer : MonoBehaviour
    {
        [SerializeField]
        TextMeshPro text;
#if UNITY_EDITOR
        void Awake()
        {
            if (gameObject.activeInHierarchy)
            {
                Vector2Int position = TileUtilities.GridPosition(transform.position);
                name = $"{position.x},{position.y}";
                if (text)
                {
                    text.text = name;
                }
            }
        }
#endif
    }

}