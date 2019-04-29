using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _mintProjectilePrefab;
    
    public void Use()
    {
        Instantiate(_mintProjectilePrefab, transform.position, transform.parent.rotation);
    }

    public bool Hand { get; set; }
    public int SpriteIndex => 1;
}