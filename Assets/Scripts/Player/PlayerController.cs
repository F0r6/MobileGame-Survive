using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float radius = 10.0f;
    [SerializeField] private float distBetween;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float fireRate = 1f;

    private Transform nearestEnemy;
    private Transform previousNearEnemy;
    private float lastFireTime;

    private static readonly Color defaultColour = Color.white;
    private static readonly Color targetColour = Color.red;

    public LayerMask objLayerMask;

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(joyStick.Horizontal * moveSpeed, joyStick.Vertical * moveSpeed);
       
        FindNearestEnemy();
        TryFireProjectile();       
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        float closestDistance = Mathf.Infinity;
        Transform closestFound = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFound = enemy.transform;
            } 
        }

        // Clear highlights
        if (previousNearEnemy != null && previousNearEnemy != closestFound)
        {
            var prevRenderer = nearestEnemy.GetComponentInChildren<SpriteRenderer>();
            if (prevRenderer != null)
                prevRenderer.material.color = defaultColour;
        }

        nearestEnemy = closestFound;
        previousNearEnemy = closestFound;

        if (nearestEnemy != null)
        {
            var renderer = nearestEnemy.GetComponentInChildren<SpriteRenderer>();
            if (renderer != null)
                renderer.material.color = targetColour;
        }
    }

    private void TryFireProjectile()
    {
        if (projectilePrefab == null || nearestEnemy == null) 
            return;

        float distance = Vector2.Distance(transform.position, nearestEnemy.position);
        // If out of range
        if (distance > distBetween) 
            return;                         

        // Fire rate limit
        if (Time.time - lastFireTime < 1f / fireRate) 
            return;    
        lastFireTime = Time.time;

        Vector2 direction = (nearestEnemy.position - transform.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.right = direction;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * projectileSpeed;
    }

    //private void FireProjectile(float distance)
    //{
    //    if (projectilePrefab != null && nearestEnemy != null)
    //    {
    //        if(distance < distBetween)
    //        {
    //
    //            Vector2 direction = (nearestEnemy.position - transform.position).normalized;
    //            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
    //            projectile.transform.right = direction;
    //            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    //
    //            if (rb != null)
    //            {
    //                rb.velocity = direction * projectileSpeed;
    //            }
    //        }
    //    }
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}