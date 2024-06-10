using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 0.8f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            rigidbody.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BarrelDespawner")) {
            Destroy(gameObject);
        }
    }

}
