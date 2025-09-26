using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNotify : MonoBehaviour
{
    // Start is called before the first frame update
    public int timer = 3;
    bool keepPlaying = true;

    void Start()
    {
        StartCoroutine(SoundOut());
    }

    IEnumerator SoundOut()
    {
        while(keepPlaying)
        {
            print("PLAYING ENEMY NOTIFY");
            AudioController.aCtrl.GetSound("enemyNotify").Play();
            yield return new WaitForSeconds(timer);
        }
    }

   
}
