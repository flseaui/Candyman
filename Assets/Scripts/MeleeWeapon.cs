using System;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{   
    private static readonly int LeftHand = Animator.StringToHash("left_hand");
    private static readonly int RightHand = Animator.StringToHash("right_hand");
    private static readonly int HandBool = Animator.StringToHash("hand");
    
    private Animator _animator;

    [SerializeField] private int _damage;
    
    public bool Hand { get; set; } // True - Left, False - Right
    public int SpriteIndex => 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.SetBool(HandBool, Hand);
    }
    
    public void Use()
    {
        _animator.SetTrigger(Hand ? LeftHand : RightHand);
        Debug.Log("yayyy");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damage(_damage);
        }
    }
}
