using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public float pushRecoverySpeed = 0.2f;

    protected float immuneTime = 1f;
    protected float lastImmune;
    protected Vector3 pushDirection;

    protected virtual void ReceiveDamage(Damage damage)
    {
        if(Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hp -= damage.damageAmount;
            pushDirection = (transform.position - damage.origin).normalized * damage.pushForce;

            GameManager.instance.ShowText(
                damage.damageAmount.ToString(),
                25,
                Color.red,
                transform.position,
                Vector3.up * 25f, .5f
            );

            if (hp <= 0)
            {
                hp = 0;
                Die();
            }
        }
    }
    protected virtual void Die()
    {

    }
}