using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rupie : Collectable
{
    public override void UseItem(GameObject user)
    {
        user.GetComponent<Inventory>().UpdateRupies(1);
        Destroy(gameObject);
    }
}
