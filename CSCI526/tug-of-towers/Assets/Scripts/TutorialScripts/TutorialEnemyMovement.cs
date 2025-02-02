using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D trb;


    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        target = TutorialLevelManager.main.tpath[pathIndex];
    }
    private void Update()
    {
        if(Vector2.Distance(target.position, transform.position)<= 0.1f)
        {
            pathIndex++;
            if (pathIndex == TutorialLevelManager.main.tpath.Length)
            {
                TutorialLevelManager.main.EnemyReachedEndPoint();

                TutorialEnemySpawner.onTEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = TutorialLevelManager.main.tpath[pathIndex];
            }
        }

        
    }
    private void FixedUpdate()
    {
        Vector2 tdirection = (target.position - transform.position).normalized;

        trb.velocity = tdirection * moveSpeed;
    }
}
