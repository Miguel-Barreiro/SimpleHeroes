using System;
using Gram.Core;
using Gram.Model;
using Gram.Utils;
using Gram.Utils.AnimationUtils;
using Unity.Mathematics;
using UnityEngine;

namespace Gram.Battle
{
    public class BattleCharacter : MonoBehaviour
    {

        [SerializeField]
        private CharacterTooltip CharacterTooltip;

        [SerializeField]
        private HealthBar HealthBar;

        
        [SerializeField]
        private AnimationType AttackAnimationType;
        [SerializeField]
        private AnimationType HitAnimationType;
        [SerializeField]
        private AnimationType DeathAnimationType;

        
        private EndAnimationController _endAnimationController;
        
        protected ICharacterDatabase CharacterDatabase;
        protected GameObject Visuals;
        protected Animator Animator;
        protected CharacterConfiguration CharacterConfiguration;
        
        protected void Awake() {
            CharacterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
        }

        public void Setup(Character character) {
            CharacterConfiguration = CharacterDatabase.GetCharacterConfigurationById(character.CharacterNameId); 
            Visuals = GameObject.Instantiate(CharacterConfiguration.BattlePrefab, transform.position, 
                                                quaternion.identity, transform);
            _endAnimationController = Visuals.GetComponent<EndAnimationController>();
            Animator = Visuals.GetComponent<Animator>();
            
            HealthBar.SetPercentage(character.CurrentHealth/(float)character.Health);
            if (!character.IsAlive()) {
                Animator.SetTrigger(FORCE_DEAD_ANIMATOR_PARAMETER);
            }
        }

        public void ResetCharacter() {
            Destroy(Visuals);
            _endAnimationController = null;
        }


        public GameObject GetVisuals() { return Visuals; }


        private readonly int ATTACK_ANIMATOR_PARAMETER = UnityEngine.Animator.StringToHash("attack"); 
        private readonly int DEATH_ANIMATOR_PARAMETER = UnityEngine.Animator.StringToHash("death"); 
        private readonly int HIT_ANIMATOR_PARAMETER = UnityEngine.Animator.StringToHash("hit");
        private readonly int FORCE_DEAD_ANIMATOR_PARAMETER = UnityEngine.Animator.StringToHash("forceDead");
        
        
        public void Attack(int attackValue, Action doneCallback) {
            void EndAttackAnimationCallback(AnimationType type) {
                if (type == AttackAnimationType) {
                    _endAnimationController.OnAnimationEnd -= EndAttackAnimationCallback;
                    doneCallback();
                }
            }

            _endAnimationController.OnAnimationEnd += EndAttackAnimationCallback;
            Animator.SetTrigger(ATTACK_ANIMATOR_PARAMETER);
            CharacterTooltip.TriggerAttack(attackValue);
        }

        public void Damage(int damage, int newHealthValue, float percentageHealthLeft, Action doneCallback) {
            HealthBar.SetPercentage(percentageHealthLeft);
            CharacterTooltip.TriggerDamage(damage);
            
            if (newHealthValue > 0) {
                void EndHitAnimationCallback(AnimationType type) {
                    if (type == HitAnimationType) {
                        _endAnimationController.OnAnimationEnd -= EndHitAnimationCallback;
                        doneCallback();
                    }
                }

                _endAnimationController.OnAnimationEnd += EndHitAnimationCallback;
                Animator.SetTrigger(HIT_ANIMATOR_PARAMETER);
            } else {
                Kill(doneCallback);
            }
        }

        public void Kill(Action doneCallback) {
            void EndDeathAnimationCallback(AnimationType type) {
                if (type == DeathAnimationType) {
                    _endAnimationController.OnAnimationEnd -= EndDeathAnimationCallback;
                    doneCallback();
                }
            }
            HealthBar.SetPercentage(0);

            _endAnimationController.OnAnimationEnd += EndDeathAnimationCallback;
            Animator.SetTrigger(DEATH_ANIMATOR_PARAMETER);
        }
        
    }
}
