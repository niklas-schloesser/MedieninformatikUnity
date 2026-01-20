using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    public Animator boxAnimator;    // Animator auf der Box
    public GameObject itemInside;   // Gun ODER Knife (je Box genau 1 Item)
    private bool opened = false;

    public void OpenBox()
    {
        if (opened) return;
        opened = true;

        if (boxAnimator != null)
            boxAnimator.SetTrigger("Open");

        if (itemInside != null)
            itemInside.SetActive(true);
    }
}
