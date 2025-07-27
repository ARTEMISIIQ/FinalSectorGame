using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// NOTE: Make sure to include the following namespace wherever you want to access Leaderboard Creator methods
using Dan.Main;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] _entryTextObjects;
    [SerializeField]
    private TMP_InputField _usernameInputField;
    [SerializeField]
    private UIManager uim;
    public Canvas c;
    [SerializeField]
    private TextMeshProUGUI currentScore;
    [SerializeField]
    private TextMeshProUGUI highScore;

    public int sceneNum;

    public void Start()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadEntries()
    {
        LoadData();
        if (sceneNum == 2)
        {
            Leaderboards.lvl1.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                {
                    t.text = "";
                }
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score} ms";
                }
            });
        }
        if (sceneNum == 3)
        {
            Leaderboards.lvl2.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                {
                    t.text = "";
                }
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score} ms";
                }
            });
        }
        if (sceneNum == 4)
        {
            Leaderboards.lvl3.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                {
                    t.text = "";
                }
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score} ms";
                }
            });
        }
        if (sceneNum == 5)
        {
            Leaderboards.lvl4.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                {
                    t.text = "";
                }
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score} ms";
                }
            });
        }
        if (sceneNum == 6)
        {
            Leaderboards.lvl5.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                {
                    t.text = "";
                }
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score} ms";
                }
            });
        }
        if (sceneNum == 7)
        {
            Leaderboards.tutorial.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                {
                    t.text = "";
                }
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score} ms";
                }
            });
        }
    }

    public void UploadEntry(float Score)
    {
        SaveData();
        if (sceneNum == 2)
        {
            Leaderboards.lvl1.UploadNewEntry(_usernameInputField.text, (int)(Score * 100), isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries();
                }
            });
        }
        if (sceneNum == 3)
        {
            Leaderboards.lvl2.UploadNewEntry(_usernameInputField.text, (int)(Score * 100), isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries();
                }
            });
        }
        if (sceneNum == 4)
        {
            Leaderboards.lvl3.UploadNewEntry(_usernameInputField.text, (int)(Score * 100), isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries();
                }
            });
        }
        if (sceneNum == 5)
        {
            Leaderboards.lvl4.UploadNewEntry(_usernameInputField.text, (int)(Score * 100), isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries();
                }
            });
        }
        if (sceneNum == 6)
        {
            Leaderboards.lvl5.UploadNewEntry(_usernameInputField.text, (int)(Score * 100), isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries();
                }
            });
        }
        if (sceneNum == 7)
        {
            Leaderboards.tutorial.UploadNewEntry(_usernameInputField.text, (int)(Score * 100), isSuccessful =>
            {
                if (isSuccessful)
                {
                    LoadEntries();
                }
            });
        }
    }

    public void SaveData()
    {
        if (PlayerPrefs.GetFloat(sceneNum.ToString()) != 0)
        {
            if (uim.time < PlayerPrefs.GetFloat(sceneNum.ToString()))
            {
                PlayerPrefs.SetFloat(sceneNum.ToString(), uim.time);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(sceneNum.ToString(), uim.time);
        }
    }

    public void LoadData()
    {
        highScore.text = "High Score: " + PlayerPrefs.GetFloat(sceneNum.ToString()).ToString() + " Secs";
    }
}