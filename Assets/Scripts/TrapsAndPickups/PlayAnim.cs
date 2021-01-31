using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class PlayAnim : MonoBehaviour
{
    private Animation _anim;

    private void Start()
    {
         _anim = GetComponent<Animation>();
    }

    public void PlayAnimation()
    {
        _anim.Play();
    }
}
