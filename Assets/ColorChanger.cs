using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TkrainDesigns.Tiles.Core
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ColorChanger : MonoBehaviour
    {
        const string Selected = "Bool_Selected";
        const string Blocked = "Bool_Blocked";
        const string MouseOver = "Bool_MouseOver";

        [SerializeField] Material highlightMaterial;
        [SerializeField] Material selectedMaterial;
        MeshRenderer rend;
        Material originalMaterial;

        void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            if(rend)
            {
                originalMaterial = rend.material;
            }
        }

        public void SetMouseOver(bool mouseIsOver)
        {
#if UNITY_ANDROID
            return;
#else
            rend.material.SetFloat(MouseOver, mouseIsOver? 1.0f: 0.0f);
#endif
        }

        public void HighlightMaterial()
        {
            if (rend != null)
            {
#if UNITY_ANDROID
                rend.material = highlightMaterial;
#else
                //rend.material = highlightMaterial;
                rend.material.SetFloat(Selected, 0.0f);
                rend.material.SetFloat(Blocked, 1.0f);
#endif
            }
        }

        public void ResetMaterial()
        {
            if (rend != null)
            {
#if UNITY_ANDROID
                rend.material = originalMaterial;
#else
                rend.material.SetFloat(Selected, 0.0f);
                rend.material.SetFloat(Blocked, 0.0f);
#endif

            }
        }

        public void SelectedMaterial()
        {
            if (rend != null )
            {

#if UNITY_ANDROID
                rend.material = selectedMaterial;
#else
                rend.material.SetFloat(Selected, 1.0f);
                rend.material.SetFloat(Blocked, 0.0f);
#endif
            }
        }
    }
}