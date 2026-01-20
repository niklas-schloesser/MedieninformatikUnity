using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 25;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyAiTutorial enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
