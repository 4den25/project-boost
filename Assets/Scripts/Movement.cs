using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in the editor
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem boosterParticals;
    [SerializeField] ParticleSystem leftThrusterParticals;
    [SerializeField] ParticleSystem rightThrusterParticals;

    //CACHE - e.g. references for readability or speed
    Rigidbody rb;
    AudioSource audioSource;

    //STATE - private instance (member) variables
    //bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space)) //Ctrl + Shift + Space to show information of method
        {
            StartThrusting(); //Ctrl + R + M to extract a block of code into a new method
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); //rb.AddRelativeForce(0, 1, 0) is ok;

        // Ctrl + K + S to quickly wrap code into an block (if, try...)
        if (!audioSource.isPlaying) //with this condition, the audio clip would only be started once at the first frame instead of starting again and again at every frames where Input.GetKey(KeyCode.Space) = true
        {
            //audioSource.Play(); //is good when the audio source of the object just has one audio clip
            audioSource.PlayOneShot(mainEngine); //is good when the audio source of the object has multiple audio clips
            if (!boosterParticals.isPlaying)
            {
                boosterParticals.Play();
            }
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        boosterParticals.Stop();
    }

    void RotateLeft()
    {
        ApplyRotation(1);
        if (!leftThrusterParticals.isPlaying)
        {
            leftThrusterParticals.Play();
            rightThrusterParticals.Stop();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-1);
        if (!rightThrusterParticals.isPlaying)
        {
            rightThrusterParticals.Play();
        }
    }

    void StopRotating()
    {
        leftThrusterParticals.Stop();
        rightThrusterParticals.Stop();
    }

    void ApplyRotation(float direction)
    {
        rb.freezeRotation = true; //freeze rotation by physics simulator, give full control of the rotation to player
        transform.Rotate(direction * Vector3.forward * rotationThrust * Time.deltaTime);
        rb.freezeRotation = false; //after player did the rotation, unfreeze the rotation by physics simulator
    }
}

