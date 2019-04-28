/*using System;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class BodypartMap : MonoBehaviour
{
    private Animator _animator;
    private Player _player;
    
    [SerializeField] private Button _leftLegButton,
        _rightLegButton,
        _leftArmButton,
        _rightArmButton,
        _headButton;

    private static readonly int Zoom = Animator.StringToHash("zoom");

    [SerializeField] private GameObject _candyCanePrefab;
    [SerializeField] private Sprite _plusButton;

    public bool HasBodyPart(int bodyPart)
    {
        switch (bodyPart)
        {
            case 0:
                return _leftLegButton.IsActive() && _leftLegButton.GetComponent<Image>().sprite != _plusButton;
            case 1:
                return _rightLegButton.IsActive() && _rightLegButton.GetComponent<Image>().sprite != _plusButton;
            case 2:
                return _leftArmButton.IsActive() && _leftArmButton.GetComponent<Image>().sprite != _plusButton;
            case 3:
                return _rightArmButton.IsActive() && _rightArmButton.GetComponent<Image>().sprite != _plusButton;
            case 4:
                return _headButton.IsActive() && _headButton.GetComponent<Image>().sprite != _plusButton;
        }

        return false;
    }

    // 0 - leftLeg, 1 - rightLeg, 2 - leftArm, 3 - rightArm, 4 - head
    public void UseBodypart(int bodyPart)
    {
        switch (bodyPart)
        {
            case 0:
                _leftLegButton.gameObject.SetActive(false);
                _player.LeftLeg = false;
                break;
            case 1:
                _rightLegButton.gameObject.SetActive(false);
                _player.RightLeg = false;
                break;
            case 2:
                if (_player.HasItem(_candyCanePrefab.GetComponent<IWeapon>()))
                    _leftArmButton.GetComponent<Image>().sprite = _plusButton;
                else
                    _leftArmButton.gameObject.SetActive(false);

                _player.LeftArm = false;
                _player.SetSlot(Selector.SelectorPos, _candyCanePrefab.GetComponent<IWeapon>());
                break;
            case 3:
                if (_player.HasItem(_candyCanePrefab.GetComponent<IWeapon>()))
                    _rightArmButton.GetComponent<Image>().sprite = _plusButton;
                else
                    _rightArmButton.gameObject.SetActive(false);

                _player.RightArm = false;
                _player.SetSlot(Selector.SelectorPos, _candyCanePrefab.GetComponent<IWeapon>());
                break;
            case 4:
                _headButton.gameObject.SetActive(false);
                _player.Head = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(bodyPart), bodyPart, null);
        }
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _animator.SetBool(Zoom, true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            _animator.SetBool(Zoom, false);
        }
    }
}*/