using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using New;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private Inventory _inventory;
    [SerializeField] private Body _body;
    
    [SerializeField]
    private float _speed;

    private int _defense = 2;
    
    private float _horiz, _vert;
    private float _moveLimiter = 0.7f;

    [CanBeNull] private IWeapon _leftHand, _rightHand;

    private MonoBehaviour LeftHand => _leftHand as MonoBehaviour;
    private MonoBehaviour RightHand => _rightHand as MonoBehaviour;

    [SerializeField] private GameObject _candyCanePrefab;
    [SerializeField] private GameObject _mintPrefab;
    [SerializeField] private GameObject _shieldPrefab;
    
    private Animator _animator;

    [SerializeField] private GameObject _deathTextPrefab;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _itemToWeapon = new Dictionary<string, IWeapon>
        {
            ["leg"] = _candyCanePrefab.GetComponent<IWeapon>(),
            ["arm"] = _candyCanePrefab.GetComponent<IWeapon>(),
            ["head"] = _mintPrefab.GetComponent<IWeapon>(),
            ["torso"] = _shieldPrefab.GetComponent<IWeapon>(),
            ["empty"] = null
        };
        
        _rb = GetComponent<Rigidbody2D>();
        InventoryUI.SelectorMoved += () =>
        {
            SetHand(LRSelector.SelectorPos == 0, _itemToWeapon[_inventory.SelectedItem.Name]);
        };
    }

    private Dictionary<string, IWeapon> _itemToWeapon;
    private static readonly int Walking = Animator.StringToHash("walking");

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _body.HealthUntilLoss -= _defense;
            if (_body.HealthUntilLoss <= 0)
                _body.BodyParts.RemoveAt(Random.Range(0, _body.BodyParts.Count));
            other.rigidbody.AddForce(-Vector2.right * 10, ForceMode2D.Impulse);
        }
    }

    public void SetHand(bool hand, IWeapon weapon)
    {
        if (hand)
        {
            if (_leftHand != null)
                if (LeftHand.gameObject != null)
                {
                    Destroy(LeftHand.gameObject);
                    _leftHand = null;
                   return;
                }

            if (weapon is null) return;
            if (!(weapon is MonoBehaviour)) return;

            _leftHand = Instantiate(((MonoBehaviour) weapon).gameObject, transform).GetComponent<IWeapon>();
            _leftHand.Hand = hand;
            LeftHand.transform.localPosition = new Vector3(0.5f, 0.5f, 0.0f);
            LeftHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else
        {
            if (_rightHand != null)
                if (RightHand.gameObject != null)
                {
                    Destroy(RightHand.gameObject);
                    _rightHand = null;
                    return;
                }

            if (weapon is null) return;
            if (!(weapon is MonoBehaviour)) return;

            _rightHand = Instantiate(((MonoBehaviour) weapon).gameObject, transform).GetComponent<IWeapon>();
            _rightHand.Hand = hand;
            RightHand.transform.localPosition = new Vector3(0.5f, -0.3f, 0.0f);
            RightHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }

    public void Update()
    {
        _horiz = Input.GetAxisRaw("Horizontal");
        _vert = Input.GetAxisRaw("Vertical");

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetMouseButtonDown(0))
        {
            _leftHand?.Use();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _rightHand?.Use();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= 1.5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= 1.5f;
        }

        // DEATH
        if (_body.BodyParts.Count <= 0)
        {
            gameObject.SetActive(false);
            Instantiate(_deathTextPrefab, GameObject.Find("Canvas").transform);
        }

        var speed = 0.0f;
        var defense = 4;
        foreach (var part in _body.BodyParts)
        {
            switch (part.Name)
            {
                case "leg":
                    speed += 100;
                    break;
                case "torso":
                    defense -= 2;
                    break;
                case "arm":
                    //if (part.BodyPartName == "left_arm")
                        //SetHand(true, null);
                   // else
                        //SetHand(false, null);
                    break;
            }
        }

        _speed = speed;
        _defense = defense;

    }

    public void FixedUpdate()
    {
        if (_horiz != 0 && _vert != 0)
        {
            _horiz *= _moveLimiter;
            _vert *= _moveLimiter;
        }
        
        if (_vert != 0 || _horiz != 0)
            _animator.SetBool(Walking, true);
        else
            _animator.SetBool(Walking, false);
        
        _rb.velocity = new Vector2(_horiz * _speed, _vert * _speed) * Time.fixedDeltaTime;
    }
}
