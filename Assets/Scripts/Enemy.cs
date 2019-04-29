
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static readonly int Eating = Animator.StringToHash("eating");

    [SerializeField] private float _range;
    [SerializeField] private float _speed;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private GameObject _player;

    private Vector2 _moveVec;
    private int _health;

    public void Damage(int damage)
    {
        _animator.SetTrigger("hit");
        _health -= damage;
        //_animator.ResetTrigger("hit");
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        _player = GameObject.Find("Player");
        _health = 10;
        var maxSpeed = 0;
        var minSpeed = 85;
        var minRange = 3;
        var maxRange = 20;
        if (GameData.Score > 200)
            maxSpeed = 200;
        if (GameData.Score > 400)
            maxSpeed = 225;
        if (GameData.Score > 500)
        {
            maxSpeed = 250;
            minSpeed = 110;
        }
        if (GameData.Score > 700)
        {
            maxSpeed = 275;
            minSpeed = 150;
        }
        if (GameData.Score > 1000)
        {
            maxSpeed = 300;
            minSpeed = 175;
        }

        _speed = Random.Range(minSpeed, maxSpeed);
        _range = Random.Range(minRange, maxRange);
    }

    private void OnDestroy()
    {
        // Death
        GameData.Score += 20;
    }

    private void Update()
    {
        if (_health <= 0)
            Destroy(gameObject);
        
        var position = transform.position;
        var up = Physics2D.Raycast(position, Vector2.up, 2);
        var down = Physics2D.Raycast(position, Vector2.down, 2);
        var left = Physics2D.Raycast(position, Vector2.left, 2);
        var right = Physics2D.Raycast(position, Vector2.right, 2);

        if (Vector2.Distance(position, _player.transform.position) < _range)
        {
            _animator.SetBool(Eating, true);
            _moveVec = _player.transform.position - position;
            
            if (up.transform != null && up.transform.CompareTag("Enemy"))
                _moveVec -= Vector2.up;

            if (down.transform != null && down.transform.CompareTag("Enemy"))
                _moveVec += Vector2.up;

            if (left.transform != null && left.transform.CompareTag("Enemy"))
                _moveVec += Vector2.right;

            if (right.transform != null && right.transform.CompareTag("Enemy"))
                _moveVec -= Vector2.right;
        }
        else
        {
            _animator.SetBool(Eating, false);
            _moveVec = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_moveVec.normalized * _speed, ForceMode2D.Force);
        var dir = _player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        var theta = 0f;
        const int size = (int)(1f / 0.01f + 1f);
        var lastVec = Vector3.zero;
        for(var i = 0; i < size; i++){          
            theta += 2.0f * Mathf.PI * 0.01f;         
            var x = _range * Mathf.Cos(theta);
            var y = _range * Mathf.Sin(theta);
            if (i == 0)
                lastVec = new Vector3(x, y, 0);
            Gizmos.DrawLine(transform.position + lastVec, transform.position + new Vector3(x, y, 0));
            lastVec = new Vector3(x, y,0);
        }
    }
}