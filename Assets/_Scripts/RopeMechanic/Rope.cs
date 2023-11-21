using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Inputs;
using _Scripts.UI;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Scripts.RopeMechanic
{
    public class Rope : MonoBehaviour
    {
        #region Singleton

        public static Rope Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public bool IsActive = true;

        private List<RopePuller> _leftRopePullers = new List<RopePuller>();
        private List<RopePuller> _rightRopePullers = new List<RopePuller>();

        public float balance;

        [SerializeField] private List<AudioClip> ropeLoops, ropeClips;
        [SerializeField] private Slider balanceBarSlider;
        [SerializeField] private GameObject favorLeftGo, favorRightGo;
        [SerializeField] private float ropePullDelay = 1f, loseBalance, ropePullMultiplier;
        [SerializeField] private GameObject finishPanel, humanWinPanel, robotWinPanel;
        private float _lastTimeRopePulled;

        private void Start()
        {
            var slots = GetComponentsInChildren<DragSlot>();
            foreach (var dragSlot in slots)
            {
                dragSlot.RopeDragSlot = true;
            }

            StartCoroutine(RandomRopeSoundRoutine());
        }

        private IEnumerator RandomRopeSoundRoutine()
        {
            while (IsActive)
            {
                yield return new WaitForSeconds(Random.Range(3f, 7f));
                if(!IsActive) yield break;
                SoundManager.Instance.PlayAudioClip(ropeLoops.GetRandom());
            }
        }

        private void Update()
        {
            if (_leftRopePullers.Count < 1 && _rightRopePullers.Count < 1) return;

            if (!IsActive) return;

            if (Time.time > _lastTimeRopePulled + ropePullDelay)
            {
                _lastTimeRopePulled = Time.time;
                var leftPullPower = CalculateLeftPullPower();
                var rightPullPower = CalculateRightPullPower();
                SetFavorVisual(leftPullPower, rightPullPower);

                if(_leftRopePullers.Count > 0) _leftRopePullers.GetRandom().PlayScream(Random.Range(0f, .25f));
                if(_rightRopePullers.Count > 0) _rightRopePullers.GetRandom().PlayScream(Random.Range(0f, .25f));
                SoundManager.Instance.PlayAudioClip(ropeClips.GetRandom());
                balance += rightPullPower - leftPullPower;
                balance = Mathf.Clamp(balance, -loseBalance, loseBalance);
                MainCanvas.Instance.AddScore((int)MathF.Abs(balance));
                StartCoroutine(transform.Move(balance * ropePullMultiplier * Vector3.right, ropePullDelay - 0.1f));
                //transform.position = balance * ropePullMultiplier * Vector3.right;
                UpdateBalanceBar();

                if (balance >= loseBalance || balance <= -loseBalance)
                {
                    GameObject.Find("Bottom Segment").SetActive(false);
                    favorLeftGo.SetActive(false);
                    favorRightGo.SetActive(false);

                    StartCoroutine(OpenFinishPanel(3));
                    if (balance > 0)    // rightist Win
                    {
                        foreach (var puller in FindObjectsOfType<DragObject>().Where(d => !d.Leftist))
                        {
                            puller.RopePuller.Win();
                        }
                        foreach (var puller in _leftRopePullers)
                        {
                            puller.Lose();
                        }
                        humanWinPanel.SetActive(false);
                        robotWinPanel.SetActive(true);
                    }
                    else                // leftist Win
                    {
                        foreach (var puller in FindObjectsOfType<DragObject>().Where(d => d.Leftist))
                        {
                            puller.RopePuller.Win();
                        }
                        foreach (var puller in _rightRopePullers)
                        {
                            puller.Lose();
                        }
                        humanWinPanel.SetActive(true);
                        robotWinPanel.SetActive(false);
                    }

                    SoundManager.Instance.GameOverSounds();
                    IsActive = false;
                    InputHandler.IsActive = false;
                }
            }
        }
        private IEnumerator OpenFinishPanel(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            finishPanel.SetActive(true);
        }
        private float CalculateLeftPullPower()
        {
            var power = _leftRopePullers.Sum(leftRopePuller => leftRopePuller.PullPower) + 15f;
            return power;
        }
        private float CalculateRightPullPower()
        {
            var power = _rightRopePullers.Sum(leftRopePuller => leftRopePuller.PullPower) + 15f;
            return power;
        }

        private void SetFavorVisual(float leftPullPower, float rightPullPower)
        {
            if (leftPullPower > rightPullPower)
            {
                favorLeftGo.SetActive(true);
                favorRightGo.SetActive(false);
            }
            else if (leftPullPower < rightPullPower)
            {
                favorLeftGo.SetActive(false);
                favorRightGo.SetActive(true);
            }
            else
            {
                favorLeftGo.SetActive(false);
                favorRightGo.SetActive(false);
            }
        }

        private void UpdateBalanceBar()
        {
            var endValue = Mathf.InverseLerp(0, loseBalance * 2, loseBalance + balance);
            StartCoroutine(SlideAnim(balanceBarSlider, balanceBarSlider.value, endValue, ropePullDelay - 0.2f));
            // balanceBarSlider.value = Mathf.InverseLerp(0, loseBalance * 2, loseBalance + balance);

            IEnumerator SlideAnim(Slider slider, float startValue, float endValue, float duration)
            {
                var elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > duration) elapsedTime = duration;
                    slider.value = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                }
            }
        }

        public void AddPullerToRope(RopePuller ropePuller, bool toLeft)
        {
            ropePuller.EnablePuller(this, toLeft);
            if (toLeft)
            {
                _leftRopePullers.Add(ropePuller);
            }
            else
            {
                _rightRopePullers.Add(ropePuller);
            }

            SetFavorVisual(CalculateLeftPullPower(), CalculateRightPullPower());
        }

        public void RemovePullerFromRope(RopePuller ropePuller, bool toLeft)
        {
            if (toLeft)
            {
                if (_leftRopePullers.Contains(ropePuller)) _leftRopePullers.Remove(ropePuller);
            }
            else
            {
                if (_rightRopePullers.Contains(ropePuller)) _rightRopePullers.Remove(ropePuller);
            }

            SetFavorVisual(CalculateLeftPullPower(), CalculateRightPullPower());
        }
    }
}