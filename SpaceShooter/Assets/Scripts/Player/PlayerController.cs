using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Atributos referentes ao player
    [SerializeField] private float playerVelocity = 5f;
    [SerializeField] private GameObject playerExplosion;
    private Rigidbody2D playerRB;

    //Atributos referentes ao posicionamento do player
    private float xLimit = 8.3f;
    private float yLimit = 4.3f;

    //Atributos referentes ao escudo
    [SerializeField] private GameObject playerShield;
    [SerializeField] private GameObject shieldCreated;
    [SerializeField] int shieldCount = 3;
    private float shieldTime = 0f;
    [SerializeField] GameObject shieldsIcons;
    [SerializeField] private AudioClip[] shieldSound;

    //Atributos referentes ao projétil do tiro
    [SerializeField] private GameObject[] playerShot;
    [SerializeField] private Transform shotPosition;
    [SerializeField] private float shotVelocity = 10f;
    [SerializeField] private int shotLevel = 1;
    [SerializeField] private AudioClip[] shotSound;

    //Atributos referentes a barra de vida do player
    [SerializeField] private int lifePoints = 50;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Slider healthBarSlider;

    //Atributos referentes ao Power Up
    [SerializeField] private AudioClip powerUpSound;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        healthBarSlider = healthBar.GetComponentInChildren<Slider>();
        healthBarSlider.maxValue = lifePoints;
        healthBarSlider.value = lifePoints;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        CreateShield();
    }

    //Incrementa ou diminui o valor da posição do eixo que o jogador apertar o botão multiplicando esse valor por uma velocidade constante para aumentar a velocidade
    private void Move()
    {        
        Vector2 axisPosition = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        axisPosition.Normalize();
        playerRB.velocity = axisPosition * playerVelocity;

        //Limitando para não sair da tela
        //Clamp -> checa um valor e verifica se ele passa de um limite min ou máx caso passar ele volta aos limites
        float xClamp = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        float yClamp = Mathf.Clamp(transform.position.y, -yLimit, yLimit);

        transform.position = new Vector3(xClamp, yClamp, 0f);
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 position = new Vector3();
            switch(shotLevel)
            {
                case 1:
                    {
                        position = new Vector3(shotPosition.transform.position.x, shotPosition.transform.position.y, 0f);
                        ShotType(playerShot[0], position);
                        PlaySound(shotSound[0], Vector3.zero);
                        break;
                    }
                case 2: 
                    {
                        position = new Vector3(shotPosition.transform.position.x + 0.3f, shotPosition.transform.position.y - 0.25f, 0f);
                        ShotType(playerShot[1], position);
                        position = new Vector3(shotPosition.transform.position.x - 0.3f, shotPosition.transform.position.y - 0.25f, 0f);
                        ShotType(playerShot[1], position);
                        PlaySound(shotSound[1], Vector3.zero);
                        break; 
                    }
                case 3:
                    {
                        position = new Vector3(shotPosition.transform.position.x, shotPosition.transform.position.y, 0f);
                        ShotType(playerShot[0], position);
                        position = new Vector3(shotPosition.transform.position.x + 0.3f, shotPosition.transform.position.y - 0.25f, 0f);
                        ShotType(playerShot[1], position);
                        position = new Vector3(shotPosition.transform.position.x - 0.3f, shotPosition.transform.position.y - 0.25f, 0f);
                        ShotType(playerShot[1], position);
                        PlaySound(shotSound[2], Vector3.zero);
                        break;
                    }
            }        
        }
    }

    private void ShotType(GameObject shotType, Vector3 shotPosition)
    {
        var shot = Instantiate(shotType, shotPosition, transform.rotation);
        //Acessa o Rigidbody do objeto tiro e seta uma velocidade para o projétil
        shot.GetComponent<Rigidbody2D>().velocity = Vector2.up * shotVelocity;
    }

    private void PlaySound(AudioClip audio, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }

    public void RemoveLife(int lifePoints)
    {
        this.lifePoints -= lifePoints;
        healthBarSlider.value = this.lifePoints;
        if (this.lifePoints <= 0)
        {
            Destroy(gameObject);
            Instantiate(playerExplosion, transform.position, transform.rotation);

            //Reiniciando o jogo após o player morrer
            FindObjectOfType<GameManager>().InitialScreen(3f);
        }
    }

    private void CreateShield()
    {
        if(shieldCreated)
        {
            shieldCreated.transform.position = transform.position;
            shieldTime += Time.deltaTime;
            if(shieldTime > 5.7f)
            {
                Destroy(shieldCreated); ;
                shieldTime = 0f;
                PlaySound(shieldSound[1], Vector3.zero);
            }
        }
        else if(Input.GetButtonDown("Shield") && shieldCount > 0)
        {
            shieldCreated = Instantiate(playerShield, transform.position, transform.rotation);
            if(shieldCount > 0) shieldsIcons.GetComponentsInChildren<Image>()[shieldCount - 1].color = Color.gray;
            shieldCount--;
            PlaySound(shieldSound[0], Vector3.zero);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PowerUp")) 
        { 
            if (shotLevel < 3)
            {
                shotLevel++; 
                Destroy(collision.gameObject);
                PlaySound(powerUpSound, Vector3.zero);
            }
                
        }
        else if(collision.CompareTag("Boss"))
        {
           if(!shieldCreated) RemoveLife(1);
        }
    }
}
