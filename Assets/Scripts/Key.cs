using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectable
{
    public override void UseItem(GameObject user)
    {
        user.GetComponent<Inventory>().UpdateKeys(1);
        Destroy(gameObject);
    }
}
