using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //waiting "check",
    [SerializeField]
    bool _patrolWaiting;

    [SerializeField]
    float _totalWaitTime;

    //probability of switching direction
    [SerializeField]
    float _switchProbability;

    //list of all waypoints to visit
    [SerializeField]
    List<Waypoint> _patrolPoints;

    //private variables for base behaviour
    NavMeshAgent _navMeshAgent;
    int _currentPatrolIndex;
    bool _travelling;
    bool _waiting;
    bool _patrolForward;
    float _waitTimer;

    Animation _animation;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _animation = gameObject.GetComponent<Animation>();

        if (_patrolPoints != null && _patrolPoints.Count >= 2)
        {
            _currentPatrolIndex = 0;
            SetDestination();
        }
        else
            Debug.Log("Insufficient waypoints for basic patrolling behaviour.");
    }

    // Update is called once per frame
    void Update()
    {
        //"check" to see if it's close to the destination
        if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _travelling = false;

            //if going to wait -> wait
            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        //instead if waiting
        if (_waiting)
        {
            _animation.Play("idle");
            //transform.Rotate(new Vector3(0, 45, 0) * 2 * Time.deltaTime);

            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _waiting = false;
        
        if (_patrolPoints != null)
        {
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
            _animation.Play("Walk");
        }
    }

    private void ChangePatrolPoint()
    {
        _patrolForward = _currentPatrolIndex == 0 ? _patrolForward = true : (_currentPatrolIndex == _patrolPoints.Count - 1 ? _patrolForward = false : _patrolForward = _patrolForward);

        if (_patrolForward)
        {
            //_currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
            NextPatrolPoint();
        }
        else if (_patrolForward == false)
            PreviousPatrolPoint();
        else if (--_currentPatrolIndex < 0)
        {
            //_currentPatrolIndex = _patrolPoints.Count - 1;
            NextPatrolPoint();
        }
            
    }

    private void NextPatrolPoint()
    {
        ++_currentPatrolIndex;
    }

    private void PreviousPatrolPoint()
    {
        --_currentPatrolIndex;
    }

}