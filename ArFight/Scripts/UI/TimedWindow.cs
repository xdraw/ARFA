using System;
using TMPro;
using UnityEngine;

namespace ArFight.Scripts
{
    public class TimedWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private Action _action;

        public void Show(string text, float time, Action onHideAction)
        {
            _text.text = text;
            _action = onHideAction;
            gameObject.SetActive(true);
            Invoke(nameof(Hide), time);
        }
        
        private void Hide()
        {
            gameObject.SetActive(false);
            _action?.Invoke();
        }
    }
}