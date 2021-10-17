using System.Collections.Generic;
using Battle.Heroes;
using Game;

namespace Model
{
    public class GameModel
    {

        public delegate void GameModelDelegate();

        public event GameModelDelegate OnLogicStateChange;
        public event GameModelDelegate OnHeroCollectionChange;
        
        public event GameModelDelegate OnSelectedHeroesChange;
        
        private GameState _currentState;

        private CharacterDatabase _characterDatabase;
        private GameDefinitions _gameDefinitions;
        
        
        public GameModel(GameDefinitions gameDefinitions, CharacterDatabase characterDatabase) {
            _characterDatabase = characterDatabase;
            _gameDefinitions = gameDefinitions;
        }


        public GameState GetGameState() {
            return _currentState;
        }
        public void SetGameState(GameState newGameState) {
            _currentState = newGameState;
        }
        public void GenerateInitialGameState() {

            _currentState = new GameState() {
                HeroesCollected = new List<Hero>(),
                BattleCount = 0,
                SelectedHeroes = new List<int>(),
                CurrentLogicState = GameState.GameLogicState.HeroSelection
            };

            GenerateInitialHeroes();
        }

        private void GenerateInitialHeroes() {
            
            int numberHeroes = _gameDefinitions.InitialNumberHeroes;
            
            List<CharacterConfiguration> newHeroCharacters = _characterDatabase.GetHeroCharactersData(numberHeroes);
            foreach (CharacterConfiguration newHeroCharacter in newHeroCharacters) {
                _currentState.HeroesCollected.Add(GenerateNewHero(newHeroCharacter));
            }
        }

        private Hero GenerateNewHero(CharacterConfiguration heroCharacter) {
            Hero newHero = new Hero() {
                Experience = 0,
                Level = 0,
                Health = heroCharacter.InitialHealth,
                AttackPower = heroCharacter.InitialAttackPower,
                CharacterDataName = heroCharacter.Name
            };
            return newHero;
        }

        public void Start() {
            OnLogicStateChange?.Invoke();
        }
    }
}