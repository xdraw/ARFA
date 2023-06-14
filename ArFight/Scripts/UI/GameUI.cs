using System;
using ArFight.Scripts.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArFight.Scripts
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerHud[] _playerHuds;
        
        [SerializeField]
        private DialogWindow _dialogWindow;

        [SerializeField]
        private Button _attackButton;
        
        [SerializeField]
        private Button _skipButton;

        [SerializeField]
        private TextMeshProUGUI _turnText;

        [SerializeField]
        private TimedWindow _timedWindow;
        
        public void Awake()
        {
            foreach (var playerHud in _playerHuds)
            {
                playerHud.gameObject.SetActive(false);
            }
            _dialogWindow.gameObject.SetActive(false);
            SetAttackButton(false);
            _attackButton.onClick.AddListener(() =>
            {
                ServiceLocator.Get<GameController>().Attack();
                ServiceLocator.Get<SoundsController>().PlayButtonSound();
            });
            _skipButton.onClick.AddListener(() =>
            {
                ServiceLocator.Get<GameController>().NextTurn();
            });
        }

        public void SetTurn(int player)
        {
            SetTurnTextVisibility(false);
            _turnText.text = $"Ход {player + 1} игрока";
            _timedWindow.Show(_turnText.text, 3f, ()=>SetTurnTextVisibility(true));
        }

        public void SetTurnTextVisibility(bool isVisible)
        {
            var transformParent = _turnText.transform.parent;
            transformParent.gameObject.SetActive(isVisible);
        }

        public void SetAttackButton(bool isActive)
        {
            _attackButton.gameObject.SetActive(isActive);
            _skipButton.gameObject.SetActive(isActive);
        }
        
        public void SetPlayerData(int id, Texture2D avatar, int health, int maxHealth, int mana, int maxMana)
        {
            _playerHuds[id].SetData(avatar, health, maxHealth, mana, maxMana);
            _playerHuds[id].gameObject.SetActive(true);
        }


        public void ShowCardUsedMessage(CardSO cardSo)
        {
            _dialogWindow.Show(null, $"Карта {cardSo.Name} уже была использована", "Ok");
        }

        public void ShowUseCardMessage(CardSO cardSo, Action action)
        {
            _dialogWindow.Show(action, null, $"Использовать карту {cardSo.Name}?", "Да", "Нет");
        }

        public void ShowNotEnoughMana()
        {
            _dialogWindow.Show(null, $"Недостаточно маны", "Ok");
        }

        public void ShowWinMessage(int i, Action restartGame)
        {
            SetAttackButton(false);
            SetTurnTextVisibility(false);
            _dialogWindow.Show(restartGame, Application.Quit, $"Игрок {i + 1} выиграл! Перезапустить игру?", "Да", "Нет");
        }
    }
}