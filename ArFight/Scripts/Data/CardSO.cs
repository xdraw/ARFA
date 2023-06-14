using UnityEngine;

namespace ArFight.Scripts
{
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
    public class CardSO : ScriptableObject, IHasVuforiaTarget
    {
        public string Name;
        public Texture2D Target;
        public CharacteristicsData Characteristics;
        public CardLogicSO CardLogic;

        public Texture2D TargetTexture => Target;

        public string TargetName => Name;

        public string PlayerAnimation;
        public GameObject PlayerVfx;
        public AudioClip[] PlayerSound;
        
        public string EnemyAnimation;
        public GameObject EnemyVfx;
        public AudioClip[] EnemySound;
    }
}
