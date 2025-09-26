using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class ObjectDamageScript : MonoBehaviour
{
    public int objectDamage = 0;
    public int origDamage;
    public float objectStunDuration = 0.0f;
    public float damageKnockback = 0.0f;
    bool canDamage = false;
    public GameObject damageMarker;
    GameObject playerObject;
    CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    void Start()
    {
        // Placeholder hardcoded values for Beta Test
        // objectDamage = 60;
        // objectStunDuration = 5.0f;
        // damageKnockback = 200.0f;
        origDamage = objectDamage;
        playerObject = GameObject.FindWithTag("Player");
        cam = GameObject.FindWithTag("Camera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude <= 0) {
            canDamage = false;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (canDamage && !playerObject.GetComponent<GrabScript>().isGrabbed()) {
            if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy") {
                Debug.Log("Damage dealt: " + objectDamage);
                GameObject dmgMarker = Instantiate(damageMarker, other.GetContact(0).point, Quaternion.Euler(0,0,0), null);
                dmgMarker.GetComponent<TextMeshPro>().text = objectDamage.ToString();
                other.rigidbody.AddForce(other.GetContact(0).normal * -damageKnockback, ForceMode.Impulse);
                other.gameObject.GetComponent<Target>().Damage(objectDamage);
                other.gameObject.GetComponent<AIAgent>().Stun(objectStunDuration);        
                GetComponent<Target>().Damage(origDamage);
                Instantiate(other.gameObject.GetComponent<AIAgent>().bloodParticle, other.GetContact(0).point, Quaternion.LookRotation(other.GetContact(0).normal), null);
            }
        }
    }

    public void EnableDamage() {
        canDamage = true;
    }

    public void DisableDamage() {
        canDamage = false;
    }
}
