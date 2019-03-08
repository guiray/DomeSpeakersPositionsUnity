using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JsonRead : MonoBehaviour 
{

    private string jsonDirectory = "Assets/Resources/Configs/";

    public GameObject prefab;

    private List<string> configJsonList = new List<string>();

    public enum configSelector { };
    public configSelector ConfigSelector;

    void Start()
    {

        // get path for each files in this directory
        DirectoryInfo dir = new DirectoryInfo(jsonDirectory);
        FileInfo[] info = dir.GetFiles("*.json");

        //get all json file name in the config directory
        foreach (FileInfo f in info)
        {
            string[] pathArray = f.ToString().Split('\\');
            string lastItem = pathArray[pathArray.Length - 1];

            configJsonList.Add(lastItem);

        }

        



        //Resort le contenue du text au path donné
        string json = ReadTextFile("Assets/Resources/Configs/speakersPositions_167channels.json");

        //Convert le json dans la classe
        SpeakersPositions speakersPositions = JsonUtility.FromJson<SpeakersPositions>(json);

        //Loop sur tous les positions
        for (int i = 0; i < speakersPositions.positions.Length; i++)
        {
            Vector3 position = speakersPositions.positions[i];
            Vector3 ajustedPosition = new Vector3(position.x, position.z, position.y);

            GameObject prefabInstance = Instantiate(prefab, transform.position,Quaternion.identity);
            prefabInstance.transform.position = ajustedPosition;

            
        }

    }


    //Tu donne un path pis y te resort le contenue
    string ReadTextFile(string path)
    {
        System.IO.StreamReader reader = new StreamReader(path);
        string result = reader.ReadToEnd();
        reader.Close();
        return result;
    }


}