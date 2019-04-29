using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage;
    public float Speed;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * Speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damage(Damage);
        }
    }
}