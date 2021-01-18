using System;
using TkrainDesigns.Attributes;
using TkrainDesigns.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Actions
{
    [CreateAssetMenu(fileName="HealthPotion", menuName = "Actions/Health Potion")]
    public class HealthPotion : PerformableActionItem
    {
        [SerializeField] float amountToHeal;
        [SerializeField] bool isPercentage;
        [SerializeField] GameObject particleGameObject;
        [SerializeField] ScriptableAudioArray audioArray = null;
    
        public override bool Use(GameObject user)
        {
            if (!user) return false;
            Health health = user.GetComponent<Health>();
            if (!health) return false;
            health.Heal(isPercentage?health.MaxHealth*(amountToHeal/100.0f):amountToHeal);
            ActivateCooldown(user);
            if (particleGameObject)
            {
                Instantiate(particleGameObject, user.transform.position, Quaternion.identity);
            }

            if (audioArray)
            {
                AudioSource.PlayClipAtPoint(audioArray.GetRandomClip(),user.transform.position, 1.0f);
            }
            return true;
        }

        public override void PerformAction(GameObject user, GameObject target = null, Action callback = null)
        {
            Use(user);
            callback?.Invoke();
        }

        public override bool AIHealingSpell()
        {
            return true;
        }

        public override int Range(GameObject user)
        {
            return 300;
        }

        public override string TimerToken()
        {
            return "Health"+(IsConsumable()?"Potion":"Spell");
        }

        public override bool CanUse(GameObject user)
        {
            if (!base.CanUse(user)) return false;
            Health health = user.GetComponent<Health>();
            if (!health || health.IsDead) return false;
            if (health.HealthAsPercentage >= 1.0f) return false;
            return true;
        }

        public override bool CanUseImmediate(GameObject user)
        {
            return false;
        }

        public override string GetDescription()
        {
            string result = base.GetDescription();
            result += "\n\n";
            result += isPercentage
                          ? $"Will heal for {amountToHeal} percent of max health."
                          : $"Will heal for {amountToHeal} health points.";
            return result;
        }

        

#if UNITY_EDITOR

        void SetAmountToHeal(float value)
        {
            if (amountToHeal == value) return;
            Undo.RecordObject(this, "Set Amount To Heal");
            amountToHeal = value;
            Dirty();
        }

        void SetIsPercentage(bool value)
        {
            if (isPercentage == value) return;
            Undo.RecordObject(this, value?"Heal Is Percentage":"Heal Is Absolute");
            isPercentage = value;
            Dirty();
        }

        public void SetParticleSystem(GameObject go)
        {
            if (particleGameObject == go) return;
            Undo.RecordObject(this, particleGameObject==null?"Assign Particle System":"Change Particle System");
            particleGameObject = go;
            Dirty();
        }

        public void SetAudioArray(ScriptableAudioArray array)
        {
            if (audioArray==array) return;
            Undo.RecordObject(this, "Change Audio Array");
            audioArray = array;
            Dirty();
        }

        bool drawHealthPotion = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            drawHealthPotion = EditorGUILayout.Foldout(drawHealthPotion, "HealthPotion Data", style);
            if (!drawHealthPotion) return;
            SetAmountToHeal(EditorGUILayout.IntSlider("Amount To Heal", (int)amountToHeal, 1,100));
            SetIsPercentage(EditorGUILayout.Toggle("Is Percentage", isPercentage));
            SetParticleSystem((GameObject)EditorGUILayout.ObjectField("Particle System:", particleGameObject, typeof(GameObject),false));
            SetAudioArray((ScriptableAudioArray)EditorGUILayout.ObjectField("Audio Array", audioArray, typeof(ScriptableAudioArray), false));
        }

#endif

    }
}