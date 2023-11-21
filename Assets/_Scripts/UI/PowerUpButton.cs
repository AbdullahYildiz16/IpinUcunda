using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class PowerUpButton : MonoBehaviour
    {
        public Image FillImage;

        [HideInInspector] public Button Button;

        protected virtual void Awake()
        {
            Button = GetComponent<Button>();
        }
    }
}