using _Scripts.Inputs;
using UnityEngine;

namespace _Scripts.UI
{
    public class BananaPowerUpUI : PowerUpButton
    {
        public void SelectBananaPu()
        {
            InputHandler.BananaPuSelected = true;
        }

        protected override void Awake()
        {
            base.Awake();
            //Button.onClick.AddListener(MainCanvas.Instance.BananaPowerUpUsed);
        }
    }
}