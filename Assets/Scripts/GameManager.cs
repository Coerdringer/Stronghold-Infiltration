using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private SceneData data;
    public static SaveData saveData;

    public GameObject checkpointReachedText;
    public GameObject mainMenu;
    public GameObject playerHasBeenDetectedScreenUI;
    public GameObject pauseMenuUI;
    public GameObject playerRef;
    public GameObject UI;

    public Vector3 lastCheckpointPosition;
    
    bool playerHasBeenSeen = false;
    public bool gameIsPaused = false;

    //========================================================================
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(playerRef);
            DontDestroyOnLoad(UI);
            DontDestroyOnLoad(checkpointReachedText);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            Destroy(playerRef);
            Destroy(UI);
            Destroy(checkpointReachedText);
        }

        data = FindObjectOfType<SceneData>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }
    public void SaveData(Vector3 lastCheckpointCoords)
    {
        saveData.savedPlayerPosition = lastCheckpointCoords;
        var gameData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("GameData", gameData);
    }

    public void LoadData()
    {
        var gameData = PlayerPrefs.GetString("GameData");
        saveData = JsonUtility.FromJson<SaveData>(gameData);
    }

    //=========================================================================

    private void Start()
    {
        DeactivateAllUI();
    }

    private void FixedUpdate()
    {
        

    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            DeactivateAllUI();
            //Debug.Log("ONLEVELWASLOADED");
            LoadData();
            playerRef.transform.position = saveData.savedPlayerPosition;
        }
    }

    public void Detection()
    {
        if (playerHasBeenSeen == false)
        {
            playerHasBeenSeen = true;
            Debug.Log("You have been caught!");
            //PlayerHasBeenSeen();
        }
    }

    void PlayerHasBeenSeen ()
    {
        playerHasBeenDetectedScreenUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        data.pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void Pause()
    {
        data.pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (gameIsPaused)
            Resume();
        else
            Pause();
    }

    void DeactivateAllUI()
    {
        data.pauseMenuUI.SetActive(false);
        data.playerHasBeenDetectedScreenUI.SetActive(false);
        data.mainMenu.SetActive(false);
        data.checkpointReachedText.SetActive(false);
        //Debug.Log("Deactivated!");
    }

    public void ReloadFromCheckpoint()
    {
        Resume();
        StartCoroutine(ReloadGame());
        //Debug.Log("Reloaded!");
    }

    AsyncOperation asyncLoadLevel;

    IEnumerator ReloadGame()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(0);
        while(!asyncLoadLevel.isDone)
        {
            yield return null;
        }
    }
}
