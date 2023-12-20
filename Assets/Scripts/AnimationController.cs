using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;

    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int Moving = Animator.StringToHash("Moving");

    // Start is called before the first frame update
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        _animator.SetBool(Moving, _agent.speed > 0.1f);
        _animator.SetFloat(MoveSpeed, _agent.speed);
    }

    public void PlayAttackAnimation(Vector2 target)
    {
        StartCoroutine(AttackAnimation(target));
    }

    private IEnumerator AttackAnimation(Vector2 target)
    {
        var originalSpeed = _agent.speed;
        _agent.speed = 100;
        _agent.SetDestination(target);
        yield return new WaitUntil(() => _agent.remainingDistance < 0.1f);
        _agent.SetDestination(transform.position);
        yield return new WaitUntil(() => _agent.remainingDistance < 0.1f);
        _agent.speed = originalSpeed;
    }
}
