using _Scripts.Inputs;
using UnityEngine;

namespace _Scripts.UI
{
    public class MudPowerUpUI : PowerUpButton
    {
        public void SelectMudPu()
        {
            InputHandler.MudPuSelected = true;
        }

        protected override void Awake()
        {
            base.Awake();
            //Button.onClick.AddListener(MainCanvas.Instance.MudPowerUpUsed);
        }
    }
}