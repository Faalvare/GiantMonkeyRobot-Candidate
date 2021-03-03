using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject titlePrefab;
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject headerTextPrefab;
    [SerializeField] private GameObject textPrefab;
    private string path = "Assets/StreamingAssets/JsonChallenge.json";
    private void OnEnable()
    {
        ShowJsonContentOnScreen();
    }

    public void ShowJsonContentOnScreen()
    {
        ClearData();
        //Read text file
        StreamReader reader = new StreamReader(path);
        string jsonString = reader.ReadToEnd();
        reader.Close();

        //Transform json text into readable data
        jsonData _jsonData = JsonConvert.DeserializeObject<jsonData>(jsonString);

        //Instantiating title
        GameObject titleObj = Instantiate(titlePrefab, container.transform);
        titleObj.GetComponent<Text>().text = _jsonData.Title;

        //instantiating header columns
        GameObject headerRow = Instantiate(rowPrefab,container.transform);
        foreach (string header in _jsonData.ColumnHeaders)
        {
            GameObject headerTextObj = Instantiate(headerTextPrefab,headerRow.transform);
            headerTextObj.GetComponent<Text>().text = header;
        }

        //Instantiating Data Rows
        foreach (Dictionary<string,string> rowData in _jsonData.Data)
        {
            GameObject dataRow = Instantiate(rowPrefab, container.transform);
            foreach (KeyValuePair<string,string> data in rowData)
            {
                GameObject dataTextObj = Instantiate(textPrefab, dataRow.transform);
                dataTextObj.GetComponent<Text>().text = data.Value;
            }
        }
        Canvas.ForceUpdateCanvases();
    }

    public void ClearData()
    {
        foreach (Transform children in container.transform)
        {
            if (children != container.transform)
            {
                Destroy(children.gameObject);
            }
        }
    }
}

#region Data_Classes

public class jsonData
{
    public string Title;
    public string[] ColumnHeaders;
    public List<Dictionary<string,string>> Data;
}

#endregion
