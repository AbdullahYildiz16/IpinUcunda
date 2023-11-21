using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private Image ButtonImage;
        [SerializeField] private Sprite enabledSprite, disabledSprite;
        private AudioSettings settings;

        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(SoundButtonClicked);
        }

        private void SoundButtonClicked()
        {
            if (SoundManager.Instance.IsActive)
            {
                SoundManager.Instance.IsActive = false;
                SoundManager.Instance.StopAudios();
                ButtonImage.sprite = disabledSprite;
            }
            else
            {
                SoundManager.Instance.IsActive = true;
                ButtonImage.sprite = enabledSprite;
            }
        }
    }
}