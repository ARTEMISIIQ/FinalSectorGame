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
    public UIManager uim;
    public LeaderboardManager lm;
    public AudioScript am;
    public Button submit;
    [SerializeField]
    private TextMeshProUGUI newScore;
    [SerializeField]
    public List<int> checks;

    float vel;
    public int checkpointNum;
    bool boost = false;
    private void Start()
    {
        checkpointNum = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (vel == 0)
        {
            vel = rb.velocity.magnitude;
        }
        if (other.tag == "Start" && uim.time == 0)
        {
            uim.StartWatch();
            uim.watchPrevActive = true;
            lastCheckpoint = other.transform;
            am.complete = false;

        }
        if (other.tag == "End" && checkpointNum == checks[SceneManager.GetActiveScene().buildIndex-2])
        {
            uim.StopWatch();
            newScore.text = "Current Score: " + uim.stopwatch.text + "Seconds";
            lm.LoadEntries();
            lm.c.gameObject.SetActive(true);
            submit.onClick.AddListener(() => lm.UploadEntry(uim.time));
            am.complete = true;
            am.engine = false;
        }
        if (other.tag == "Checkpoint")
        {
            lastCheckpoint = other.transform;
            other.gameObject.SetActive(false);
            checkpointNum++;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Booster" && !boost)
        {
            StartCoroutine(Boost());
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
        if (Input.GetKeyDown(KeyCode.R) && lastCheckpoint && checkpointNum > 0)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = lastCheckpoint.position;
            transform.rotation = lastCheckpoint.rotation;
            rb.velocity = new Vector3(0, 0, 0);
            rb.constraints = RigidbodyConstraints.None;
        }
        else if (Input.GetKeyDown(KeyCode.R) && lastCheckpoint)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = lastCheckpoint.position + lastCheckpoint.transform.TransformDirection(0, 0, -10);
            transform.rotation = lastCheckpoint.rotation;
            uim.StopWatch();
            uim.time = 0;
            rb.velocity = new Vector3(0, 0, 0);
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
