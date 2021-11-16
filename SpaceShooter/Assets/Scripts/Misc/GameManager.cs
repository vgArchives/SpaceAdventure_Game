using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] AudioClip initialScreenMusic;

    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1) Destroy(gameObject);        
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        StartGame();
    }

    //Método que inicia o jogo
    public void StartGame()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
            audioSource.volume = 0.090f;
        }
             
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InitialScreen(float seconds)
    {
        StartCoroutine(InitialScreenHelper(seconds));
    }

    IEnumerator InitialScreenHelper(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(0);
        audioSource.clip = initialScreenMusic;
        audioSource.Play();
    }
}
