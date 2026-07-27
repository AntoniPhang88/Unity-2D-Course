[System.Serializable]
public class ExampleData
{
    public int exampleInt;
    public string exampleString;
}
[System.Serializable]
public class SpawnData
{
    public string spawnPointKey;
    public bool facingRight;

    public SpawnData()
    {
        spawnPointKey = "Start";
        facingRight = true;
    }
}
