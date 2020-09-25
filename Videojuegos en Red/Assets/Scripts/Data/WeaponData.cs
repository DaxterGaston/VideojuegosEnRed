using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private float _rateOfFire;

    public float RateOfFire { get => _rateOfFire; set => _rateOfFire = value; }
}
