﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon
{
    protected int level = 0;

    protected override float fireInterval
    {
        get { return 2f; }
    }

    public Missile(Transform[] shotPosTrans) : base(2, shotPosTrans)
    {
        this.level = 0;
    }

    protected override void Shoot(Transform shotPos)
    {
        for (int i =0; i < shotPosTrans.Length; i++)
        {
            if(i <= optionLevel)
            {
                if (this.level > 0)
                {
                    GameObject missleInstance = GameObject.Instantiate(bulletPrefab, shotPosTrans[i].transform.position, Quaternion.Euler(0, 0, -45));
                    if (level > 1)
                    {
                        GameObject.Instantiate(bulletPrefab, shotPosTrans[i].transform.position + shotPos.right * 0.5f, Quaternion.Euler(0, 0, -45));
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