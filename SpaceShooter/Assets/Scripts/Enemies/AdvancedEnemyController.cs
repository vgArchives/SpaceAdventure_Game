using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemyController : Enemy
{
    //Atributos referentes a movimenta��o do inimigo
    [SerializeField] private Rigidbody2D enemyRB;
    private float changeMovPosition = 2f;
    private bool canChangeMov = true;

    //Atributos referentes ao proj�til do tiro
    [SerializeField] private GameObject enemyShot;
    [SerializeField] private Transform shotPosition;
    [SerializeField] private float timer = 1.5f;
    private float maxInterval = 1.5f;
    private float minInterval = 3f;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyRB.velocity = Vector2.down * velocity;
    }

    // Update is called once per frame
    void Update()
    {        
        if (EnemyVisible()) Shoot();
        Move();
    }
    private void Shoot()
    {
        //Encontrando o player na cena
        var playerInScene = FindObjectOfType<PlayerController>();

        if(playerInScene)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                var shot = Instantiate(enemyShot, shotPosition.transform.position, shotPosition.transform.rotation);
                //Encontrando o valor da dire��o (Subtrai a posi��o em vetor do player em rela��o a posi��o em vetor do tiro)
                Vector2 direction = playerInScene.transform.position - shot.transform.position;
                //Dando a dire��o e velocidade ao tiro
                direction.Normalize();
                shot.GetComponent<Rigidbody2D>().velocity = direction * shotVelocity;

                //Dando o angulo que o tiro tem que sair
                //Necess�rio converter o valore de radiano para graus
                var angulo = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                shot.transform.rotation = Quaternion.Euler(0, 0, angulo + 90);

                timer = Random.Range(maxInterval, minInterval);

                PlaySound(shotSound, Vector3.zero);
            }
        }
    }
    private void Move()
    {
        if (transform.position.y < changeMovPosition && canChangeMov)
        {
             if(transform.position.x > 0)
             {
                var leftDirection = new Vector2(velocity, velocity) * -1;
                leftDirection.Normalize();
                enemyRB.velocity = leftDirection * velocity;

                canChangeMov = false;
             }
            else
            {
                var rightDirection = new Vector2(velocity, -velocity);
                rightDirection.Normalize();
                enemyRB.velocity = rightDirection * velocity;

                canChangeMov = false;
            }
        }
    }
    private bool EnemyVisible()
    {
        bool enemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        return enemyVisible;
    }
}
