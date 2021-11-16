using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private GameObject powerUp;
    
    //Atributos referentes a posição de criação do power up
    private float pwMaxLimitY = 4.5f;
    private float pwMinLimitY = 1.7f;
    private float pwLimitX = 8.5f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CreatePowerUp()
    {
        var position = new Vector3(Random.Range(-pwLimitX, pwLimitX), Random.Range(pwMinLimitY, pwMaxLimitY), 0f);
        var pw = Instantiate(powerUp, position, transform.rotation);
        pw.GetComponent<Rigidbody2D>().velocity = Vector2.down * 1f;
        Destroy(pw.gameObject, 5f);
    }

    public float DropChance()
    {
        float dropChance = Random.Range(0f, 1f);
        return dropChance;
    }
}
