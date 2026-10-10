using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private void Start()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if(File.Exists(loadPath))
        {
            continueButton.SetActive(true);
            EventSystem.current.firstSelectedGameObject = continueButton;
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
    public void NewGame()
    {
        SaveLoadManager.instance.DeleteFolder(SaveLoadManager.instance.folderName);
        LevelManager.instance.LoadLevelString("Level 1");
    }
    public void ContinueGame()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        SpawnMode.spawnFromCheckPoint = true;
        if (File.Exists(loadPath))
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.instance.Load(helpCheck, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            LevelManager.instance.LoadLevelString(helpCheck.sceneToLoad);
        }
        else
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.instance.Save(helpCheck, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            LevelManager.instance.LoadLevelString(helpCheck.sceneToLoad);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
