using UnityEngine;

public class Enemy : Mover
{
    public int xpValue = 10;

    public float triggerLength = 1f;
    public float chaseLength = 3f;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter2D;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    public LockedChest[] chestsToUnlock;
    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
        playerTransform = GameManager.instance.player.transform;
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void drawLineToPlayer(Color color)
    {
        Debug.DrawLine(transform.position, GameManager.instance.player.transform.position, color);
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, startingPosition);

        if(distanceToPlayer < chaseLength)
        {
            // In chase range
            drawLineToPlayer(Color.green);
            if(distanceToPlayer < triggerLength)
            {
                chasing = true;
            }
                
            if(chasing)
            {
                drawLineToPlayer(Color.red);
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
                else
                {
                    UpdateMotor(startingPosition - transform.position);
                }
            }
        }
        else
        {
            drawLineToPlayer(Color.blue);
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        // Check for collision
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter2D, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            hits[i] = null;
        }
    }

    protected override void Die()
    {
        GameManager.instance.AddExperience(5);
        GameManager.instance.ShowText(
            "+" + xpValue.ToString() + "XP",
            25,
            Color.white,
            transform.position,
            Vector3.up * 25f,
            1f
        );

        if(chestsToUnlock.Length > 0)
        {
            foreach(LockedChest chest in chestsToUnlock)
            {
                chest.OnUnlock();
            }
        }

        Destroy(gameObject);
    }
}
