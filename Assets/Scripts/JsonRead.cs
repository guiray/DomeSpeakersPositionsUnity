using System.IO;
using UnityEngine;

public class JsonRead : MonoBehaviour 
{
    //public string jsonPath = "Assets/Resources/temp_test.txt";
    public jsonTextFile = Resources.Load<TextAsset>("Text/temp_test.txt");

    public GameObject prefab;

    void Start()
    {
        //TestJson();

        //Resort le contenue du text au path donné
        string json = ReadTextFile(jsonPath);


        //Convert le json dans la classe
        SpeakersPositions speakersPositions = JsonUtility.FromJson<SpeakersPositions>(json);


        //Loop sur tous les positions
        for (int i = 0; i < speakersPositions.positions.Length; i++)
        {

            GameObject prefabInstance = Instantiate(prefab, transform.position,Quaternion.identity);
            prefabInstance.transform.position = speakersPositions.positions[i];

            //La ta la liste des positions 
            //speakersPositions[i].transform.position = speakersPositions.positions[i];
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


    void TestJson()
    {
        int sizePosition = 20;
        SpeakersPositions info = new SpeakersPositions();
        Vector3[] positions = new Vector3[sizePosition];

        for (int i = 0; i < sizePosition; i++)
        {
            positions[i] = Vector3.one;
        }

        info.positions = positions;
        string jsonString = JsonUtility.ToJson(info);

        Debug.Log(jsonString);
    }


}