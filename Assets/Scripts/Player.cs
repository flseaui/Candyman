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
    
    [SerializeField]
    private float _speed;

    private float _horiz, _vert;
    private float _moveLimiter = 0.7f;

    [CanBeNull] private IWeapon _leftHand, _rightHand;

    private MonoBehaviour LeftHand => _leftHand as MonoBehaviour;
    private MonoBehaviour RightHand => _rightHand as MonoBehaviour;

    [SerializeField] private GameObject _candyCanePrefab;
    
    private void Awake()
    {
        _itemToWeapon = new Dictionary<string, IWeapon>
        {
            ["candy_cane"] = _candyCanePrefab.GetComponent<IWeapon>()
        };
        
        _rb = GetComponent<Rigidbody2D>();
        InventoryUI.SelectorMoved += () =>
        {
            SetHand(LRSelector.SelectorPos == 0, _inventory.SelectedItem.Sprite == null ? null :_itemToWeapon[_inventory.SelectedItem.Sprite.name]);
        };

    }

    public bool LeftLeg, RightLeg, LeftArm, RightArm, Head;

    private Dictionary<string, IWeapon> _itemToWeapon;
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
            LeftHand.transform.position = new Vector3(0.5f, 0.5f, 0.0f);
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
            RightHand.transform.position = new Vector3(0.5f, -0.3f, 0.0f);
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
    }

    public void FixedUpdate()
    {
        if (_horiz != 0 && _vert != 0)
        {
            _horiz *= _moveLimiter;
            _vert *= _moveLimiter;
        }
        
        _rb.velocity = new Vector2(_horiz * _speed, _vert * _speed) * Time.fixedDeltaTime;
    }
}
