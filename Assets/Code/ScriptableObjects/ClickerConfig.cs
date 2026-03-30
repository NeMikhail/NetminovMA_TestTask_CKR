using UnityEngine;

namespace Clicker
{
    [CreateAssetMenu(fileName = "ClickerConfig", menuName = "ScriptableObjects/ClickerConfig")]
    public class ClickerConfig : ScriptableObject
    {
        public int MaxEnergy = 1000;
        public int StartEnergy = 1000;
        public int EnergyPerClick = 1;
        public int EnergyPerAutoCollect = 1;
        public int EnergyRechargeAmount = 10;
        public float EnergyRechargeInterval = 10f;
        public int CurrencyPerClick = 1;
        public float AutoCollectInterval = 3f;
    }
}
