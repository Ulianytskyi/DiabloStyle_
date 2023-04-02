using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class ClickToMove : MonoBehaviour
    {
        [Header("Stats")]
        public float attackDistance;
        public float attackRate;
        private float _nextAttack;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private Transform _targetedEnemy;
        private bool _enemyClicked;
        private bool _walking;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
        
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetButtonDown("Fire2"))
            {
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.collider.CompareTag($"Enemy"))
                    {
                        _targetedEnemy = hit.transform;
                        _enemyClicked = true;
                        Debug.Log("ENEMY HITTED");
                    }
                    else
                    {
                        _walking = true;
                        _enemyClicked = false;
                        _navMeshAgent.isStopped = false;
                        _navMeshAgent.destination = hit.point;
                    }
                }
            }

            if (_enemyClicked)
            {
                MoveAndAttack();
            }

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _walking = false;
            }
            else
            {
                _walking = true;
            }
            //_animator.SetBool("isWalking", _walking);
        }

        void MoveAndAttack()
        {
            if (_targetedEnemy == null)
            {
                return;
            }

            _navMeshAgent.destination = _targetedEnemy.position;

            if (_navMeshAgent.remainingDistance > attackDistance)
            {
                _navMeshAgent.isStopped = false;
                _walking = true;
            }
            else
            {
                transform.LookAt(_targetedEnemy);
                Vector3 dirToAttack = _targetedEnemy.transform.position - transform.position;

                if (Time.time > _nextAttack)
                {
                    _nextAttack = Time.time + attackRate;
                }

                _navMeshAgent.isStopped = true;
                _walking = false;
            }
        }
    }
}
