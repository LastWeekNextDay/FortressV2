using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{
    private EventManager _eventManager;
    
    private NavMeshAgent _agent;
    private CellData _onCellData;
    private const float SlightDrift = 0.01f;

    public List<Vector3> PathNodes { get; set; }
    
    public float DefaultSpeed { get; set; }
    
    public CellData OnCellData
    {
        get => _onCellData;
        set
        {
            _onCellData = value;
            _agent.speed = DefaultSpeed * OnCellData.GroundInfo.SpeedModifier;
        }
    }

    private void Update()
    {
        if (_agent.speed > 0)
        {
            var velocity = _agent.velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (IsAtDestination())
        {
            PathNodes.Clear();
            Stop();
            _eventManager.InvokeReachDestinationEvent(new EventArgs
            {
                Object = gameObject
            });
        }
    }

    private void Awake()
    {
        PathNodes = new List<Vector3>();
        _eventManager = GameObject.Find("Game Manager").GetComponent<EventManager>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        // TODO: DefaultSpeed proper calculation using character stats
        _agent.speed = DefaultSpeed;
    }

    public void GoTo(Vector3 destination)
    {
        Resume();
        var driftPos = destination;
        if(Mathf.Abs(transform.position.x - destination.x) < SlightDrift) 
            driftPos = destination + new Vector3(SlightDrift, 0f, 0f);
        _agent.SetDestination(driftPos);
        PathNodes = _agent.path.corners.ToList();
        var args = new EventArgs
        {
            Object = gameObject,
            Vector3Coords = destination
        };
        _eventManager.InvokeMoveEvent(args);
    }
    
    public void GoAwayFrom(GameObject target)
    {
        Resume();
        var position = transform.position;
        var direction = position - target.transform.position;
        var destination = position + direction;
        GoTo(destination);
    }
    
    public bool IsAtDestination()
    {
        return _agent.remainingDistance <= _agent.stoppingDistance;
    }

    public void Stop()
    {
        _agent.speed = 0;
    }
    
    public void Resume()
    {
        _agent.speed = DefaultSpeed;
    }
}
