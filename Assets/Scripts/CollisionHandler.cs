using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    float levelLoadDelay = 3f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isTransitioning = false; //only true when the object bump into something
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; //disable/enable collision to easily completed a level
        }
    }

    void OnCollisionEnter(Collision collision) //was callback when object bump into something, collision var is the thing the object bump into
    {
        if (isTransitioning || collisionDisabled) { return; } //end the OnCollisionEnter method here without executing code lines below

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true; //using isTransitioning to prevent this method was called more than once
        audioSource.Stop(); //stop the rocket boost clip just in case
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false; //it also prevent Movement script from excuting audioSource.Stop at every frames when we didn't hit space key
        Invoke("LoadNextLevel", levelLoadDelay);
    }


    void StartCrashSequence()
    {
        isTransitioning = true; //using isTransitioning to prevent this method was called more than once
        audioSource.Stop(); //stop the rocket boost clip just in case 
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false; //disable script Movement after crashing event so that player cannot continue moving the rocket
        Invoke("ReloadLevel", levelLoadDelay);//Invoke will delay the method from being executed, it needs 2 params: method name and delay time  
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //GetActiveScene method get the scene that player is currently on
        //LoadScene needs the index or name of the scene so we use buildIndex here

        SceneManager.LoadScene(currentSceneIndex);
        //LoadScene method load the scene in the Scenes in Build field (Unity>File>Build Settings)        
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int sceneCount = SceneManager.sceneCountInBuildSettings; //get the number of scenes in Build Settings

        if (nextSceneIndex == sceneCount)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}

