using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using TkrainDesigns.Grids.Stats;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Combat;
using TkrainDesigns.Tiles.Control;
using UnityEditor;
using UnityEngine;


namespace TkrainDesigns.Tiles.Actions
{
    [CreateAssetMenu(fileName = "Fire Spell", menuName = "Actions/New Fire Spell")]
    public class Fireball : OffensiveSpellBase
    {
        [SerializeField] FireballMover fireball;
        [SerializeField] bool splashDamage = false;


        public override string TimerToken()
        {
            return "Fireball";
        }

        public override bool ShouldPerform()
        {
            return true;
        }

        public override bool AIRangedAttackSpell()
        {
            return true;
        }

        public override int Range(GameObject user)
        {
            if (user == null) return 0;
            PersonalStats stats = user.GetComponent<PersonalStats>();
            if (stats && intelligenceStat)
            {
                return (int)(range + (stats.GetStatValue(intelligenceStat) * .1f));
            }
            return range;
        }

        
        

        public override void PerformAction(GameObject user, GameObject target = null, Action callback = null)
        {
            if (!target)
            {
                callback();
                return;
            }
            ActivateCooldown(user);
            CacheParameters(user, target, callback);
            currentUser.transform.LookAt(target.transform.position);
            InvokeCastAnimation(ReleaseFireball);
        }

        


        void ReleaseFireball()
        {
            Vector3 startPosition = GetStartPosition();

            FireballMover mover = Instantiate(fireball, startPosition, Quaternion.identity);
            mover.SetTarget(currentTarget.transform, Damage);

        }


        void Damage()
        {
            currentTarget.GetComponent<Health>()
                         .TakeDamage(CombatBroker.CalculateDamage(currentUser, currentTarget,
                                                                  AdjustedDamage, damageStat, defenseStat), currentUser);
            if(splashDamage) PerformSplashDamage();
            callbackAction();
        }

        void PerformSplashDamage()
        {
            foreach (BaseController c in FindObjectsOfType<BaseController>()
                                         .Where(t => Vector3.Distance(t.transform.position,
                                                                      currentTarget.transform.position) < 3)
                                         .Where(z => z.gameObject != currentTarget)
                                         .Where(y => y.gameObject != currentUser)
                                         .Where(w => w.IsAlive))
            {
                c.GetComponent<Health>().TakeDamage(CombatBroker.CalculateDamage(currentUser, c.gameObject,
                                                                                 baseDamage, damageStat, defenseStat),
                                                    currentUser);
            }
        }


#if UNITY_EDITOR

        


        void SetFireball(FireballMover go)
        {
            if (fireball == go) return;
            Undo.RecordObject(this, "Change Projectile");
            fireball = go;
            Dirty();
        }

        





        bool drawFireball;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            drawFireball = EditorGUILayout.Foldout(drawFireball, "Fireball Data", style);
            if (!drawFireball) return;
            //BeginIndent();
            SetItem(ref splashDamage, EditorGUILayout.Toggle("Splash Damage:", splashDamage));
            SetFireball(DrawObjectList("Projectile", fireball));

            //EndIndent();
        }

#endif

    }
}