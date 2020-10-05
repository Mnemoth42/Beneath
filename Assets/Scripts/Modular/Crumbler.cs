using UnityEngine;

namespace TkrainDesigns.Tiles.Modular
{
    public class Crumbler : MonoBehaviour
    {
        public GameObject CorrespondinGameObject = null;

        public void Crumble()
        {
            if (CorrespondinGameObject)
            {
                GameObject go = Instantiate(CorrespondinGameObject, transform.position, transform.rotation);
                go.AddComponent<MeshCollider>();
                go.AddComponent<Rigidbody>();
                Destroy(go,4);
                gameObject.SetActive(false);
            }
        }
    }
}
