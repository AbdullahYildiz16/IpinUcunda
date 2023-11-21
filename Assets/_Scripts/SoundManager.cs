using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts
{
    public class SoundManager : MonoBehaviour
    {
        #region Singleton

        public static SoundManager Instance;

        #endregion

        public bool IsActive = true;
        public static bool AmbientSoundPlaying = true;
        [SerializeField] private int asCount;

        [SerializeField] private AudioSource ambientAudioSource;
        [SerializeField] private List<AudioClip> ambientClips;
        [SerializeField] private AudioClip winClip;
        private List<AudioSource> _sources = new List<AudioSource>();

        private void Awake()
        {
            Instance = this;

            for (int i = 0; i < asCount; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                _sources.Add(source);
            }
        }

        private IEnumerator Start()
        {
            AmbientSoundPlaying = true; // For editor options
            while (AmbientSoundPlaying)
            {
                ambientAudioSource.clip = ambientClips.GetRandom();
                if(IsActive) ambientAudioSource.Play();

                yield return new WaitUntil(() => !ambientAudioSource.isPlaying);
            }
        }

        public void PlayAudioClip(AudioClip clip)
        {
            if(!IsActive) return;

            var source = PullAudioSource();
            source.PlayOneShot(clip);
        }

        private int _sourceIdx;
        private AudioSource PullAudioSource()
        {
            _sourceIdx++;
            if (_sourceIdx >= _sources.Count) _sourceIdx -= _sources.Count;

            return _sources[_sourceIdx];
        }

        public void GameOverSounds()
        {
            StartCoroutine(DecreaseVolume(1f));
            AmbientSoundPlaying = false;
            PlayAudioClip(winClip);

            IEnumerator DecreaseVolume(float duration)
            {
                var startVol = ambientAudioSource.volume;
                var elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > duration) elapsedTime = duration;

                    ambientAudioSource.volume = Mathf.Lerp(startVol, 0, elapsedTime/duration);
                }
            }
        }

        public void StopAudios()
        {
            foreach (var audioSource in _sources)
            {
                if(audioSource.isPlaying) audioSource.Stop();
            }
            if(ambientAudioSource.isPlaying) ambientAudioSource.Stop();
        }
    }
}