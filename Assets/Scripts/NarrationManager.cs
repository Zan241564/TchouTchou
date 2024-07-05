using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
     string csvFilePath = "TextesTrains";
    private Dictionary<string, string> narrationBlocks = new Dictionary<string, string>();

    [SerializeField]
    GameObject _bullePassager1;
    [SerializeField]
    GameObject _bullePassager2;
    [SerializeField]
    GameObject _bulleNarrateur;

    TMP_Text _textePassager1;
    TMP_Text _textePassager2;
    TMP_Text _texteNarrateur;

    RessourcesManager _ressourceManager;

    GameManager _gameManager;
    [SerializeField]
    string previousGameState;
    [SerializeField]
    string gameState;

    [SerializeField]
    GameObject _blackScreen;

    string idBloc;

    [SerializeField]
    int nombreMaxExitBloc;
    [SerializeField]
    int nombreMaxEntryBloc;
    [SerializeField]
    int nombreMaxActionBloc;

    string[] _nomBiomes = { "forest", "desert", "mountain" };
    string[] _nomActions = { "getfood", "getwater", "getmorale","leave" };

    void Start()
    {
        LoadNarrationBlocks();
        _ressourceManager = this.GetComponent<RessourcesManager>();

        _textePassager1 = _bullePassager1.GetComponentInChildren<TMP_Text>();
        _textePassager2 = _bullePassager2.GetComponentInChildren<TMP_Text>();
        _texteNarrateur = _bulleNarrateur.GetComponentInChildren<TMP_Text>();

        _gameManager = this.gameObject.GetComponent<GameManager>();
    }
    
    private void Update()
    {
        if (_gameManager.TrainActuallyStopped())
        {
            gameState = "gameplay";
        }
        else if (_blackScreen.activeSelf)
            {
            gameState = "fade to black";
            }
        else if (previousGameState == "gameplay" || previousGameState == "exitingBiome")
        {
            gameState = "exitingBiome";
        }
        else if (previousGameState == "fade to black" || previousGameState == "enteringBiome")
        {
            gameState = "enteringBiome";
        }
        if (gameState != previousGameState)
        {  
            if (gameState == "enteringBiome")
            {
                FinNarration();
                NarrationEntreeBiome();
            }
            if (gameState == "gameplay" || gameState == "fade to black") { 
                FinNarration();
            }
        }
        previousGameState = gameState;
    }
    
    void LoadNarrationBlocks()
    {
        //narrationBlocks = File.ReadLines(csvFilePath).Select(line => line.Split(";")).ToDictionary(line => line[0], line => line[1]);
        string str = Resources.Load<TextAsset>(csvFilePath).text;
        narrationBlocks = str.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
                                        .Select(line => line.Split(';'))
                                        .ToDictionary(line => line[0], line => line[1]);
    }

    public string GetNarrationByID(string id)
    {
        if (narrationBlocks.ContainsKey(id))
        {
            return narrationBlocks[id];
        }
        else
        {
            Debug.LogWarning("Dialogue ID not found: " + id);
            return string.Empty;
        }
    }

    public void NarrationEntreeBiome()
    {
        int biomeID = _gameManager.GetBiomeID();
        Debug.Log("entering biome");
        idBloc = "entering " + "normal " + _nomBiomes[biomeID] + " " + UnityEngine.Random.Range(1,1+ nombreMaxEntryBloc);
        _bulleNarrateur.gameObject.SetActive(true);
        _texteNarrateur.text = narrationBlocks[idBloc];

    }

    public void NarrationSortieBiome()
    {
        int biomeID = _gameManager.GetBiomeID();
        Debug.Log("exiting biome");
        idBloc = "exiting " +_nomBiomes[biomeID]+" "+ UnityEngine.Random.Range(1,1+nombreMaxExitBloc);
        _bulleNarrateur.gameObject.SetActive(true);
        _texteNarrateur.text = narrationBlocks[idBloc];

    }

    public void NarrationAction(int actionRef)
    {
        FinNarration();
        int biomeID = _gameManager.GetBiomeID();
        Debug.Log("narration d'action");

        idBloc = "action " + _nomActions[actionRef] +" "+ _nomBiomes[biomeID]+" "+ UnityEngine.Random.Range(1,1+nombreMaxActionBloc);
        string str = GetNarrationByID(idBloc);
        if(str == null || str == "" || str == string.Empty)
        {
            return;
        }
        _texteNarrateur.text = str;
        _bulleNarrateur.gameObject.SetActive(true);
    }

    public void FinNarration()
    {
        Debug.Log("fin de narration");
        _bullePassager1.gameObject.SetActive(false);
        _bullePassager2.gameObject.SetActive(false);
        _bulleNarrateur.gameObject.SetActive(false);
    }
}
