using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string m_name;

    public virtual void GetItem(GameObject user, float animTime)
    {
        user.GetComponent<Inventory>().AddItem(m_name);
        StartCoroutine(AnimTime(animTime));
    }

    public virtual IEnumerator AnimTime(float animTime)
    {
        yield return new WaitForSeconds(animTime);
        Destroy(gameObject);
    }
}
