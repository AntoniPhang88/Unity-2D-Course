using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathAbility : BaseAbility
{
    public override void EnterAbility()
    {
        SpawnMode.spawnFromCheckpoint = true;
        player.gatherInput.DisablePlayerMap();
        linkedPhysics.ResetVelocity();
        if(linkedPhysics.grounded)
            linkedAnimator.SetBool("Death", true);
        else
        {
            //if have other death animation
            linkedAnimator.SetBool("Death", true);
        }
    }
    //public override void UpdateAnimator()
    //{
    //    linkedAnimator.SetBool("Death", linkedStateMachine.currentState == PlayerStates.State.Death);
    //}

    public void ResetGame()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
        if (File.Exists(loadPath))
        {
            CheckpointData checkData = new CheckpointData();
            SaveLoadManager.instance.Load(checkData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
            LevelManager.instance.LoadLevelString(checkData.sceneToLoad);
        }
        else
        {
            LevelManager.instance.RestartLevel();
        }
    }
}
