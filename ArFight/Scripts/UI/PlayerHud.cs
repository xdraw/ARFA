using UnityEngine;
using UnityEngine.UI;

namespace ArFight.Scripts
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField]
        private ProgressBar _healthBar;

        [SerializeField]
        private ProgressBar _manaBar;

        [SerializeField]
        private Image _avatar;

        public void SetHealth(int current, int max)
        {
            _healthBar.SetProgress(Mathf.Max(0, current), max);
        }

        public void SetMana(int current, int max)
        {
            _manaBar.SetProgress(current, max);
            _manaBar.gameObject.SetActive(max > 0);
        }

        public void SetAvatar(Texture2D avatar)
        {
            _avatar.sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), Vector2.zero);
        }

        public void SetData(Texture2D avatar, int health, int maxHealth, int mana, int maxMana)
        {
            if (avatar != null)
                SetAvatar(avatar);
            SetHealth(health, maxHealth);
            SetMana(mana, maxMana);
        }
    }
}