using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAtNearestEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private float shootForce = 50f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire2")) // Change "Fire1" to your desired input
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            Vector3 shootDirection = (nearestEnemy.transform.position - firePoint.position).normalized;
            GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = shootDirection * shootForce;
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Tag your enemies appropriately
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
