using UnityEngine;

public class TestSaveLoad : MonoBehaviour
{
    public ExampleData someData;

    private void Start()
    {
        SaveLoadManager.instance.DeleteExample("testData.json");
        SaveLoadManager.instance.LoadExample(someData, "testData.json");    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            someData.exampleInt = 5;
            someData.exampleString = "AP";
            SaveLoadManager.instance.SaveExample(someData, "testData.json");
        }
    }
}
