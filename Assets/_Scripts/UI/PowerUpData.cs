using UnityEngine;

namespace _Scripts.UI
{
    [CreateAssetMenu(fileName = "PowerUpSo", menuName = "SOs/PowerUp", order = 0)]
    public class PowerUpData : ScriptableObject
    {
        public float MudPowerUpCooldown, BananaPowerUpCooldown;
    }
}