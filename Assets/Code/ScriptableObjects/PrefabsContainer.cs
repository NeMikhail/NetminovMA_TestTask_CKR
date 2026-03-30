using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsContainer", menuName = "ScriptableObjects/PrefabsContainer", order = 0)]
public class PrefabsContainer : ScriptableObject
{
    public GameObject ScenePrefab;
    public GameObject DayButtonPrefab;
    public GameObject DogButtonPrefab;
    public GameObject ClickParticlePrefab;
    public GameObject CoinVFXPrefab;
}
