using UnityEngine;

public class Missile : Bullet
{
    [Header("Missile Attributes")]
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float acceleration = 1f;

    private float currentSpeed;

    private void Start()
    {
        bulletDamage *= 2.5f;
        currentSpeed = bulletSpeed;
        if (target != null)
        {
            SetInitialRotation();
        }
    }

    private void FixedUpdate()
    {
        if (!target) return;
        
        Vector2 direction = (target.position - transform.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = rb.rotation;
        
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, angle, rotationSpeed * Time.fixedDeltaTime);
        rb.rotation = newAngle;
        
        currentSpeed += acceleration * Time.fixedDeltaTime;
        rb.velocity = rb.transform.right * currentSpeed;
    }

    private void SetInitialRotation()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        rb.rotation = initialAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, initialAngle);
    }
}
