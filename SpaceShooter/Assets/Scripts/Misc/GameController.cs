using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Atributos referentes ao objeto do inimigo
    [SerializeField] private GameObject[] enemies;

    //Atributos referentes ao tempo de criação do inimigo
    [SerializeField] private float timeToCreateEnemy = 1.5f;
    private float timer = 0;

    //Atributos referentes a posição de criação do inimigo
    private float enemyMaxPosY = 9.5f;
    private float enemyMinPosY = 5.5f;
    private float enemyMaxPosX = -8.5f;
    private float enemyMinPosX = 8.5f;

    //Atributos referentes ao sistema de pontuação e level
    [SerializeField] private float scorePoints = 0;
    [SerializeField] private Text scorePointsText;
    [SerializeField] private int level = 1;
    [SerializeField] private float baseXp = 25;

    ////Atributos referentes a criação do boss
    [SerializeField] private GameObject bossAnimation;
    private bool bossAnimationCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateEnemy(CheckType());
    }

    // Update is called once per frame
    void Update()
    {
        if(level <= 5)
        {
            CreateEnemy(CheckType());
            GainLevel();
        }
        else
        {
            if (FindObjectOfType<Enemy>() == null && !bossAnimationCreated)
            {
                FindObjectOfType<GameManager>().audioSource.clip = null;
                bossAnimationCreated = true;
                Invoke("BossArriveAnimation", 3);
            }
        }
    }

    private void BossArriveAnimation()
    {
        var animBoss = Instantiate(bossAnimation, Vector3.zero, transform.rotation);        
    }

    private void CreateEnemy(int type)
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Vector3 positionToCreateEn = new Vector3(Random.Range(enemyMinPosX, enemyMaxPosX), Random.Range(enemyMinPosY, enemyMaxPosY), 0f);
            Instantiate(enemies[type], positionToCreateEn, transform.rotation);
            timer = timeToCreateEnemy;
        }
    }

    //Esse método serve para gerar uma chance de acordo com o level de criar um determinado inimigo
    //Retorna o tipo do inimigo que vai ser criado
    private int CheckType()
    {
        float chance = Random.Range(1, level + 1);
        if(chance > 2) { return 1; }
        else { return 0; }
    }

    public void GainScorePoints(float points) { this.scorePoints += points; scorePointsText.text = scorePoints.ToString(); }

    public void GainLevel()
    {
        float xpRequired = baseXp * level;
        if(scorePoints > xpRequired) { level++; baseXp *= 1.7f; }

        switch (level)
        {
            case 1: { timeToCreateEnemy = 1.5f; break; }
            case 2: { timeToCreateEnemy = 1.25f; break; }
            case 3: { timeToCreateEnemy = 1f; break; }
            case 4: { timeToCreateEnemy = 0.75f; break; }
            case 5: { timeToCreateEnemy = 0.5f; break; }
            default: {timeToCreateEnemy = 0.5f; break; }
        }
    }
}
