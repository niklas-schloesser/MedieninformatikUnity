using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;

    private Animator enemyAnimator;

    void Start()
    {
        enemyAnimator = this.GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            StartCoroutine(Die());
        }
        else
        {
            enemyAnimator.SetTrigger("takeDamage");
        }
    }

    IEnumerator Die()
    {
        enemyAnimator.SetBool("isDead", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}