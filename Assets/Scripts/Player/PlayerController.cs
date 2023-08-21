using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float radius = 10.0f;

    public GameObject _projectilePrefab;

    public float _projectileSpeed = 10f;
    public float _fireRate = 1f;

    private Transform _nearestEnemy;
    private float lastFireTime;

    public LayerMask objLayerMask;


    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(_joystick.Horizontal * _moveSpeed, _joystick.Vertical * _moveSpeed);

        FindNearestEnemy();
    }

    private void Update()
    {
        if (_nearestEnemy != null && Time.time - lastFireTime >= 1f / _fireRate)
        {
            _nearestEnemy.GetComponentInChildren<SpriteRenderer>().material.color = Color.red;

            FireProjectile();
            lastFireTime = Time.time;
        }
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                _nearestEnemy = enemy.transform;
            }
        }
    }

    private void FireProjectile()
    {
        if (_projectilePrefab != null && _nearestEnemy != null)
        {
            Vector2 direction = (_nearestEnemy.position - transform.position).normalized;
            GameObject projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
            projectile.transform.right = direction;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = direction * _projectileSpeed;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}