using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _interactionVisual;
    [SerializeField]
    private Transform _interactionRoot;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private bool _disableAfterTrigger = false;
    [SerializeField]
    private bool _deadOnly = false;

    public UnityEvent<double, Player> OnInteracted = new UnityEvent<double, Player>();
    public UnityEvent<bool> OnCanInteractChanged = new UnityEvent<bool>();

    public bool _isInInteractionRange = false;
    [HideInInspector]
    public bool CanInteract = true;

    public void Awake()
    {
        SetCanInteract(true);
        _interactionVisual.SetActive(false);
        _isInInteractionRange = false;
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
        if (_isInInteractionRange && CanInteract && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
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
        if (controller.PhotonView.Owner.IsLocal)
        {
            var hasRole = GameManager.Instance.PlayerRoles.ContainsKey(controller.photonView.Controller);
            var canInteract = _deadOnly && hasRole ? CanInteract && controller.Role != PlayerRole.Human : CanInteract;
            _interactionVisual.SetActive(canInteract);
            _isInInteractionRange = true;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (!GetPlayerControllerInCollision(collision, out var controller)) return;
        if (controller.PhotonView.Owner.IsLocal)
        {
            _interactionVisual.SetActive(false);
            _isInInteractionRange = false;
        }
    }

    public void Interact()
    {
        photonView.RPC("ExecuteInteraction", RpcTarget.All, PhotonNetwork.Time, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void ExecuteInteraction(double timeStamp, int actorNumber)
    {
        var user = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        var hasRole = GameManager.Instance.PlayerRoles.ContainsKey(user);
        var canInteract = _deadOnly && hasRole ? CanInteract && GameManager.Instance.PlayerRoles[user] != PlayerRole.Human : CanInteract;
        if (!canInteract) return;
        if (user == null) return;
        OnInteracted.Invoke(timeStamp, user);

        if(_disableAfterTrigger)
            SetCanInteract(false);
    }

    /// <summary>
    /// Use for changing visual states of objects (toggle on/off)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="forceValue"></param>
    public void SetCanInteract(bool value, bool forceValue = false)
    {
        if (CanInteract == value && !forceValue) return;
        CanInteract = value;
        OnCanInteractChanged.Invoke(value);
    }
}