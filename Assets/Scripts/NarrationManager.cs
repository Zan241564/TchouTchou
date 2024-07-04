using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public string csvFilePath = "Assets/Texts/fakeText.csv";
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

    RessourceManager _ressourceManager;

    GameManager _gameManager;
    [SerializeField]
    string previousGameState;
    [SerializeField]
    string gameState;
    string idBloc;

    [SerializeField]
    int nombreMaxExitBloc;

    string[] _nomBiomes = { "Forest", "Desert", "Moutains" };

    void Start()
    {
        LoadNarrationBlocks();
        Debug.Log(GetNarrationByID("1"));
        _ressourceManager = this.GetComponent<RessourceManager>();

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
        else if (_gameManager._trainStop)
        {
            gameState = "enteringBiome";
        }
        else
        {
            gameState = "exitingBiome";
        }
        if (gameState != previousGameState)
        {  
            if (gameState == "enteringBiome")
            {
                FinNarration();
                NarrationEntreeBiome(_gameManager.GetBiomeID());
            }
            if (gameState == "gameplay") { 
                FinNarration();
            }
        }
        previousGameState = gameState;
    }

    void LoadNarrationBlocks()
    {
         narrationBlocks = File.ReadLines(csvFilePath).Select(line => line.Split(";")).ToDictionary(line => line[0], line => line[1]);
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

    public void NarrationEntreeBiome(int biomeID)
    {
        Debug.Log("entering biome");
        idBloc = "Entering " + " normal " + _nomBiomes[biomeID] + " " + Random.Range(0, nombreMaxExitBloc);
        _bulleNarrateur.gameObject.SetActive(true);
        //idBloc = "1";
        _texteNarrateur.text = narrationBlocks[idBloc];

    }

    public void NarrationSortieBiome()
    {
        int biomeID = _gameManager.GetBiomeID();
        Debug.Log("exiting biome");
        idBloc = "Exiting" +" "+" "+_nomBiomes[biomeID]+" "+ Random.Range(0,nombreMaxExitBloc);
        //idBloc = "2";
        _bulleNarrateur.gameObject.SetActive(true);
        _texteNarrateur.text = narrationBlocks[idBloc];

    }

    public void NarrationAction(int actionRef)
    {
        int biomeID = _gameManager.GetBiomeID();
        Debug.Log("narration d'action");
        idBloc = "Exiting" +" "+" "+_nomBiomes[biomeID]+" "+ Random.Range(0,nombreMaxExitBloc);
        //idBloc = (actionRef+3).ToString();
        _bulleNarrateur.gameObject.SetActive(true);
        _texteNarrateur.text = narrationBlocks[idBloc];
    }

    public void NarrationDepart()
    {
        int biomeID = _gameManager.GetBiomeID();
        Debug.Log("narration d'action");
        idBloc = "Exiting" +" "+" "+_nomBiomes[biomeID]+" "+ Random.Range(0,nombreMaxExitBloc);
        //idBloc = "6";
        _bulleNarrateur.gameObject.SetActive(true);
        _texteNarrateur.text = narrationBlocks[idBloc];
    }

    public void FinNarration()
    {
        Debug.Log("fin de narration");
        _bullePassager1.gameObject.SetActive(false);
        _bullePassager2.gameObject.SetActive(false);
        _bulleNarrateur.gameObject.SetActive(false);
    }
}
