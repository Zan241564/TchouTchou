using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public string csvFilePath = "Assets/Texts/dialogues.csv";
    private Dictionary<string, string> dialogues = new Dictionary<string, string>();

    void Start()
    {
        LoadDialogues();
    }

    void LoadDialogues()
    {
        try
        {
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                bool isFirstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; // skip header line if there's one
                    }

                    var values = line.Split(',');
                    if (values.Length >= 2)
                    {
                        var id = values[0];
                        var text = values[1];
                        dialogues[id] = text;
                    }
                }
            }
            Debug.Log("Dialogues loaded successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading dialogues: " + e.Message);
        }
    }

    public string GetDialogueById(string id)
    {
        if (dialogues.ContainsKey(id))
        {
            return dialogues[id];
        }
        else
        {
            Debug.LogWarning("Dialogue ID not found: " + id);
            return string.Empty;
        }
    }
}
