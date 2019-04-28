
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

    public void Damage(int damage) => _health -= damage;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        _player = GameObject.Find("Player");
        _health = 10;
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