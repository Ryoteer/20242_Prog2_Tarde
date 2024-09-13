using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private Player _parent;

    private void Start()
    {
        _parent = GetComponentInParent<Player>();
    }

    public void Attack()
    {
        _parent.Attack();
    }

    public void MultiAttack()
    {
        _parent.MultiAttack();
    }

    public void Cast()
    {
        _parent.Cast();
    }
}
