using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Collectable
{
    public override void UseItem(GameObject user)
    {
        user.GetComponent<Inventory>().GetHealthComp().UpdateHealth(2);
        Destroy(gameObject);
    }
}
