using System;
using _Scripts.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private Image ButtonImage;
        [SerializeField] private Sprite pausedSprite, continueSprite;

        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(PauseButtonClicked);
        }

        private void PauseButtonClicked()
        {
            if (Math.Abs(Time.timeScale - 1) < .1f) // game playing
            {
                Time.timeScale = 0;
                InputHandler.IsActive = false;
                ButtonImage.sprite = continueSprite;
                MainCanvas.Instance.creditsPanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                InputHandler.IsActive = true;
                ButtonImage.sprite = pausedSprite;
                MainCanvas.Instance.creditsPanel.SetActive(false);
            }
        }
    }
}