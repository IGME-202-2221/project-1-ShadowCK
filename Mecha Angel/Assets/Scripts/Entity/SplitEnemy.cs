using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : Enemy
{
    [SerializeField]
    protected GameObject splitPrefab;
    [SerializeField]
    protected int splitNum = 2;
    [SerializeField]
    protected Vector2 spreadRange = new Vector2(5f, 5f);

    public override void Die()
    {
        if (splitNum > 1)
        {
            Vector2 spreadStep = spreadRange * 2 / (splitNum - 1);
            Vector2 minPoint = (Vector2)transform.position - spreadRange;
            for (int i = 0; i < splitNum; i++)
            {
                Instantiate(splitPrefab, minPoint, transform.rotation);
                minPoint += spreadStep;
            }
        }
        else if (splitNum == 1)
        {
            Instantiate(splitPrefab, transform.position, transform.rotation);
        }

        base.Die();
    }
}
