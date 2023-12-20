using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomCamera : MonoBehaviour
{
    public EventManager eventManager;
    
    private Transform _trackingTarget;
    private float _followSpeed;
    
    private float _cameraZoom;

    private bool _isPanning = false;
    private Vector3 _panScreenPointPrev;

    private void Awake()
    {
        _followSpeed = 1f;
        _cameraZoom = 5f;
    }
    
    private void Start()
    {
        eventManager.OnFollowSpeciesEvent += HandleFollowArgs;
        eventManager.OnUnfollowSpeciesEvent += HandleUnfollowArgs;
        eventManager.OnRemoveSpeciesEvent += HandleUnfollowGameObjectArgs;
    }
    private void Update()
    {
        if (_trackingTarget != null)
        {
            HandleFollow();
        }
        else
        {
            HandlePanningInput();
            HandleKeyBoardCameraMovement();
        }
        HandleZoomInput();
    }
    private void HandleKeyBoardCameraMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector3.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(Vector3.down);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector3.right);
        }
    }
    
    public void HandleFollowArgs(EventArgs args)
    {
        Follow(args.Object.transform);
    }
    
    public void HandleUnfollowArgs(EventArgs args)
    {
        Unfollow();
    }
    
    public void HandleUnfollowGameObjectArgs(EventArgs args)
    {
        UnfollowGameObject(args.Object);
    }
    
    public void Follow(Transform target)
    {
        _trackingTarget = target;
    }
    
    public void FollowGameObject(GameObject target)
    {
        Follow(target.transform);
    }
    
    public void UnfollowGameObject(GameObject target)
    {
        if (_trackingTarget == target.transform)
        {
            Unfollow();
        }
    }
    
    public void Unfollow()
    {
        _trackingTarget = null;
    }
    private void HandlePanningInput()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _isPanning = true;
            _panScreenPointPrev = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (Input.GetMouseButtonUp(2))
        {
            _isPanning = false;
        }

        if (!_isPanning) return;
        if (Camera.main == null) return;
        var currentScreenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var offset = _panScreenPointPrev - currentScreenPoint;
        var move = new Vector3(offset.x, offset.y, 0);
        if (float.IsInfinity(move.x) || float.IsInfinity(move.y)) return;
        if (float.IsNaN(move.x) || float.IsNaN(move.y)) return;
        transform.position += move;
        _panScreenPointPrev = currentScreenPoint;
    }
    private void HandleZoomInput()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0.0f) return;
        _cameraZoom -= scroll * 5;
        _cameraZoom = Mathf.Clamp(_cameraZoom, 1f, 10f);
        if (Camera.main != null) Camera.main.orthographicSize = _cameraZoom;
    }
    private void HandleFollow()
    {
        var myPosition = transform.position;
        var targetPosition = _trackingTarget.position;
        var xNew = Mathf.Lerp(myPosition.x, targetPosition.x, Time.deltaTime * _followSpeed);
        var yNew = Mathf.Lerp(myPosition.y, targetPosition.y, Time.deltaTime * _followSpeed);
        transform.position = new Vector3(xNew, yNew, myPosition.z);
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction;
    }

}