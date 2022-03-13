using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnlockChest : Collidable
{
    public LockedChest[] chests;

    protected override void OnCollide(Collider2D collider)
    {
        for(int i = 0; i < chests.Length; i++)
        {
            chests[i].OnUnlock();
        }
    }
}
