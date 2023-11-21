using System.Collections;
using System.Collections.Generic;
using _Scripts.Inputs;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.RopeMechanic
{
    public class RopePuller : MonoBehaviour
    {
        public float PullPower => strength;

        [SerializeField] private float strength, durability;
        [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();
        [SerializeField] private List<AudioClip> _dieClips = new List<AudioClip>();
        private float _currentDurability;

        [SerializeField] private ParticleSystem _sweatPs;
        [SerializeField] private Image fillImage;
        [SerializeField] private Transform fillCanvas;

        private bool _isActive, _isLeftPuller; public bool IsLeftist => _isLeftPuller; public bool IsActive => _isActive;
        private Rope _attachedRope;
        private Animator _animator;
        private Camera _mainCam;
        private static readonly int Pull = Animator.StringToHash("Pull");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Victory = Animator.StringToHash("Victory");
        private static readonly int Random = Animator.StringToHash("Random");

        private void OnEnable()
        {
            _mainCam = Camera.main;
            _currentDurability = durability;
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!_isActive) return;

            _currentDurability -= Time.deltaTime;
            fillCanvas.rotation = Quaternion.LookRotation(_mainCam.transform.forward, _mainCam.transform.up);
            UpdateDurabilityFillImage();
            if (_currentDurability < 0)
            {
                _isActive = false;
                DisablePuller(); // Die Die Die
            }
        }

        public void DisablePuller()
        {
            SoundManager.Instance.PlayAudioClip(_dieClips.GetRandom());
            ParticleManager.Instance.PlayBlastParticle(transform.position + Vector3.up);
            _attachedRope.RemovePullerFromRope(this, _isLeftPuller);
            var dragObj = GetComponent<DragObject>();
            dragObj.Pick();
            StartCoroutine(Die());

            IEnumerator Die()
            {
                _animator.SetTrigger(Fall);
                yield return new WaitForSeconds(1f);
                gameObject.SetActive(false);
            }
        }

        public void EnablePuller(Rope rope, bool isLeft)
        {
            fillCanvas.gameObject.SetActive(true);
            _attachedRope = rope;
            _isActive = true;
            _isLeftPuller = isLeft;
            _animator.SetTrigger(Pull);
            if (_isLeftPuller)
            {
                _sweatPs.transform.position += Vector3.forward * -0.176f;
                _sweatPs.Play();
                _animator.transform.position += Vector3.forward * -0.176f;
                _animator.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            }
            else if (!_isLeftPuller)
            {
                _animator.transform.position += Vector3.forward * 0.45f;
                _animator.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
            }
        }

        private void UpdateDurabilityFillImage()
        {
            fillImage.fillAmount = Mathf.InverseLerp(0, durability, _currentDurability);
        }

        public void Weaken()
        {
            strength *= 0.5f;
        }

        public void PlayScream(float delay)
        {
            StartCoroutine(PlayDelayedScream());

            IEnumerator PlayDelayedScream()
            {
                yield return new WaitForSeconds(delay);
                SoundManager.Instance.PlayAudioClip(_clips.GetRandom());
            }
        }

        public void Win()
        {
            _isActive = false;
            fillCanvas.gameObject.SetActive(false);
            _animator.SetInteger(Random, UnityEngine.Random.Range(0,2));
            _animator.SetTrigger(Victory);
        }

        public void Lose()
        {
            _isActive = false;
            fillCanvas.gameObject.SetActive(false);
            _animator.SetTrigger(Fall);
        }
    }
}