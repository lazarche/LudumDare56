using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public float projectileSpeed = 10f;

    protected override void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().damage = attackDamage;

        Vector3 direction = (player.position - launchPoint.position);
        direction.y = 0;
        Vector3 horizontalDirection = direction.normalized;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 velocity = horizontalDirection * projectileSpeed;
        velocity.y = CalculateVerticalVelocity(player.position);

        rb.velocity = velocity;
    }

    float CalculateVerticalVelocity(Vector3 targetPosition)
    {
        float verticalDistance = targetPosition.y - launchPoint.position.y;

        float horizontalDistance = Vector3.Distance(new Vector3(targetPosition.x, 0, targetPosition.z), new Vector3(launchPoint.position.x, 0, launchPoint.position.z));
        float time = horizontalDistance / projectileSpeed;

        return (verticalDistance + 0.5f * Mathf.Abs(Physics.gravity.y) * time * time) / time;
    }
}
