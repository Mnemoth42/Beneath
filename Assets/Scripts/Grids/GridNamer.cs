using TMPro;
using UnityEngine;

namespace TkrainDesigns.Tiles.Grids
{
    [ExecuteAlways]
    public class GridNamer : MonoBehaviour
    {
        [SerializeField]
        TextMeshPro text;
#if UNITY_EDITOR
        // Update is called once per frame
        void Update()
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