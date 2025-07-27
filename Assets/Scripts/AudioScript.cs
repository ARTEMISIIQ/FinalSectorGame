using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource runningSound;
    public float runningMaxVolume;
    public float runningMaxPitch;
    public AudioSource revSound;
    public AudioSource startSound;
    public AudioClip startSoundClip;
    public float startVolume;

    public Rigidbody rb;
    public float gameVolume;
    public float speedRatio;

    public bool complete;
    public bool engine;

    private CarController carController;
    // Start is called before the first frame update
    void Start()
    {
        carController = GetComponent<CarController>();
        complete = false;
        engine = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (carController)
        {
            speedRatio = rb.velocity.magnitude / 100;
        }
        runningSound.volume = Mathf.Lerp(0.3f, runningMaxVolume, speedRatio) * gameVolume;
        revSound.volume = gameVolume;

        runningSound.pitch = Mathf.Lerp(0.3f, runningMaxPitch, speedRatio);
        if (speedRatio > 0.1f && engine)
        {
            if (!runningSound.isPlaying)
            {
                runningSound.Play();
            }
        }
        else if (speedRatio < 0 && carController.im.throttle < 0)
        {
            if (!revSound.isPlaying)
            {
                revSound.Play();
            }
        }
        else
        {
            revSound.volume = 0;
            runningSound.volume = 0;
        }
        if (complete)
        {
            gameVolume = 0;
        }
    }
    public void StartEngine()
    {
        engine = false;
        AudioSource audioSource = Instantiate(startSound, transform.position, Quaternion.identity);
        audioSource.clip = startSoundClip;
        audioSource.volume = startVolume * gameVolume;
        audioSource.Play();
        StartCoroutine(wait(audioSource.clip.length));
        Destroy(audioSource.gameObject, audioSource.clip.length);

    }

    IEnumerator wait(float n)
    {
        yield return new WaitForSeconds(n);
        engine = true;
    }
}
