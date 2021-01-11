using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Attributes;
using UnityEngine;

namespace TkrainDesigns.Tiles.Modular
{
    [RequireComponent(typeof(Renderer))]
    public class InfinityMaterialSwapper : MonoBehaviour
    {
        
        [SerializeField] List<Material> materials = new List<Material>();
        [SerializeField] Shader fadeShader = null;
        [Range(.25f,10.0f)]
        [SerializeField] float fadeSpeed = 1.0f;
        int currentMaterial = 0;

        Health health;
        Renderer rend;

        public int MaterialCount => materials.Count;

        void Awake()
        {
            rend = GetComponent<Renderer>();
            health = GetComponentInParent<Health>();
        }

        void OnEnable()
        {
            health.onDeath.AddListener(OnDeath);
        }

        public void SetMaterial(int mat)
        {
            if (materials == null) return;
            currentMaterial = (mat + materials.Count) % materials.Count;
            rend.material = materials[currentMaterial];
        }

        void OnDeath()
        {
            StartCoroutine(DeathFadeOut());
        }

        void OnDisable()
        {
            health.onDeath.RemoveListener(OnDeath);
        }

        IEnumerator DeathFadeOut()
        {
            float fade = 1;
            if (fadeShader != null)
            {
                foreach (Material mat in rend.materials)
                {
                    mat.shader = fadeShader;
                }
            }

            while (fade > 0)
            {
                fade -= 1 / fadeSpeed;
                foreach (Material mat in rend.materials)
                {
                    mat.SetFloat("Fadeout", fade);
                }
                yield return new WaitForSeconds(.1f);
            }

            rend.enabled = false;
        }


    }
}