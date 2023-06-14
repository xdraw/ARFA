using TMPro;
using UnityEngine;

namespace ArFight.Scripts
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private RectTransform _filler;

        [SerializeField]
        private bool _isMirrored = false;

        public void SetProgress(int current, int max)
        {
            if (_isMirrored)
            {
                _filler.anchorMin = new Vector2(1 - (float)current / (float)max, 0);
                _filler.anchorMax = Vector2.one;
            }
            else
            {
                _filler.anchorMin = Vector2.zero;
                _filler.anchorMax = new Vector3((float)current / (float)max, 1);
            }
            _filler.sizeDelta = Vector2.zero;
            _filler.anchoredPosition = Vector2.zero;
            _text.text = $"{current}/{max}";
        }
    }
}