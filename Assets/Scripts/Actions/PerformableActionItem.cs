using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using TkrainDesigns.ScriptableEnums;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Actions
{
    public abstract class PerformableActionItem : ActionItem
    {
        protected GameObject currentUser;
        protected GameObject currentTarget;
        protected Action callbackAction;
        protected ActionPerformer performer;

        public virtual void PerformAction(GameObject user, GameObject target = null, Action callback = null)
        {
            callback?.Invoke();
        }

        protected void CacheParameters(GameObject user, GameObject target, Action callback)
        {
            currentUser = user;
            performer = user.GetComponent<ActionPerformer>();
            currentTarget = target;
            callbackAction = callback;
        }

        protected void InvokeCastAnimation(Action animatorCallback)
        {
            performer.transform.LookAt(currentTarget.transform.position);
            performer.SetAnimatorCallback(animatorCallback);
            currentUser.GetComponent<Animator>().SetTrigger("Cast");
        }

#if UNITY_EDITOR
        protected void SetStat(ref ScriptableStat statToChange, ScriptableStat newStat, string caption="Stat")
        {
            if (statToChange == newStat) return;
            Undo.RecordObject(this, $"Set {caption}");
            statToChange = newStat;
            Dirty();
        }

        protected void DrawStat(ref ScriptableStat stat, string title, string tooltip)
        {
            GUIContent label = new GUIContent(title, tooltip);
            SetStat(ref stat, 
                    (ScriptableStat)EditorGUILayout.ObjectField(label,
                                                                          stat,
                                                                          typeof(ScriptableStat),
                                                                          false),
                    title);
        }

#endif

    }
}