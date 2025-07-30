using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private AudioScript am;

    public TextMeshProUGUI mph;
    public TextMeshProUGUI stopwatch;
    public TextMeshProUGUI fps;
    public TextMeshProUGUI restartTime;
    public TextMeshProUGUI volumeText;
    public TextMeshProUGUI evolumeText;
    public TextMeshProUGUI mvolumeText;
    public TextMeshProUGUI roundNum;
    public TextMeshProUGUI durabilityWarning;
    public TextMeshProUGUI durabilityPercent;
    public Canvas settings;
    public Slider vol;
    public Slider evol;
    public Slider mvol;
    public Slider durability;
    public Image up;
    public Image down;
    public Image left;
    public Image right;
    public Image brake;
    public float time;
    public float durabilityTime;
    public Vector3 preLeaveVel;
    public Rigidbody rb;
    bool watchActive = false;
    public bool watchPrevActive = false;
    public AudioSource gameMusic;
    bool settingsActive;

    private void Start()
    {
        settingsActive = false;
        watchPrevActive = false;
        if (gameMusic && !GameObject.FindWithTag("Finish"))
        {
            gameMusic.Play();
            DontDestroyOnLoad(gameMusic);
        }
        else
        {
            if (GameObject.FindWithTag("Finish"))
            {
                gameMusic = GameObject.FindWithTag("Finish").GetComponent<AudioSource>();
            }
        }
    }

    public virtual void changeText(float speed)
    {
        float s = speed * -2.23694f;
        mph.text = Mathf.Clamp(Mathf.Round(s), 0f, 1000f) + "MPH";
    }

    void Update()
    {
        if (gameMusic)
        {
            gameMusic.volume = PlayerPrefs.GetFloat("Volume") > PlayerPrefs.GetFloat("Music") ? PlayerPrefs.GetFloat("Music") : PlayerPrefs.GetFloat("Volume");
        }
        if (am && !am.complete)
        {
            am.gameVolume = PlayerPrefs.GetFloat("Volume") > PlayerPrefs.GetFloat("Effect") ? PlayerPrefs.GetFloat("Effect") : PlayerPrefs.GetFloat("Volume");
        }
        if (fps)
        {
            fps.text = (Mathf.Round(1f / Time.deltaTime)).ToString() + " fps";
        }
        if (watchActive)
        {
            time = time + Time.deltaTime;
        }
        if (stopwatch)
        {
            if (time < 60)
            {
                stopwatch.text = (Mathf.Round(time * 10000) / 10000).ToString();
            }
            else
            {
                if ((time % 60) > 10)
                {
                    stopwatch.text = Mathf.Floor(time / 60).ToString() + ":" + (Mathf.Round((time % 60) * 10000) / 10000).ToString();
                }
                else
                {
                    stopwatch.text = Mathf.Floor(time / 60).ToString() + ":0" + (Mathf.Round((time % 60) * 10000) / 10000).ToString();
                }
            }
        }
        float alphaFactor = 0.2f;
        if (up && down && left && right && brake)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                Color color = up.color;
                color.a = Mathf.Abs(Input.GetAxis("Vertical") * alphaFactor);
                up.color = color;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                Color color = down.color;
                color.a = Mathf.Abs(Input.GetAxis("Vertical") * alphaFactor);
                down.color = color;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                Color color = right.color;
                color.a = Mathf.Abs(Input.GetAxis("Horizontal") * alphaFactor);
                right.color = color;
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                Color color = left.color;
                color.a = Mathf.Abs(Input.GetAxis("Horizontal") * alphaFactor);
                left.color = color;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Color color = brake.color;
                color.a = alphaFactor;
                brake.color = color;
            }
            else
            {
                Color color = brake.color;
                color.a = 0;
                brake.color = color;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && settings)
        {
            if (!settingsActive)
            {
                OpenSettings();
            }
            else
            {
                CloseSettings();
            }
        }
        volumeText.text = "Game Volume: " + Mathf.Round(PlayerPrefs.GetFloat("Volume") * 100).ToString() + "%";
        vol.value = PlayerPrefs.GetFloat("Volume");
        if (evol && mvol)
        {
            evolumeText.text = "Effects Volume: " + Mathf.Round((PlayerPrefs.GetFloat("Volume") > PlayerPrefs.GetFloat("Effect") ? PlayerPrefs.GetFloat("Effect") : PlayerPrefs.GetFloat("Volume")) * 100).ToString() + "%";
            evol.value = PlayerPrefs.GetFloat("Volume") > PlayerPrefs.GetFloat("Effect") ? PlayerPrefs.GetFloat("Effect") : PlayerPrefs.GetFloat("Volume");
            mvolumeText.text = "Music Volume: " + Mathf.Round((PlayerPrefs.GetFloat("Volume") > PlayerPrefs.GetFloat("Music") ? PlayerPrefs.GetFloat("Music") : PlayerPrefs.GetFloat("Volume")) * 100).ToString() + "%";
            mvol.value = PlayerPrefs.GetFloat("Volume") > PlayerPrefs.GetFloat("Music") ? PlayerPrefs.GetFloat("Music") : PlayerPrefs.GetFloat("Volume");
        }
    }

    public void StartWatch()
    {
        watchActive = true;
    }
    public void StopWatch() // Wow!
    {
        watchActive = false;
    }

    public void returnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenSettings()
    {
        preLeaveVel = rb.velocity;
        StopWatch();
        settings.gameObject.SetActive(true);
        settingsActive = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void CloseSettings()
    {
        settings.gameObject.SetActive(false);
        settingsActive = false;
        StartCoroutine(restart());
    }

    public void changeSound(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void changeEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("Effect", volume);
    }

    public void changeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("Music", volume);
    }

    IEnumerator restart()
    {
        restartTime.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            restartTime.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        restartTime.gameObject.SetActive(false);
        rb.constraints = RigidbodyConstraints.None;
        if (watchPrevActive)
        {
            StartWatch();
        }
        rb.velocity = preLeaveVel;
    }
}
