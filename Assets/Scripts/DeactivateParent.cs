using UnityEngine;

public class DeactivateParent : MonoBehaviour
{
    public void DeactivateParentObject()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
