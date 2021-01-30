using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OnDeadSFX : MonoBehaviourPunCallbacks, IDied
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private List<AudioClip> _deathClips;

    private bool _dead = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnDeath()
    {
        if(_dead)
            return;

        _dead = true;
        int clipIndex = Random.Range(0, _deathClips.Count);
        _audioSource.PlayOneShot(_deathClips[clipIndex]);
    }
}