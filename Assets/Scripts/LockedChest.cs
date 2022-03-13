using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedChest : Chest
{
    public bool locked;
    public Sprite unlockedChest;

    protected override void Start()
    {
        base.Start();
        locked = true;
    }

    public void OnUnlock()
    {
        if(locked)
        {
            locked = false;
            GetComponent<SpriteRenderer>().sprite = unlockedChest;
        }
    }

    protected override void OnCollect()
    {
        if(!locked)
        {
            base.OnCollect();
        }
    }
}
