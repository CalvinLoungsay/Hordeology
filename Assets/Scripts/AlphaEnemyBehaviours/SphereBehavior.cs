using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehavior : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        
      
        Invoke(nameof(Destroy), 1);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }

}
