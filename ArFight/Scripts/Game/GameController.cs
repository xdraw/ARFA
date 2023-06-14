using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;
using Random = UnityEngine.Random;

namespace ArFight.Scripts.Game
{
    public class GameController
    {
        private States _currenState;
        private int _turn;
        private TargetsController _targetsController;
        private List<CharacterData> _characterDatas = new List<CharacterData>();
        private GameUI _gameUI;
        private GameDataSO _gameData;

        public void Initialize()
        {
            _currenState = States.Player1Scan;
            _targetsController = ServiceLocator.Get<TargetsController>();
            _gameUI = ServiceLocator.Get<GameUI>();
            _gameUI.SetTurn(0);
            _gameData = ServiceLocator.Get<GameDataSO>();
            _targetsController.OnCardTargetFound += OnCardFound;
            _targetsController.OnCardTargetLost += OnCardLost;
            _targetsController.OnCharacterTargetFound += OnCharacterFound;
            _targetsController.OnCharacterTargetLost += OnCharacterLost;
            _turn = 0;
        }

        private void OnCharacterLost(CharacterSO characterSO, ObserverBehaviour character)
        {
            
        }

        private void OnCharacterFound(CharacterSO characterSO, ObserverBehaviour character)
        {
            if (_currenState == States.Player1Scan)
            {
                var copyCharacteristics = GetCopyCharacteristicsCopy(characterSO);
                _currenState = States.Player2Scan;
                _gameUI.SetTurn(1);
                _characterDatas.Add(new CharacterData(characterSO, character, copyCharacteristics));
                _gameUI.SetPlayerData(0, characterSO.TargetTexture, 
                    (int)copyCharacteristics[CharacterConstants.CurrentHealth],
                    (int)copyCharacteristics[CharacterConstants.MaxHealth],
                    (int)copyCharacteristics[CharacterConstants.CurrentMana],
                    (int)GetCurrentMana());
            }
            else if (_currenState == States.Player2Scan && _characterDatas.All(x => x.Character != characterSO))
            {
                var copyCharacteristics = GetCopyCharacteristicsCopy(characterSO);
                _currenState = States.Player1Turn;
                _gameUI.SetTurn(0);
                _characterDatas.Add(new CharacterData(characterSO, character, copyCharacteristics));
                _gameUI.SetPlayerData(1, characterSO.TargetTexture, 
                    (int)copyCharacteristics[CharacterConstants.CurrentHealth],
                    (int)copyCharacteristics[CharacterConstants.MaxHealth],
                    (int)copyCharacteristics[CharacterConstants.CurrentMana],
                    (int)GetCurrentMana());
                _gameUI.SetAttackButton(true);
            }
        }

        private CharacteristicsData GetCopyCharacteristicsCopy(CharacterSO characterSO)
        {
            var copyCharacteristics = characterSO.CopyCharacteristics();
            copyCharacteristics[CharacterConstants.CurrentHealth] = copyCharacteristics[CharacterConstants.MaxHealth];
            copyCharacteristics[CharacterConstants.CurrentMana] = GetCurrentMana();
            return copyCharacteristics;
        }

        private float GetCurrentMana()
        {
            var manaProgression = _gameData.ManaProgression;
            return manaProgression[Mathf.Min(_turn, manaProgression.Count - 1)];
        }

        HashSet<CardSO> _usedCards = new HashSet<CardSO>();
        private void OnCardLost(CardSO obj)
        {
            
        }

        private void OnCardFound(CardSO obj)
        {
            if (_usedCards.Contains(obj))
            {
                _gameUI.ShowCardUsedMessage(obj);
            }
            else
            {
                int whosTurn = _currenState == States.Player1Turn ? 0 : 1;
                if ((int)obj.Characteristics[CharacterConstants.Mana] >
                    (int)_characterDatas[whosTurn].Characteristics[CharacterConstants.CurrentMana])
                {
                    _gameUI.ShowNotEnoughMana();
                }
                else
                {
                    var card = obj;
                    _gameUI.ShowUseCardMessage(card, () =>
                    {
                        _usedCards.Add(card);
                        UseCard(card);
                        RefreshHud();
                    });
                }
            }
        }

        private void UseCard(CardSO obj, Action callback = null)
        {
            int whosTurn = _currenState == States.Player1Turn ? 0 : 1;
            int enemy = _currenState == States.Player2Turn ? 0 : 1;
            var playerController = _characterDatas[whosTurn].GetComponent<CharacterController>();
            var enemyController = _characterDatas[enemy].GetComponent<CharacterController>();
            playerController.PlaySound(obj.PlayerSound);
            playerController.PlayVfx(obj.PlayerVfx, null);
            _gameUI.SetAttackButton(false);
            playerController.PlayAnimation(obj.PlayerAnimation,()=>
            {
                enemyController.PlayVfx(obj.EnemyVfx, null);
                enemyController.PlayAnimation(obj.EnemyAnimation, null);
                playerController.PlaySound(obj.EnemySound);
                if (obj.CardLogic != null)
                {
                    obj.CardLogic.Execute(_characterDatas[whosTurn].Characteristics,
                        _characterDatas[enemy].Characteristics,
                        obj.Characteristics);
                }
                callback?.Invoke();
                RefreshHud();
                _gameUI.SetAttackButton(true);
            });
            
            if (obj != _gameData.DeathCard)
            {
                CheckWin();
            }
        }

        public void RefreshHud()
        {
            _gameUI.SetTurn(_currenState == States.Player1Turn ? 0 : 1);
            for (int i = 0; i < 2; i++)
            {
                var data = _characterDatas[i];
                data.Characteristics[CharacterConstants.CurrentHealth] =
                    Mathf.Min(data.Characteristics[CharacterConstants.CurrentHealth], data.Characteristics[CharacterConstants.MaxHealth]);
                
                _gameUI.SetPlayerData(i, null, 
                    (int)data.Characteristics[CharacterConstants.CurrentHealth],
                    (int)data.Characteristics[CharacterConstants.MaxHealth],
                    (int)data.Characteristics[CharacterConstants.CurrentMana],
                    (int)GetCurrentMana());
            }
        }

        // Update is called once per frame
        void Update()
        {
            ProcessState();
        }

        private void ProcessState()
        {
            switch (_currenState)
            {
                case States.Player1Scan:
                    break;
                case States.Player2Scan:
                    break;
                case States.Player1Turn:
                    break;
                case States.Player2Turn:
                    break;
            }
        }

        public void Attack()
        {
            if (_currenState != States.Player1Turn && _currenState != States.Player2Turn)
                return;
            
            int whosTurn = _currenState == States.Player1Turn ? 0 : 1;
            int enemy = _currenState == States.Player2Turn ? 0 : 1;
            var playerData = _characterDatas[whosTurn].Characteristics;
            var enemyData = _characterDatas[enemy].Characteristics;
            _gameUI.SetAttackButton(false);
            UseCard(_gameData.AttackCard, () =>
            {
                _gameUI.SetAttackButton(true);
                enemyData[CharacterConstants.CurrentHealth] -=
                    Random.Range(playerData[CharacterConstants.MinAttack],
                        playerData[CharacterConstants.MaxAttack]);
                NextTurn();
                CheckWin();
            });
        }

        public void NextTurn()
        {
            IncreaseTurnValue();
            _currenState = _currenState == States.Player1Turn ? States.Player2Turn : States.Player1Turn;
            RefreshHud();
        }

        private void IncreaseTurnValue()
        {
            _turn += _currenState == States.Player1Turn ? 0 : 1;
            for (int i = 0; i < 2; i++)
            {
                _characterDatas[i].Characteristics[CharacterConstants.CurrentMana] = GetCurrentMana();
            }
        }

        private void CheckWin()
        {
            for (int i = 0; i < 2; i++)
            {
                if (_characterDatas[i].Characteristics[CharacterConstants.CurrentHealth] <= 0)
                {
                    _gameUI.ShowWinMessage(i == 0 ? 1 : 0, RestartGame);
                    UseCard(_gameData.DeathCard,null);
                    _currenState = States.End;
                    _turn = 0;
                    _usedCards.Clear();
                    _characterDatas.Clear();
                    return;
                }
            }
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public struct CharacterData
    {
        public CharacterSO Character;
        public ObserverBehaviour Observer;
        public CharacteristicsData Characteristics;

        public CharacterData(CharacterSO characterSo, ObserverBehaviour character, CharacteristicsData copyCharacteristics)
        {
            Character = characterSo;
            Observer = character;
            Characteristics = copyCharacteristics;
        }

        public T GetComponent<T>()
        {
            return Observer.transform.GetChild(0).GetComponent<T>();
        }
    }

    public enum States
    {
        Player1Scan, Player2Scan, Player1Turn, Player2Turn, End
    }
}
