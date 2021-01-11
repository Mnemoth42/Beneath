using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TkrainDesigns.Attributes;
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
        List<Vector2Int> path;

        public void BeginAction(PerformableActionItem actionToPerform, Health targetHealth, List<Vector2Int> pathToFollow,
                                System.Action callback)
        {
            
            if (actionToPerform == null)
            {
                callback?.Invoke();
                return;
            }

            if (pathToFollow == null)
            {
                Debug.Log($"{name} is trying to perform {actionToPerform.displayName}, but the path is null");
                callback?.Invoke();
                return;
            }
            target = targetHealth;
            callbackAction = callback;
            currentActionItem = actionToPerform;
            path = GetPerformablePath(actionToPerform, target, pathToFollow);
            if(path==null)
            {
                callback?.Invoke();
                return;
            }

            if(path.Count==0)
            {
                ExecuteAction();
            }
            else
            {
                mover.BeginMoveAction(path, ExecuteAction);
            }

            //path = pathToFollow;
            //Vector2Int targetLocation = pathToFollow.Last();
            //path.Remove(targetLocation);
            //int stepsRequired = path.Count - actionToPerform.Range(gameObject);
            //if (stepsRequired > 0 && actionToPerform.AIRangedAttackSpell())
            //{
            //    List<Vector2Int> newPath = new List<Vector2Int>();
            //    for(int i=0;i < stepsRequired; i++)
            //    {
            //        newPath.Add(path[i]);
            //    }
            //    mover.BeginMoveAction(newPath, ExecuteAction);
            //}
            //else
            //{
            //    ExecuteAction();
            //}
        }

        public List<Vector2Int> GetPerformablePath(PerformableActionItem actionToPerform,Health target, List<Vector2Int> pathToFollow)
        {
            if (pathToFollow == null)
            {
                Debug.Log("Path is NULL");
                return null;
            }
           // Debug.Log($"{name} is checking for a path to execute {actionToPerform.displayName} (Start length = {pathToFollow.Count}");
            if (actionToPerform.AIHealingSpell())
            {
                Debug.Log($"{name}'s action {actionToPerform.GetDisplayName()} is a healing spell, no movement required.");
                return new List<Vector2Int>();
            }

            Vector2Int targetLocation = pathToFollow.Last();
            pathToFollow.Remove(targetLocation);
            if(pathToFollow.Count==1)
            {
              //  Debug.Log($"{name}'s target is adjacent to self, no movement required");
                return new List<Vector2Int>();
            }

            int firstPossibleCastingPoint = Mathf.Max(pathToFollow.Count-2-actionToPerform.Range(gameObject), 0);
            int lastPossibleCastingPoint = Mathf.Min(mover.MaxStepsPerTurn-1, pathToFollow.Count-2);
            //Debug.Log($"{name} is calculating path, First possible spot = {firstPossibleCastingPoint}, second possible spot = {lastPossibleCastingPoint}");
            List<Vector2Int> result = new List<Vector2Int>();
            for (int i = 0; i <= lastPossibleCastingPoint; i++)
            {
                //Debug.Log($"{name} adding {i} {pathToFollow[i]}");
                result.Add(pathToFollow[i]);
                if (i < firstPossibleCastingPoint)
                {
                    //Debug.Log($"Point {i} is out of range.");
                    continue;
                }
                if (Vector2Int.Distance(targetLocation, pathToFollow[i]) < pathToFollow.Count - i)
                {
                    continue;
                }
                //Debug.Log($"Ideal casting point located.");
                return result;
            }
            //Debug.Log("No adequate casting point located.");
            return null;
        }

        void ExecuteAction()
        {
            if (!currentActionItem)
            {
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