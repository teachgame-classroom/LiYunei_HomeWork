using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon
{
    public int level { get; protected set; }

    protected override float FireInterval
    {
        get { return 2f; }
    }

    protected override string fireClipName { get { return "Sound Effect (11)"; } }

    public Missile(Transform[] shotPosTrans,bool isPlayerWeapon) : base(2, shotPosTrans, isPlayerWeapon)
    {
        this.level = 0;
    }

    protected override void Shoot()
    {
        for (int i =0; i < shotPosTrans.Length; i++)
        {
            if(i <= optionLevel)
            {
                if (this.level > 0)
                {
                    bulletPool.Get(shotPosTrans[i].position, Quaternion.Euler(0, 0, -45));

                    if (level > 1)
                    {
                        bulletPool.Get(shotPosTrans[i].position + shotPosTrans[i].right * 0.5f, Quaternion.Euler(0, 0, -45));
                    }

                }
            }
        }
    }

    public void LevelUp()
    {
        level++;
    }

    public void Reset()
    {
        level = 0;
    }
}
