using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Grids;
using UnityEngine;
using UnityEngine.AI;

namespace TkrainDesigns.Tiles.Movement
{
    public class GridMover : MonoBehaviour
    {
        Animator anim;
        [SerializeField] int maxStepsPerTurn = 3;

        [Header("Used only when Root Motion is not applied in animator.")] [SerializeField]
        float movementSpeed = 2.0f;

        [SerializeField] SkinnedMeshRenderer rend;

        [Header("Place Movement Stat Here")] [SerializeField]
        ScriptableStat movementStat = null;

        Vector2Int currentPosition;
        System.Action callbackAction;
        public event System.Action onMoveStepCompleted;
        PersonalStats stats;
        List<Vector2Int> path = new List<Vector2Int>();
        bool isMoving = false;
        Vector3 locationToTrack;


        public int MaxStepsPerTurn
        {
            get
            {
                if (!stats || !movementStat)
                {
                    return maxStepsPerTurn;
                }

                return (int) stats.GetStatValue(movementStat);
            }
            //set => maxStepsPerTurn = value;
        }

        void Awake()
        {
            anim = GetComponent<Animator>();
            stats = GetComponent<PersonalStats>();
            currentPosition = TileUtilities.GridPosition(transform.position);
        }

        public void BeginMoveAction(List<Vector2Int> pathToFollow, System.Action callback)
        {
            callbackAction = callback;
            path = pathToFollow;
            StopAllCoroutines();
            StartCoroutine(ProcessMovement());
        }

        public void Update()
        {
            if (!isMoving || anim.applyRootMotion) return;
            transform.LookAt(locationToTrack);
            transform.Translate(Vector3.forward * (movementSpeed * Time.deltaTime));
        }

        IEnumerator ProcessMovement()
        {
            Vector3 destination = transform.position;
            Queue<Vector3> queue = new Queue<Vector3>();
            if (path.Count > 1)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    if (i < MaxStepsPerTurn + 1)
                    {
                        destination = TileUtilities.IdealWorldPosition(path[i]);
                    }

                    queue.Enqueue(destination);
                }
            }
            else
            {
                callbackAction?.Invoke();
                yield break;
            }

            if (queue.Count > 0)
            {
                queue.Dequeue(); //cook off the tile we're standing on.


                int currentSteps = 0;
                yield return new WaitForEndOfFrame();
                while (queue.Count > 0 && currentSteps < MaxStepsPerTurn)
                {
                    yield return null;
                    Vector3 currentWaypoint = queue.Dequeue();
                    yield return MoveToNextLocation(currentWaypoint);
                    currentSteps++;
                    onMoveStepCompleted?.Invoke();
                }
            }


            anim.SetTrigger("Idle");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            {
                
                transform.position = destination;
                yield return new WaitForEndOfFrame();
            }

            transform.position = destination;

            callbackAction?.Invoke();
        }

        IEnumerator MoveToNextLocation(Vector3 newLocation)
        {
            //Debug.Log($"{name} is moving to {newLocation}");
            if (rend && rend.enabled)
            {
                locationToTrack = newLocation;
                transform.LookAt(newLocation);

                anim.ResetTrigger("Walking");
                anim.SetTrigger("Walking");

                while (Vector3.Distance(transform.position, newLocation) > .2f)

                {
                    transform.LookAt(newLocation);
                    isMoving = true;
                    yield return null;
                }

                isMoving = false;
            }
            else
            {
               // Debug.Log($"{name} is not on screen, and will move instantly");
            }

            transform.position = newLocation;
        }

        public void MoveBackToIdealPosition(Vector3 location)
        {
            StartCoroutine(MoveToNextLocation(location));
        }

        void FootR()
        {

        }

        void FootL()
        {

        }
    }
}