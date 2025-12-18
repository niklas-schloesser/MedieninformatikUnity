using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody bulletRb;

    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // bulletRb.linearVelocity = transform.forward * speed;
        bulletRb.AddForce(transform.forward * speed);
    }
}
