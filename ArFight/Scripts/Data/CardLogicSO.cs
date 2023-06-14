using UnityEngine;

namespace ArFight.Scripts
{
    [CreateAssetMenu(fileName = "CardLogic", menuName = "ScriptableObjects/CardLogic", order = 1)]
    public class CardLogicSO : ScriptableObject
    {
        public virtual void Execute(CharacteristicsData player, CharacteristicsData enemy, CharacteristicsData cardData)
        {
            var minAttack = cardData[CharacterConstants.MinAttack];
            var maxAttack = cardData[CharacterConstants.MinAttack];
            enemy[CharacterConstants.CurrentHealth] -= Random.Range(minAttack, maxAttack);
            player[CharacterConstants.CurrentHealth] += cardData[CharacterConstants.Heal];
            player[CharacterConstants.CurrentMana] -= cardData[CharacterConstants.Mana];
        }
    }
}