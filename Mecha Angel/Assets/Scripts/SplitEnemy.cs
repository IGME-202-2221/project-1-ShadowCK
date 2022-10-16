using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : Enemy
{
    [SerializeField]
    private GameObject splitPrefab;
    [SerializeField]
    private int splitNum = 2;
    // TODO
    // private float spreadRange = 1f;

    public override void Die()
    {
        // TODO
        // float spreadStep = spreadRange / splitNum;

        for (int i = 0; i < splitNum; i++)
        {
            Instantiate(splitPrefab, transform.position, transform.rotation);
        }
        base.Die();
    }
}
