using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator weaponAnimator;

    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float bulletRange = 100f;
    [SerializeField] private float fireRate;

    private float time;

    void Update()
    {
        time += Time.deltaTime;

        float nextTimeToFire = 1 / fireRate;

        if (Input.GetKey(KeyCode.Mouse0) && time >= nextTimeToFire)
        {
            Shoot();
            time = 0;
        }
    }

    void Shoot()
    {
        weaponAnimator.SetTrigger("shoot");
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, bulletRange))
        {
            if (hit.transform.name.Contains("Enemy"))
            {
                Enemy enemyScript;
                enemyScript = hit.transform.GetComponent<Enemy>();
                enemyScript.TakeDamage(bulletDamage);
            }
        }
    }

}
