using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class MainCanvas : MonoBehaviour
    {
        #region Singleton

        public static MainCanvas Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        [SerializeField] private TMP_Text scoreText, finishScoreText1, finishScoreText2, bestScoreText1, bestScoreText2;
        [SerializeField] private PowerUpData powerUpData;
        [SerializeField] private PowerUpButton mudPuBtn, bananaPuBtn;

        [SerializeField] private List<AudioClip> mudClips, bananaClips;
        public GameObject creditsPanel;

        public bool MudPuReady => !_mudPuInCooldown;
        public bool BananaPuReady => !_bananaPuInCooldown;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MudPowerUpUsed();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                BananaPowerUpUsed();
            }
            if (_mudPuInCooldown)
            {
                if (Time.time > _lastTimeMudPuUsed + powerUpData.MudPowerUpCooldown)
                {
                    _mudPuInCooldown = false;
                    mudPuBtn.transform.GetChild(0).gameObject.SetActive(true);
                    mudPuBtn.Button.interactable = true;
                }
                else
                {
                    mudPuBtn.FillImage.fillAmount = Mathf.InverseLerp(0, powerUpData.MudPowerUpCooldown, Time.time - _lastTimeMudPuUsed);
                }
            }
            else
            {
                if(!mudPuBtn.transform.GetChild(0).gameObject.activeSelf) mudPuBtn.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (_bananaPuInCooldown)
            {
                if (Time.time > _lastTimeBananaPuUsed + powerUpData.BananaPowerUpCooldown)
                {
                    _bananaPuInCooldown = false;
                    bananaPuBtn.transform.GetChild(0).gameObject.SetActive(true);
                    bananaPuBtn.Button.interactable = true;
                }
                else
                {
                    bananaPuBtn.FillImage.fillAmount = Mathf.InverseLerp(0, powerUpData.BananaPowerUpCooldown, Time.time - _lastTimeBananaPuUsed);
                }
            }
            else
            {
                if(bananaPuBtn.transform.GetChild(0).gameObject.activeSelf) bananaPuBtn.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        private bool _mudPuInCooldown;
        private float _lastTimeMudPuUsed;
        public void MudPowerUpUsed()
        {
            mudPuBtn.Button.interactable = false;
            SoundManager.Instance.PlayAudioClip(mudClips.GetRandom());
            _lastTimeMudPuUsed = Time.time;
            _mudPuInCooldown = true;
        }

        private bool _bananaPuInCooldown;
        private float _lastTimeBananaPuUsed;
        public void BananaPowerUpUsed()
        {
            bananaPuBtn.Button.interactable = false;
            SoundManager.Instance.PlayAudioClip(bananaClips.GetRandom());
            _lastTimeBananaPuUsed = Time.time;
            _bananaPuInCooldown = true;
        }

        private int score;
        public void AddScore(int amount)
        {
            StartCoroutine(ScoreRoutine(amount, .4f));
            UpdateFinishPanelScores();
            // score += amount;
            // scoreText.text = score.ToString();

            IEnumerator ScoreRoutine(int scoreAdd, float duration)
            {
                var startScore = score;
                score += scoreAdd;
                var endScore = score;
                var elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > duration) elapsedTime = duration;
                    scoreText.text = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, elapsedTime / duration)).ToString();
                }
            }
        }
        private void UpdateFinishPanelScores()
        {
            finishScoreText1.text = score + "";
            finishScoreText2.text = score + "";
            if (score > PlayerPrefs.GetInt("best_score", 0))
            {
                PlayerPrefs.SetInt("best_score", score);

            }
            bestScoreText1.text = "BEST: " + PlayerPrefs.GetInt("best_score");
            bestScoreText2.text = "BEST: " + PlayerPrefs.GetInt("best_score");
        }
    }
}