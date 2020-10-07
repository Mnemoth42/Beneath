using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.Tiles.Movement;
using UnityEngine;

namespace TkrainDesigns.Tiles.Actions
{
    [RequireComponent(typeof(GridMover))]
    [RequireComponent(typeof(Animator))]
    public class ActionPerformer : MonoBehaviour
    {
        Animator anim;
        GridMover mover;

        void Awake()
        {
            mover = GetComponent<GridMover>();
            anim = GetComponent<Animator>();
        }

        Health target;
        PerformableActionItem currentActionItem;
        System.Action callbackAction;

        public void BeginAction(PerformableActionItem actionToPerform, Health targetHealth, List<Vector2Int> pathToFollow,
                                System.Action callback)
        {
            
            if (actionToPerform == null)
            {
                Debug.Log("Malformed action setup");
                callback?.Invoke();
                return;
            }

            target = targetHealth;
            callbackAction = callback;
            currentActionItem = actionToPerform;
            int stepsRequired = pathToFollow.Count - actionToPerform.Range(gameObject);
            Debug.Log($"{name} requires {stepsRequired} steps before performing action");
            if (stepsRequired > 0 && actionToPerform.AIRangedAttackSpell())
            {
                List<Vector2Int> newPath = new List<Vector2Int>();
                for(int i=0;i < stepsRequired; i++)
                {
                    newPath.Add(pathToFollow[i]);
                }
                mover.BeginMoveAction(newPath, ExecuteAction);
            }
            else
            {
                ExecuteAction();
            }
        }


        void ExecuteAction()
        {
            if (!currentActionItem)
            {
                Debug.Log($"{name} ExecuteAction called with no current Action");
                callbackAction?.Invoke();
            }
            else
            {
                currentActionItem.PerformAction(gameObject, target.gameObject,  callbackAction);
            }
        }

        public void PerformActionStep(IEnumerator step)
        {
            StartCoroutine(step);
        }

        Action animatorCallback = null;

        public void SetAnimatorCallback(Action callback)
        {
            animatorCallback = callback;
        }

        public void Perform()
        {
            animatorCallback();
        }

    }
}