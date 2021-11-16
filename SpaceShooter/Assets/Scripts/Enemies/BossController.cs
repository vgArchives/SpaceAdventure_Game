using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bossRb;
    bool direction = false; //false = left / true = right

    [SerializeField] private int lifePoints;
    [SerializeField] private float bossVelocity = 2.5f;
    [SerializeField] private float shotDamage;
    [SerializeField] private float shotVelocity;
    [SerializeField] private Transform[] shotPosition;
    [SerializeField] private GameObject[] bossShot;
    private int shootType;
    [SerializeField] private float timerShot0 = 1f;
    [SerializeField] private float timerShot1 = 2f;
    [SerializeField] private float timerShot2 = 2f;
    [SerializeField] private string[] phases;
    [SerializeField] private string currentPhase;
    [SerializeField] private float phaseWaitTime = 0f;
    [SerializeField] private GameObject deathAnimation;
    [SerializeField] private float scorePointsValue;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] protected AudioClip shotSound;

    // Start is called before the first frame update
    void Start()
    {
        bossRb = GetComponent<Rigidbody2D>();
        healthBarSlider = GetComponentInChildren<Slider>();
        healthBarSlider.maxValue = lifePoints;
        healthBarSlider.value = lifePoints;

        //Passando a camera do jogo para o canvas do boss
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        PhaseChanger();

        switch(currentPhase)
        {
            case "FirstPhase":
                FirstPhase();
                break;

            case "SecondPhase":
                SecondPhase();
                break;

            case "ThirdPhase":
                ThirdPhase();
                break;
        }
    }

    private void FirstPhase()
    {
        shootType = 0;
        Moving();
        Shoot();
    }

    private void SecondPhase()
    {
        shootType = 1;
        Shoot();
        Stoping();
    }

    private void ThirdPhase()
    {
        shootType = 2;
        Moving();
        Shoot();
    }

    private void Moving()
    {
        var leftDir = Vector2.left * bossVelocity;
        var rightDir = Vector2.right * bossVelocity;
        float limitPosX = 5.9f;

        if (direction)
        {
            bossRb.velocity = leftDir;
            if (transform.position.x <= -limitPosX) direction = false;
        }
        else
        {
            bossRb.velocity = rightDir;
            if (transform.position.x >= limitPosX) direction = true;
        }
    }

    private void Stoping()
    {
        bossRb.velocity = Vector2.zero;
    }

    private void Shoot()
    {
        switch (shootType)
        {
            case 0:
                {
                    GearFirst();
                    break;
                }
            case 1:
                {
                    GearSecond();
                    break;
                }
            case 2:
                {
                    GearFirst();
                    GearSecond();
                    break;
                }
        }
    }

    private void GearFirst()
    {
        GameObject shot;
        timerShot1 -= Time.deltaTime;
        timerShot2 -= Time.deltaTime;
        float minInterval = 2f;
        float maxInterval = 0.5f;

        if (timerShot1 <= 0)
        {
            shot = Instantiate(bossShot[1], shotPosition[1].position, shotPosition[1].rotation);
            shot.GetComponent<Rigidbody2D>().velocity = Vector2.down * shotVelocity;
            timerShot1 = Random.Range(maxInterval, minInterval);

            PlaySound(shotSound, Vector3.zero);
        }
        else if (timerShot2 <= 0)
        {
            shot = Instantiate(bossShot[1], shotPosition[2].position, shotPosition[2].rotation);
            shot.GetComponent<Rigidbody2D>().velocity = Vector2.down * shotVelocity;
            timerShot2 = Random.Range(maxInterval, minInterval);

            PlaySound(shotSound, Vector3.zero);
        }
    }

    private void GearSecond()
    {
        //Encontrando o player na cena
        var playerInScene = FindObjectOfType<PlayerController>();
        GameObject shot;
        float minInterval = 0.5f;
        float maxInterval = 0.7f;

        if (playerInScene)
        {
            timerShot0 -= Time.deltaTime;
            if (timerShot0 <= 0)
            {
                shot = Instantiate(bossShot[2], shotPosition[0].position, shotPosition[0].rotation);
                //Encontrando o valor da direção (Subtrai a posição em vetor do player em relação a posição em vetor do tiro)
                Vector2 shotDirection = playerInScene.transform.position - shot.transform.position;
                //Dando a direção e velocidade ao tiro
                shotDirection.Normalize();
                shot.GetComponent<Rigidbody2D>().velocity = shotDirection * shotVelocity;

                //Dando o angulo que o tiro tem que sair
                //Necessario converter o valor de radianos para graus
                var angulo = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;
                shot.transform.rotation = Quaternion.Euler(0, 0, angulo + 90);

                timerShot0 = Random.Range(maxInterval, minInterval);

                PlaySound(shotSound, Vector3.zero);
            }
        }
    }

    private void PhaseChanger()
    {
        if(phaseWaitTime <= 0)
        {
            int phaseIndex = Random.Range(0, phases.Length);
            currentPhase = phases[phaseIndex];

            phaseWaitTime = 5f;
        }
        else
        {
            phaseWaitTime -= Time.deltaTime;
        }
    }

    public void RemoveLife(int lifePoints)
    {
        this.lifePoints -= lifePoints;
        healthBarSlider.value = this.lifePoints;
        if (this.lifePoints <= 0)
        {
            Instantiate(deathAnimation, transform.position, transform.rotation);
            FindObjectOfType<GameController>().GainScorePoints(scorePointsValue);
            Destroy(gameObject);                
        }
    }

    public void PlaySound(AudioClip audio, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }
}
