using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class FloorBehaviors : MonoBehaviour
{
    [SerializeField]
    float boosterBuff;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Transform lastCheckpoint;
    [SerializeField]
    Transform startArea;
    public UIManager uim;
    public LeaderboardManager lm;
    public AudioScript am;
    public Button submit;
    [SerializeField]
    private TextMeshProUGUI newScore;
    [SerializeField]
    public List<int> checks;
    public List<GameObject> checkPoints;
    float vel;
    public int checkpointNum;
    public int roundNum;
    bool boost = false;
    public bool duraStart;
    public bool atPitStop;
    private void Start()
    {
        checkpointNum = checks[SceneManager.GetActiveScene().buildIndex - 2] / 3;
        roundNum = 0;
        uim.durability.value = 1;
        duraStart = false;
        atPitStop = false;
        uim.durabilityTime = 100 + 20 * PlayerPrefs.GetFloat("Span");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (vel == 0)
        {
            vel = rb.velocity.magnitude;
        }
        if (other.tag == "Start")
        {
            if (uim.time == 0)
            {
                uim.StartWatch();
                uim.watchPrevActive = true;
                am.complete = false;
                duraStart = true;
            }
            if (checkpointNum >= checks[SceneManager.GetActiveScene().buildIndex - 2] / 3)
            {
                roundNum++;
                checkpointNum = 0;
                uim.roundNum.text = "Lap " + roundNum.ToString() + "/3";
            }
            if (roundNum > 3)
            {
                uim.StopWatch();
                newScore.text = "Current Score: " + uim.stopwatch.text + "Seconds";
                lm.LoadEntries();
                lm.c.gameObject.SetActive(true);
                submit.onClick.AddListener(() => lm.UploadEntry(uim.time));
                am.complete = true;
                am.engine = false;
            }
            foreach (GameObject point in checkPoints)
            {
                point.SetActive(true);
            }
            lastCheckpoint = other.transform;
        }
        if (other.tag == "Checkpoint")
        {
            lastCheckpoint = other.transform;
            other.gameObject.SetActive(false);
            checkpointNum++;
        }
        if (other.tag == "End") // End = TP
        {
            checkpointNum++;
            transform.position = lastCheckpoint.position + lastCheckpoint.transform.TransformDirection(0, 0, -10);
            transform.rotation = lastCheckpoint.rotation;
            transform.Rotate(0, 180, 0);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Booster" && !boost)
        {
            StartCoroutine(Boost());
        }
        if (other.tag == "PitStop" && rb.velocity.magnitude < 1)
        {
            if (!atPitStop)
            {
                atPitStop = true;
                StartCoroutine(PitStop());
            }
        }
    }
    IEnumerator Boost()
    {
        boost = true;
        rb.velocity = Vector3.Normalize(rb.velocity) * (rb.velocity.magnitude + boosterBuff);
        yield return new WaitForSeconds(3f);
        boost = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && lastCheckpoint && (roundNum > 1 || checkpointNum > 0))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = lastCheckpoint.position;
            transform.rotation = lastCheckpoint.rotation;
            rb.velocity = new Vector3(0, 0, 0);
            transform.Rotate(0, 180, 0);
            rb.constraints = RigidbodyConstraints.None;
            duraStart = false;
        }
        else if (Input.GetKeyDown(KeyCode.R) && lastCheckpoint)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = lastCheckpoint.position + lastCheckpoint.transform.TransformDirection(0, 0, -10);
            transform.rotation = lastCheckpoint.rotation;
            transform.Rotate(0, 180, 0);
            uim.StopWatch();
            uim.time = 0;
            rb.velocity = new Vector3(0, 0, 0);
            rb.constraints = RigidbodyConstraints.None;
            uim.durability.value = 1;
            duraStart = false;
        }
        if (duraStart && uim.durability.value > 0 && !atPitStop)
        {
            StartCoroutine(duraDecrease());
        }
        uim.durabilityPercent.text = Mathf.Round(uim.durability.value * 100).ToString() + "%";
    }

    IEnumerator duraDecrease()
    {
        duraStart = false;
        yield return new WaitForSeconds(1);
        uim.durability.value -= 1 / uim.durabilityTime;
        duraStart = true;
    }

    IEnumerator PitStop()
    {
        while (uim.durability.value < 1)
        {
            yield return new WaitForSeconds(0.05f);
            uim.durability.value += 1 / uim.durabilityTime;
        }
        atPitStop = false;
    }    
}
