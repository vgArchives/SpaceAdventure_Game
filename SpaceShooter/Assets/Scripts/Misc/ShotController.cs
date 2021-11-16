using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    //Atributos referentes ao projétil do tiro
    private Rigidbody2D shotRB;
    [SerializeField] private GameObject shotImpact;
    [SerializeField] private AudioClip shotHitSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
         {
            collision.GetComponent<Enemy>().RemoveLife(1);
            PlaySound(shotHitSound, Vector3.zero);
        }
        else if(collision.CompareTag("PlayerShip"))
        {
            collision.GetComponent<PlayerController>().RemoveLife(1);
            PlaySound(shotHitSound, Vector3.zero);
        }
        else if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossController>().RemoveLife(1);
        }

        Destroy(gameObject);
        Instantiate(shotImpact, transform.position, transform.rotation);
    }

    private void PlaySound(AudioClip audio, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }
}
