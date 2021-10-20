using System;
using Gram.Core;
using Gram.Model;
using Gram.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gram.UI
{
    public class BattleResultScreen : MonoBehaviour
    {

        [Serializable]
        public class HeroResultPanel
        {
            public TextMeshProUGUI Result;
            public Image Portrait;
        }

        [SerializeField]
        private HeroResultPanel[] HeroResultPanels;

        
        [SerializeField] private GameObject BattleResultPanelRoot;

        [SerializeField] private TextMeshProUGUI Title;
        
        void Start()
        {
            Hide();
            _gameModel.OnLogicStateChange += OnLogicStateChange;
        }

        private void OnLogicStateChange() {
            Hide();
        }

        private GameDefinitions _gameDefinitions;
        private ICharacterDatabase _characterDatabase;
        private IGameModel _gameModel;
        private void Awake() {
            _gameDefinitions = BasicDependencyInjector.Instance().GetObjectByType<GameDefinitions>();
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
        }

        public void Hide() {
            BattleResultPanelRoot.SetActive(false);
        }

        public void ShowResult(BattleTurn turn) {
            BattleResultPanelRoot.SetActive(true);

            if (turn.BattleEndResult.PlayerWon) {
                Title.text = "Victorious";
            } else {
                Title.text = "Defeated";
            }
            
            int index = 0;
            foreach (BattleTurn.HeroResult heroResult in turn.BattleEndResult.HeroResults) {
                HeroResultPanel heroResultPanel = HeroResultPanels[index];
                CharacterConfiguration heroData = _characterDatabase.GetCharacterConfigurationById(heroResult.Hero.CharacterNameId);
                heroResultPanel.Portrait.sprite = heroData.Portrait;

                if (!heroResult.Hero.IsAlive()) {
                    heroResultPanel.Result.text = "DEAD";
                }else if (heroResult.LevelsGained > 0) {
                    heroResultPanel.Result.text = $"Level up ({heroResult.Hero.Level}) \n +{ heroResult.HealthGained} hp \n +{ heroResult.AttackPowerGained} atk";
                } else {
                    heroResultPanel.Result.text = $"xp reached {heroResult.Hero.Experience}/{_gameDefinitions.ExperienceLevelUp}";
                }

                index++;
            }
        }
    }
}
