using System;
using System.Collections;
using UnityEngine;
using Vuforia;

namespace ArFight.Scripts
{
    public class TargetsController : MonoBehaviour
    {
        private GameDataSO _gameDataSo;
        public event Action<CardSO> OnCardTargetFound;
        public event Action<CardSO> OnCardTargetLost;
        
        public event Action<CharacterSO, ObserverBehaviour> OnCharacterTargetFound;
        public event Action<CharacterSO, ObserverBehaviour> OnCharacterTargetLost;
        
        public void Start()
        {
            // yield return new WaitForEndOfFrame();
            _gameDataSo = ServiceLocator.Get<GameDataSO>();
            VuforiaApplication.Instance.OnVuforiaStarted += InstantiateTargets;
        }
        
        public void OnDestroy()
        {
            VuforiaApplication.Instance.OnVuforiaStarted -= InstantiateTargets;
        }

        private void InstantiateTargets()
        {
            foreach (var character in _gameDataSo.Characters)
            {
                var targetController = CreateImageTargetFromSideloadedTexture(character.TargetTexture, character.Name);
                targetController.gameObject.name = character.Name;
                var go = GameObject.Instantiate(character.Prefab);
                go.transform.parent = targetController.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one * 0.1f;
                targetController.OnTargetStatusChanged += TargetStatusChanged;
            }

            foreach (var card in _gameDataSo.Cards)
            {
                var targetController = CreateImageTargetFromSideloadedTexture(card.TargetTexture, card.Name);
                targetController.gameObject.name = card.Name;
                targetController.OnTargetStatusChanged += TargetStatusChanged;
            }
        }

        private void TargetStatusChanged(ObserverBehaviour observer, TargetStatus status)
        {
            ProcessCardStatus(observer, status);
            ProcessCharacterStatus(observer, status);
        }

        private void ProcessCharacterStatus(ObserverBehaviour observer, TargetStatus status)
        {
            var character = _gameDataSo.GetCharacterByName(observer.TargetName);
            var model = observer.transform.GetChild(0);
            model.gameObject.SetActive(status.Status == Status.TRACKED);
            if (status.Status != Status.TRACKED)
            {
                observer.transform.position = Vector3.zero;
            }

            if (character != null)
            {
                switch (status.Status)
                {
                    case Status.NO_POSE:
                        break;
                    case Status.LIMITED:
                        break;
                    case Status.TRACKED:
                        OnCharacterTargetFound?.Invoke(character, observer);
                        Debug.Log(character.Name + " " + character.TargetTexture.name + " found");
                        break;
                    case Status.EXTENDED_TRACKED:
                        OnCharacterTargetLost?.Invoke(character, observer);
                        Debug.Log(character.Name + " " + character.TargetTexture.name + " lost");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ProcessCardStatus(ObserverBehaviour observer, TargetStatus status)
        {
            var card = _gameDataSo.GetCardByName(observer.TargetName);
            if (card != null)
            {
                switch (status.Status)
                {
                    case Status.NO_POSE:
                        break;
                    case Status.LIMITED:
                        break;
                    case Status.TRACKED:
                        OnCardTargetFound?.Invoke(card);
                        Debug.Log(card.Name + " " + card.TargetTexture.name + " found");
                        break;
                    case Status.EXTENDED_TRACKED:
                        OnCardTargetLost?.Invoke(card);
                        Debug.Log(card.Name + " " + card.TargetTexture.name + " lost");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        ImageTargetBehaviour CreateImageTargetFromSideloadedTexture(Texture2D textureFile, string name)
        {
            var mTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
                textureFile,
                0.1f,
                name);
            // add the Default Observer Event Handler to the newly created game object
            mTarget.gameObject.AddComponent<DefaultObserverEventHandler>();
            Debug.Log("Instant Image Target created " + mTarget.TargetName);
            return mTarget;
        }
        
    }
}