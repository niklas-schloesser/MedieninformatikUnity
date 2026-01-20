using System.Collections;
using UnityEngine;

public enum WeaponType { Hands, Knife, Gun }

public class WeaponSystem : MonoBehaviour
{
    [Header("Current")]
    public WeaponType currentWeapon = WeaponType.Hands;

    [Header("Equipped Objects (children of WeaponHolder)")]
    public GameObject handsObject;
    public GameObject knifeObject;
    public GameObject gunObject;

    [Header("Melee")]
    public int handsDamage = 5;
    public int knifeDamage = 20;
    public float attackRange = 2.5f;

    [Header("Gun")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;

    [Header("References")]
    public Camera playerCam;

    [Header("Knife Animation (optional)")]
    public Transform knifeModel; // set to KnifeModel transform (the mesh object)
    public float stabDistance = 0.08f;
    public float stabSpeed = 18f;

    private float nextFireTime;
    private Coroutine stabRoutine;

    private void Start()
    {
        // Make sure only hands is visible at start
        SetActiveWeaponObjects(WeaponType.Hands);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentWeapon == WeaponType.Gun)
            {
                TryShoot();
            }
            else
            {
                MeleeAttack();

                if (currentWeapon == WeaponType.Knife && knifeModel != null)
                {
                    if (stabRoutine != null) StopCoroutine(stabRoutine);
                    stabRoutine = StartCoroutine(StabAnimation());
                }
            }
        }
    }

    public void EquipWeapon(WeaponType type)
    {
        currentWeapon = type;
        SetActiveWeaponObjects(type);

        Debug.Log($"Equipped: {type} | Hands:{handsObject.activeSelf} Knife:{knifeObject.activeSelf} Gun:{gunObject.activeSelf}");
    }

    private void SetActiveWeaponObjects(WeaponType type)
    {
        if (handsObject != null) handsObject.SetActive(type == WeaponType.Hands);
        if (knifeObject != null) knifeObject.SetActive(type == WeaponType.Knife);
        if (gunObject != null) gunObject.SetActive(type == WeaponType.Gun);
    }

    private void MeleeAttack()
    {
        if (playerCam == null) return;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, attackRange))
        {
            if (hit.transform.TryGetComponent(out EnemyAiTutorial enemy))
            {
                int dmg = (currentWeapon == WeaponType.Knife) ? knifeDamage : handsDamage;
                enemy.TakeDamage(dmg);
            }
        }
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("WeaponSystem: bulletPrefab or firePoint missing!");
            return;
        }

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private IEnumerator StabAnimation()
    {
        Vector3 start = knifeModel.localPosition;
        Vector3 forward = start + Vector3.forward * stabDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * stabSpeed;
            knifeModel.localPosition = Vector3.Lerp(start, forward, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * stabSpeed;
            knifeModel.localPosition = Vector3.Lerp(forward, start, t);
            yield return null;
        }
    }
}
