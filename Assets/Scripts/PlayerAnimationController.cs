using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _speedModifier = 1;
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private float _animationSmooth = 10;
    
    private CharacterController _character;
    private float _velocityValue = 0;

    private void Start()
    {
        _character = _player.gameObject.GetComponent<CharacterController>();
        _player.Jumped.AddListener(Jumped);
    }

    private void Update()
    {
        _velocityValue = Mathf.Lerp(_velocityValue, _character.velocity.magnitude, _animationSmooth * Time.deltaTime);
        _animator.SetFloat("Speed", _velocityValue * _speedModifier);
    }

    private void Jumped()
    {
        _animator.SetTrigger("Jump");
    }
}
