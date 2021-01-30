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
    
    private CharacterController _character;

    private void Start()
    {
        _character = _player.gameObject.GetComponent<CharacterController>();
        _player.Jumped.AddListener(Jumped);
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _character.velocity.magnitude * _speedModifier);
    }

    private void Jumped()
    {
        _animator.SetTrigger("Jump");
    }
}
