using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable
{
    public override void UseItem(GameObject user)
    {
        user.GetComponent<Inventory>().UpdateBombs(1);
        Destroy(gameObject);
    }
}
