using UnityEngine;

namespace TkrainDesigns.Inventories
{
    public class Chest : MonoBehaviour
    {
        Animator anim;

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        
        }

        void Start()
        {
            Open();
        }

        public void Open()
        {
            anim.SetTrigger("openLid");
        }

        public void Close()
        {
            anim.SetTrigger("closeLid");
        }
    }
}