using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    ParticleSystem ps;
    [Range(0,5)]
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        ps.playbackSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
