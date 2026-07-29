using System.IO;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriter;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private CheckpointData checkpointData;

    private void Start()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
        if(File.Exists(loadPath))
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.instance.Load(helpCheck, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
            if(helpCheck.checkPointKey == checkpointData.checkPointKey)
            {
                spriter.sprite = spriteEnabled;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkPoint = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkPoint = null;
        }
    }

    public void Activate()
    {
        spriter.sprite = spriteEnabled;
        //save data
        SaveLoadManager.instance.Save(checkpointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
    }

}
