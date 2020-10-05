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
            rend.material.SetFloat(MouseOver, mouseIsOver? 1.0f: 0.0f);
        }

        public void HighlightMaterial()
        {
            if (rend != null && highlightMaterial != null)
            {
                //rend.material = highlightMaterial;
                rend.material.SetFloat(Selected, 0.0f);
                rend.material.SetFloat(Blocked, 1.0f);
            }
        }

        public void ResetMaterial()
        {
            if (rend != null)
            {
                //rend.material = originalMaterial;
                rend.material.SetFloat(Selected, 0.0f);
                rend.material.SetFloat(Blocked, 0.0f);
                
            }
        }

        public void SelectedMaterial()
        {
            if (rend != null && selectedMaterial != null)
            {
                //rend.material = selectedMaterial;
                rend.material.SetFloat(Selected, 1.0f);
                rend.material.SetFloat(Blocked, 0.0f);
            }
        }
    }
}