using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TkrainDesigns.Attributes;
using TkrainDesigns.Inventories;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Grids;
using TkrainDesigns.Tiles.Movement;
using UnityEngine;
using UnityEngine.Events;


namespace TkrainDesigns.Tiles.Combat
{
    [RequireComponent(typeof(GridMover))]
    public class GridFighter : MonoBehaviour
    {
        [Header("Weapon Info")]
        [SerializeField] GridWeaponConfig defaultWeaponConfig = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [Header("Put Weapon Slot token here")]
        [SerializeField] ScriptableEquipSlot weaponSlot;
        
        [Header("Events")]
        [SerializeField] UnityEvent onHitEvent;

        public Transform GetRightHandTransform()
        {
            return rightHandTransform;
        }

        public Transform GetLeftHandTransform()
        {
            return leftHandTransform;
        }
        

        System.Action callbackAction;
        Animator anim;
        GridMover mover;
        List<Vector2Int> path;
        Vector2Int targetLocation;
        Health target;
        GridWeapon weapon=null;
        GridWeaponConfig currentWeaponConfig;
        PersonalStats personalStats;
        StatsEquipment equipment;


        

        void Awake()
        {
            anim = GetComponent<Animator>();
            mover = GetComponent<GridMover>();
            personalStats = GetComponent<PersonalStats>();
            //EquipWeapon(defaultWeaponConfig);
            if (TryGetComponent(out StatsEquipment e))
            {
                equipment = e;
                equipment.EquipmentUpdated += CheckEquipment;
            }


        }

        void Start()
        {
            if(currentWeaponConfig==null)
            {
                GridWeaponConfig defaultWeapon = Instantiate(defaultWeaponConfig);
                defaultWeapon.Level = personalStats.Level;
                EquipWeapon(defaultWeapon);
            }
        }


        public void EquipWeapon(GridWeaponConfig config)
        {
            if (config == null)
            {
                return;
            }
            weapon = config.EquipWeapon(rightHandTransform, leftHandTransform, weapon, anim);
            currentWeaponConfig = config;
        }

        void CheckEquipment()
        {
            if (!equipment) return;
            GridWeaponConfig config = equipment.GetItemInSlot(weaponSlot) as GridWeaponConfig;
            
            if (!config)
            {
                EquipWeapon(defaultWeaponConfig);
                return;
            }

            if (config!=currentWeaponConfig)
            {
                EquipWeapon(config); 
            }
            
        }


        public GridWeaponConfig GetCurrentWeaponConfig() => currentWeaponConfig;

        float DamageCalculation()
        {
            float rawDamage = currentWeaponConfig.Damage;
            return CombatBroker.CalculateDamage(gameObject,
                                                target.gameObject,
                                                rawDamage,
                                                currentWeaponConfig.GetOffensiveStat(),
                                                currentWeaponConfig.GetDefensiveStat());
        }

        void Hit()
        {
            if (!target) return;
            if (!currentWeaponConfig)
            {
                return;
            }
            
            target.TakeDamage(DamageCalculation(), gameObject);
            if (weapon)
            {
                weapon.OnHit();
            }
            onHitEvent?.Invoke();
        }

        

        public void BeginAttackAction(Health targetHealth, List<Vector2Int> pathToFollow, System.Action callback)
        {
            if (pathToFollow.Count == 0)
            {
                callback?.Invoke();
                return;
            }
            
            callbackAction = callback;
            path = pathToFollow;
            targetLocation = pathToFollow.Last();
            path.Remove(targetLocation);
            target = targetHealth;
            mover.BeginMoveAction(path, ProcessAttack);
        }

        void ProcessAttack()
        {
            StartCoroutine(Attack());
        }

        IEnumerator Attack()
        {
            transform.LookAt(targetLocation.ToWorldPosition());
            anim.SetFloat("attackVariant", currentWeaponConfig.GetRandomAttackForm(anim));
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(2.5f);
            transform.position = path.Last().ToWorldPosition();
            callbackAction.Invoke();
        }

    }
}