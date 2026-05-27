using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFollowPlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 0.5f;

    [Header("Steering")]
    [SerializeField] private int rayCount = 12;    // directions to test
    [SerializeField] private float rayLength = 2f;    // obstacle lookahead
    [SerializeField] private float obstacleWeight = 3f;    // how hard to avoid walls
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Separation")]
    [SerializeField] private float separationRadius = 0.8f;
    [SerializeField] private float separationForce = 2f;
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private Transform player;

    private static Transform cachedPlayer;

    // ── Lifecycle ────────────────────────────────────────────────

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Start()
    {
        if (cachedPlayer == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj == null)
            {
                Debug.LogError("[Enemy] No GameObject tagged 'Player' found.");
                enabled = false;
                return;
            }

            cachedPlayer = playerObj.transform;
        }

        player = cachedPlayer;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < stoppingDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 steerDir = GetSteerDirection();
        Vector2 separation = GetSeparation();

        rb.velocity = steerDir * moveSpeed + separation * separationForce;

        FlipSprite(steerDir);
    }

    // ── Steering ─────────────────────────────────────────────────

    private Vector2 GetSteerDirection()
    {
        Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 bestDir = toPlayer;
        float bestScore = float.NegativeInfinity;

        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            // Spread rays evenly around the enemy
            float angle = i * angleStep;
            Vector2 candidate = Rotate(toPlayer, angle);

            float score = ScoreDirection(candidate, toPlayer);

            if (score > bestScore)
            {
                bestScore = score;
                bestDir = candidate;
            }
        }

        return bestDir;
    }

    private float ScoreDirection(Vector2 candidate, Vector2 toPlayer)
    {
        // How much does this direction face the player? (1 = perfect, -1 = away)
        float alignment = Vector2.Dot(candidate, toPlayer);

        // Cast a ray to check for obstacles in this direction
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            candidate,
            rayLength,
            obstacleLayer);

        if (hit.collider != null)
        {
            // Penalise based on how close the obstacle is
            float proximity = 1f - (hit.distance / rayLength);
            alignment -= obstacleWeight * proximity;
        }

        return alignment;
    }

    // ── Separation ───────────────────────────────────────────────

    private Vector2 GetSeparation()
    {
        Collider2D[] neighbours = Physics2D.OverlapCircleAll(
            transform.position, separationRadius, enemyLayer);

        Vector2 push = Vector2.zero;
        foreach (Collider2D col in neighbours)
        {
            if (col.gameObject == gameObject) continue;

            Vector2 away = (Vector2)(transform.position - col.transform.position);
            float dist = away.magnitude;

            if (dist > 0f)
                push += away.normalized * (1f - dist / separationRadius);
        }

        return push;
    }

    // ── Helpers ───────────────────────────────────────────────────

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos);
    }

    private void FlipSprite(Vector2 moveDir)
    {
        if (Mathf.Abs(moveDir.x) < 0.01f) return;

        Vector3 scale = transform.localScale;
        scale.x = moveDir.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        // Draw all steering rays
        Vector2 toPlayer = player != null
            ? ((Vector2)player.position - (Vector2)transform.position).normalized
            : Vector2.up;

        float angleStep = 360f / rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 dir = Rotate(toPlayer, i * angleStep);
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, dir, rayLength, obstacleLayer);

            Gizmos.color = hit.collider != null ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, dir * rayLength);
        }

        // Separation radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}