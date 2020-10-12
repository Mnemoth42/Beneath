using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Inventory;
using TkrainDesigns.Grids.Stats;
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
            EquipWeapon(defaultWeaponConfig);
            personalStats = GetComponent<PersonalStats>();
            if (TryGetComponent(out StatsEquipment e) && weaponSlot!=null)
            {
                equipment = e;
                equipment.EquipmentUpdated += CheckEquipment;
            }
        }

        public void EquipWeapon(GridWeaponConfig config)
        {
            if (config == null)
            {
                Debug.Log($"{name} is trying to equip a non-existent weapon.  You might want to fix that.");
                return;
            }
            Debug.Log($"Equipping {config.displayName}");
            weapon = config.EquipWeapon(rightHandTransform, leftHandTransform, weapon, anim);
            currentWeaponConfig = config;
        }

        void CheckEquipment()
        {
            Debug.Log("CheckEquipment");
            GridWeaponConfig config = equipment.GetItemInSlot(weaponSlot) as GridWeaponConfig;
            Debug.Log(config==null?"No Weapon?":$"{config.GetDisplayName()}");
            if (!config)
            {
                EquipWeapon(defaultWeaponConfig);
                return;
            }
            EquipWeapon(config);
        }

        float DamageCalculation()
        {
            float rawDamage = currentWeaponConfig.Damage;
            if (personalStats == null || currentWeaponConfig.GetOffensiveStat() == null) return rawDamage;
            return rawDamage * (personalStats.GetStatValue(currentWeaponConfig.GetOffensiveStat())/10f);
        }

        void Hit()
        {
            //Debug.Log("Hit!");
            if (!target) return;
            if (!currentWeaponConfig)
            {
                Debug.Log($"{name}'s fighter has no Weapon equipped!");
                return;
            }
            
            target.TakeDamage(DamageCalculation(), currentWeaponConfig.GetDefensiveStat(), gameObject);
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
            transform.LookAt(TileUtilities.IdealWorldPosition(targetLocation));
            anim.SetFloat("attackVariant", currentWeaponConfig.GetRandomAttackForm());
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(3);
            callbackAction.Invoke();
        }

    }
}