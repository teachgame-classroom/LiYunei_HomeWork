using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float f = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        f = Mathf.Sin(2 * Mathf.PI * Time.time) * 5 + 5;

        Gizmos.DrawWireSphere(Vector3.zero, f);
    }
}
