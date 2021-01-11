using UnityEngine;

namespace TkrainDesigns.Tiles.Grids
{
    [ExecuteAlways]
    public class SnapToPosition : MonoBehaviour
    {
#if UNITY_EDITOR
        void Update()
        {
            //if (Application.isPlaying) return;
            Vector3 position = transform.position;
            transform.position = position.ToGridPosition().ToWorldPosition();
        }
#endif
    }

}