using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject _interactionVisual;
    [SerializeField]
    private Transform _interactionRoot;
    [SerializeField]
    private Camera _camera;

    public void Awake()
    {
        _interactionVisual.SetActive(false);
    }

    public void Update()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera != null)
            {
                _interactionRoot.forward = _camera.transform.forward;
            }
        }
        else
        {
            _interactionRoot.forward = _camera.transform.forward;
        }
    }

    public bool GetPlayerControllerInCollision(Collider collision, out PlayerController controller)
    {
        //VERYBAAD
        controller = collision.gameObject.GetComponent<PlayerController>();
        return controller != null;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (!GetPlayerControllerInCollision(collision, out var controller)) return;
        _interactionVisual.SetActive(controller.PhotonView.Owner.IsLocal);
    }

    public void OnTriggerExit(Collider collision)
    {
        if (!GetPlayerControllerInCollision(collision, out var controller)) return;
        _interactionVisual.SetActive(!controller.PhotonView.Owner.IsLocal);
    }

    public void Interact()
    {

    }
}