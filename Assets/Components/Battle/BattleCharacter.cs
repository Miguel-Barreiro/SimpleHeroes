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

        public void Setup(Character character, bool flip) {
            CharacterConfiguration = CharacterDatabase.GetCharacterConfigurationById(character.CharacterDataName); 
            Visuals = GameObject.Instantiate(CharacterConfiguration.BattlePrefab, transform.position, 
                                                quaternion.identity, transform);
            if (flip) {
                var localScale = Visuals.transform.localScale;
                Vector3 newScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                Visuals.transform.localScale = newScale;
            }
            _endAnimationController = Visuals.GetComponent<EndAnimationController>();
            Animator = Visuals.GetComponent<Animator>();
            
            HealthBar.SetPercentage(character.CurrentHealth/(float)character.Health);
        }

        public void ResetCharacter() {
            Destroy(Visuals);
            _endAnimationController = null;
        }

        

        private readonly int ATTACK_ANIMATOR_PARAMETER = UnityEngine.Animator.StringToHash("attack"); 
        private readonly int DEATH_ANIMATOR_PARAMETER = UnityEngine.Animator.StringToHash("death"); 
        private readonly int HIT_ANIMATOR_PARAMETER = UnityEngine.Animator.StringToHash("hit"); 
        
        public void Attack(Action doneCallback) {
            void EndAttackAnimationCallback(AnimationType type) {
                if (type == AttackAnimationType) {
                    _endAnimationController.OnAnimationEnd -= EndAttackAnimationCallback;
                    doneCallback();
                }
            }

            _endAnimationController.OnAnimationEnd += EndAttackAnimationCallback;
            Animator.SetTrigger(ATTACK_ANIMATOR_PARAMETER);
        }

        public void Damage(int damage, int newHealthValue, float percentageHealthLeft, Action doneCallback) {
            void EndHitAnimationCallback(AnimationType type) {
                if (type == HitAnimationType) {
                    _endAnimationController.OnAnimationEnd -= EndHitAnimationCallback;
                    doneCallback();
                }
            }

            _endAnimationController.OnAnimationEnd += EndHitAnimationCallback;
            Animator.SetTrigger(HIT_ANIMATOR_PARAMETER);
            
            HealthBar.SetPercentage(percentageHealthLeft);
        }

        public void Kill(Action doneCallback) {
            void EndDeathAnimationCallback(AnimationType type) {
                if (type == DeathAnimationType) {
                    _endAnimationController.OnAnimationEnd -= EndDeathAnimationCallback;
                    doneCallback();
                }
            }

            _endAnimationController.OnAnimationEnd += EndDeathAnimationCallback;
            Animator.SetTrigger(DEATH_ANIMATOR_PARAMETER);
        }
        
    }
}
