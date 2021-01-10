using TkrainDesigns.Tiles.Modular;
using UnityEngine;

namespace Tiles.Inventory
{
    public class InfinityMaterialChooser : MonoBehaviour
    {
        void Start()
        {
            int matToSet=-1;
            foreach (InfinityMaterialSwapper swapper in GetComponentsInChildren<InfinityMaterialSwapper>())
            {
                if (matToSet < 0) matToSet = Random.Range(0, swapper.MaterialCount);
                swapper.SetMaterial(matToSet);
            }
        }
    }
}