using System;
using UnityEngine;

namespace ArFight.Scripts.Game
{
    
    [RequireComponent(typeof(AudioSource))]
    public class SoundsController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _buttonSound;
        [SerializeField]
        private AudioClip _instantMessage;
        
        private AudioSource _audioSource;
        public void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayButtonSound()
        {
            _audioSource.PlayOneShot(_buttonSound);
        }

        public void PlayInstantMessageSound()
        {
            _audioSource.PlayOneShot(_instantMessage);
        }
    }
}