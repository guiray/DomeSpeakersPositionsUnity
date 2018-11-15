using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JsonRead : MonoBehaviour 
{
    

    //public string jsonPath = "Assets/Resources/Configs/speakersPositions_167channels.json";
    //public string jsonPath = "Assets/Resources/Configs/";

    public GameObject prefab;

    public enum Dropdown { Left, Right, Top, Bottom }

    public Dropdown speakerConfiguration;

    

    void Start()
    {

        string[] filePaths = Directory.GetFiles(string "Assets/Resources/Configs/", SearchOption.TopDirectoryOnly);

        Debug.Log(filePaths);

        //Resort le contenue du text au path donné
        string json = ReadTextFile(jsonPath);

        

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