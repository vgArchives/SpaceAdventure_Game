using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : Enemy
{
    //Atributos referentes a movimentação do inimigo
    [SerializeField] private Rigidbody2D enemyRB;

    //Atributos Referentes ao projétil do tiro
    [SerializeField] private GameObject enemyShot;
    [SerializeField] private Transform shotPosition;
    [SerializeField] private float timer = 2f;
    private float maxInterval = 2f;
    private float minInterval = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(EnemyVisible()) Shoot();
    }

    private void Shoot()
    {        
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            var shot = Instantiate(enemyShot, shotPosition.transform.position, shotPosition.transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = Vector2.down * shotVelocity;
            timer = Random.Range(maxInterval, minInterval);
            PlaySound(shotSound, Vector3.zero);
        }
    }

    private void Move()
    {
        enemyRB.velocity = Vector2.down * velocity;
    }

    private bool EnemyVisible()
    {
        bool enemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        return enemyVisible;
    }


}
