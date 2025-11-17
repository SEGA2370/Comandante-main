using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bullet;
    [SerializeField] private float fireSpeed;
    [SerializeField] private Transform firePoint;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Shoot(float direction)
    {

        Vector3 movingSide;
        if (playerMovement.IsFacingRight)
        {
            movingSide = Vector3.right * 0.5f;
        }
        else
        {
            movingSide = Vector3.left * 0.5f;
        }
        Vector3 spawnPosition = firePoint.position + movingSide;
       
        Rigidbody2D currentBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);

        float speed;
        if (playerMovement.IsFacingRight)
        {
            speed = fireSpeed;
        }
        else
        {
            speed = -fireSpeed;
        }
        currentBullet.linearVelocity = new Vector2(speed, currentBullet.linearVelocity.y);
    }
}
