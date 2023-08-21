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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Finds all GameObjects with the tag "Enemy" and stores them in an array
        GameObject nearestEnemy = null;                                    // State the nearest enemy to be null
        float nearestDistance = Mathf.Infinity;                            // Declare float variable to calculate closest target

        // Check all of the GameOjects in array of enemies to find the nearest GameObject
        foreach (GameObject enemy in enemies)
        {
            // Calculates distance to enemy
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy-20 < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
