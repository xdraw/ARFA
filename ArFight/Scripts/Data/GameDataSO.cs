using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArFight.Scripts
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        public List<CharacterSO> Characters;
        public List<CardSO> Cards;
        public List<int> ManaProgression;

        public CardSO DeathCard;
        public CardSO AttackCard;

        public CardSO GetCardByName(string observerTargetName)
        {
            return Cards.FirstOrDefault(card => card.Name == observerTargetName);
        }

        public CharacterSO GetCharacterByName(string observerTargetName)
        {
            return Characters.FirstOrDefault(character => character.Name == observerTargetName);
        }
    }
}