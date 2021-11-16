using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosionController : MonoBehaviour
{
    [SerializeField] private AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(explosionSound, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyObject()
    {

        Destroy(gameObject);
    }
}
