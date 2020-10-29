using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TkrainDesigns.Tiles.Core
{
    public enum Tilestate
    {
        Normal,
        Walkable,
        Occupied
    }

    [RequireComponent(typeof(MeshRenderer))]
    public class ColorChanger : MonoBehaviour
    {
        
        
        [SerializeField] Color normalColor = Color.white;
        [SerializeField] Color normalHighlight = Color.gray;
        [SerializeField] Color walkableColor = Color.blue;
        [SerializeField] Color walkableHighlightColor = Color.cyan;
        [SerializeField] Color occupiedColor = Color.red;
        [SerializeField] Color occupiedHighlight = Color.magenta;
        Tilestate currentState;
        bool selected = false;

        MeshRenderer rend;
        Material mat;

        void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            if(rend)
            {
                mat = rend.material;
            }
        }


        public void SetMouseOver(bool mouseIsOver)
        {
            selected = mouseIsOver;
            switch (currentState)
            {
                case Tilestate.Normal:
                    mat.SetColor("SelectionColor", selected?normalHighlight:normalColor );
                    break;
                case Tilestate.Walkable:
                    mat.SetColor("SelectionColor", selected?walkableHighlightColor:walkableColor);
                    break;
                case Tilestate.Occupied:
                    mat.SetColor("SelectionColor", selected?occupiedHighlight:occupiedColor);
                    break;
            }

        }

        public void SetMaterialOccupied()
        {
            mat.SetColor("SelectionColor", selected?occupiedHighlight:occupiedColor);
            currentState = Tilestate.Occupied;
        }

        public void ResetMaterial()
        {
            mat.SetColor("SelectionColor", normalColor);
            currentState = Tilestate.Normal;
        }

        public void SetMaterialWalkable()
        {
            mat.SetColor("SelectionColor", selected?walkableHighlightColor: walkableColor);
            currentState = Tilestate.Walkable;
        }
    }
}