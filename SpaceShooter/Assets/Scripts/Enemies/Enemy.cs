using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Atributos
    [SerializeField] protected int lifePoints;
    [SerializeField] protected float shotDamage;
    [SerializeField] protected float velocity;
    [SerializeField] protected GameObject explosion;
    [SerializeField] protected float shotVelocity;
    [SerializeField] protected float scorePointsValue;
    [SerializeField] protected AudioClip shotSound;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveLife(int lifePoints)
    {
        if(transform.position.y < 5) { this.lifePoints -= lifePoints; Die(); }
    }

    public void Die()
    {
        if(this.lifePoints <= 0) 
        { 
            Destroy(gameObject); 
            Explode();
            FindObjectOfType<GameController>().GainScorePoints(scorePointsValue);

            //Dropando um power up ao matar um inimigo de acordo com uma determinada chance
            var powerUpControl = FindObjectOfType<PowerUpController>();
            var chance = powerUpControl.DropChance();
            if ( chance > 0.95f) powerUpControl.CreatePowerUp();  
        }
    }

    public void Explode() { Instantiate(explosion, transform.position, transform.rotation); }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyer")) { Destroy(gameObject); }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerShip"))
        { 
            Destroy(gameObject); Explode();
            collision.gameObject.GetComponent<PlayerController>().RemoveLife(1);
        }
        if (collision.gameObject.CompareTag("PlayerShield")) 
        { 
            Destroy(gameObject); 
            Explode();
            FindObjectOfType<GameController>().GainScorePoints(scorePointsValue);
        }
    }

    public void PlaySound(AudioClip audio, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }
}
