using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArFight.Scripts
{
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
    public class CharacterSO : ScriptableObject, IHasVuforiaTarget
    {
        public string Name;
        public GameObject Prefab;
        public Texture2D Target;
        public CharacteristicsData Characteristics;
        
        
        public Texture2D TargetTexture => Target;

        public string TargetName => Name;
        
        public void OnValidate()
        {
            if (Characteristics == null || Characteristics.IsEmpty())
            {
                Characteristics = new CharacteristicsData();
                Characteristics[CharacterConstants.MaxHealth] = 100;
                Characteristics[CharacterConstants.MinAttack] = 10;
                Characteristics[CharacterConstants.MaxAttack] = 20;
                Characteristics[CharacterConstants.AttackMultiplier] = 1;
            }
        }

        public CharacteristicsData CopyCharacteristics()
        {
            var characteristicsData = new CharacteristicsData();
            foreach (var param in Characteristics.Params)
            {
                characteristicsData.AddParam(new CharacteristicsParam(param.Key, param.Value));
            }
            return characteristicsData;
        }
    }

    [Serializable]
    public class CharacteristicsData
    {
        public List<CharacteristicsParam> Params;

        public bool IsEmpty()
        { 
            return Params == null || Params.Count == 0;
        }

        public void AddParam(CharacteristicsParam characteristicsParam)
        {
            if (Params == null)
            {
                Params = new List<CharacteristicsParam>();
            }
            Params.Add(characteristicsParam);
        }
        
        public float this[string key]
        {
            get
            {
                if (Params == null)
                {
                    return 0;
                }
                foreach (var param in Params)
                {
                    if (param.Key == key)
                    {
                        return param.Value;
                    }
                }
                return 0;
            }
            set
            {
                if (Params == null)
                {
                    Params = new List<CharacteristicsParam>();
                }
                foreach (var param in Params)
                {
                    if (param.Key == key)
                    {
                        param.Value = value;
                        return;
                    }
                }
                Params.Add(new CharacteristicsParam(key, value));
            }
        }
    }

    [Serializable]
    public class CharacteristicsParam
    {
        public string Key;
        public float Value;

        public CharacteristicsParam(string key, float value)
        {
            Key = key;
            Value = value;
        }
    }
}