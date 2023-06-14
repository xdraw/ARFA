using System;
using ArFight.Scripts.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArFight.Scripts
{
    public class DialogWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private Button _okButton;

        [SerializeField]
        private Button _cancelButton;
        
        [SerializeField]
        private TextMeshProUGUI _okButtonText;
        
        [SerializeField]
        private TextMeshProUGUI _cancelButtonText;

        public void Show(Action onOk, Action onCancel, string text, string okText, string cancelText)
        {
            ServiceLocator.Get<SoundsController>().PlayInstantMessageSound();
            _text.text = text;
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _okButton.onClick.AddListener(() =>
            {
                ServiceLocator.Get<SoundsController>().PlayButtonSound();
                onOk?.Invoke();
                Hide();
            });
            _cancelButton.onClick.AddListener(() =>
            {
                ServiceLocator.Get<SoundsController>().PlayButtonSound();
                onCancel?.Invoke();
                Hide();
            });
            gameObject.SetActive(true);
            _cancelButton.gameObject.SetActive(true);
        }

        public void Show(Action onOk, string text, string okText)
        {
            _cancelButton.gameObject.SetActive(false);
            _text.text = text;
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _okButton.onClick.AddListener(() =>
            {
                ServiceLocator.Get<SoundsController>().PlayButtonSound();
                onOk?.Invoke();
                Hide();
            });
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}