using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;

        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public FloatingTextManager floatingTextManager;

    // Logic
    public int gold;
    public int experience;

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Save state
    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += gold.ToString() + "|";
        s += experience.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);
    }
    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        if(!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] loadData = PlayerPrefs.GetString("SaveState").Split('|');

        // Change player skin
        gold = int.Parse(loadData[1]);
        experience = int.Parse(loadData[2]);

        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
    public void AddGold(int amount)
    {
        gold += amount;
    }
    public void RemoveGold(int amount)
    {
        gold -= amount;
    }
    public void AddExperience(int amount)
    {
        experience += amount;
    }
}
