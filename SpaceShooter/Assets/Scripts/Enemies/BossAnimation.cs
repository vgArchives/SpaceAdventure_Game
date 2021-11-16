using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] private GameObject bossToCreate;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform[] explosionPositions;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private GameObject gameEndingAnimation;
    

   // Start is called before the first frame update
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBoss()
    {
        Instantiate(bossToCreate, transform.position, transform.rotation);
        Destroy(transform.parent.gameObject);

        var audioHelper = FindObjectOfType<GameManager>().audioSource;
        audioHelper.clip = bossMusic;
        audioHelper.Play();       
    }

    public void CreateExplosion1()
    {        
        for (int i = 0; i < explosionPositions.Length; i+=2)
        {
                var exp = Instantiate(explosion, explosionPositions[i].position, explosionPositions[i].rotation);
                Destroy(exp, 0.6f);
                AudioSource.PlayClipAtPoint(explosionSound, Vector3.zero);
        }
    }
    public void CreateExplosion2()
    {
        for (int i = 1; i < explosionPositions.Length; i += 2)
        {
            var exp = Instantiate(explosion, explosionPositions[i].position, explosionPositions[i].rotation);
            Destroy(exp, 0.6f);
            AudioSource.PlayClipAtPoint(explosionSound, Vector3.zero);
        }
    }

    private void EndGame()
    {
        var player = FindObjectOfType<PlayerController>();
        Instantiate(gameEndingAnimation, player.transform.position, player.transform.rotation);
        Destroy(player.gameObject);
        FindObjectOfType<GameManager>().InitialScreen(5f);
    }

}

