using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button button;
    private SpawnManager spawnManager;

    void Start()
    {
        button = GetComponent<Button>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        button.onClick.AddListener(SetStatus);
    }

    void SetStatus()
    {
        Debug.Log("WAVE STARTING NOW!");
        spawnManager.StartGame();
    }
}

