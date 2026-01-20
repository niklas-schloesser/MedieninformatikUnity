using System.Collections;
using UnityEngine;

public class BoxOpenByCode : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform lid;            // UP or Rotation transform
    public GameObject rewardInside;  // gun or knife inside (start disabled)

    [Header("Open Settings")]
    public float openAngleX = -70f;
    public float openSpeed = 2f;

    private bool opened = false;
    private Quaternion lidClosedRot;

    void Awake()
    {
        if (lid != null)
            lidClosedRot = lid.localRotation;

        if (rewardInside != null)
            rewardInside.SetActive(false);
    }

    public void OpenBox()
    {
        if (opened) return;
        opened = true;

        if (rewardInside != null)
            rewardInside.SetActive(true);

        if (lid != null)
            StartCoroutine(OpenLid());
        else
            Debug.LogError($"{name}: lid is not assigned!");
    }

    private IEnumerator OpenLid()
    {
        Quaternion startRot = lidClosedRot;
        Quaternion endRot = lidClosedRot * Quaternion.Euler(openAngleX, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            lid.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
    }
}
