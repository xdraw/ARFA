using System;

namespace ArFight.Scripts.Game
{
    using UnityEngine;

    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        AudioSource _audioSource;

        private Action _onVfxFinished;
        private Action _onAnimationFinished;

        public void PlayAnimation(string triggerName, Action OnAnimationFinished)
        {
            _onAnimationFinished = OnAnimationFinished;
            //reset all triggers
            ResetAllTriggers();
            _animator.SetTrigger(triggerName);
            float animationLength = GetAnimationLength(triggerName);
            Invoke(nameof(InvokeAnimationFinishAction), animationLength);
        }

        private float GetAnimationLength(string triggerName)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            foreach(AnimationClip clip in clips)
            {
                if (clip.name == triggerName)
                {
                    return clip.length;
                }
            }

            return 0f;
        }

        public void PlaySound(AudioClip[] clips)
        {
            PlaySound(clips[UnityEngine.Random.Range(0, clips.Length)]);
        }

        public void PlaySound(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
        
        public void PlayVfx(GameObject vfx, Action OnVfxFinished)
        {
            if (vfx != null)
            {
                var vfxObject = GameObject.Instantiate(vfx, transform.position, Quaternion.identity);
                vfxObject.transform.SetParent(transform);
                vfxObject.transform.localPosition = Vector3.zero;
                vfxObject.transform.localScale = Vector3.one * 0.5f;
                var lifetime = vfxObject.GetComponent<ParticleSystem>().startLifetime;
                _onVfxFinished = OnVfxFinished;
                Destroy(vfxObject, lifetime);
                Invoke(nameof(InvokeVfxFinishAction), lifetime);
            }
            else
            {
                OnVfxFinished?.Invoke();
            }

        }
        
        private void InvokeVfxFinishAction()
        {
            _onVfxFinished?.Invoke();
        }
        
        private void InvokeAnimationFinishAction()
        {
            _onAnimationFinished?.Invoke();
        }
        
        private void ResetAllTriggers()
        {
            foreach (var param in _animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                {
                    _animator.ResetTrigger(param.name);
                }
            }
        }
        
    }
}