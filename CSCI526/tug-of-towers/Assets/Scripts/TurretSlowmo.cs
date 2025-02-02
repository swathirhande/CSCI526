using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurretSlowmo : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float aps = 4f;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float rotationSpeed = 100f;

    private float timeUntilFire;

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / aps)
            {
                GameObject iceRing = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                FreezeEnemies();
                timeUntilFire = 0f;
            }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if(hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }
}
