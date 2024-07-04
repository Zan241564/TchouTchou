using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RessourceManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] _endingScreens;

    [SerializeField]
    GameObject[] _permanentContentList;

    [SerializeField]
    TMP_Text[] _ressourceDisplays;

    [SerializeField]
    GameObject _canvasActions;
    [SerializeField]
    GameObject[] _boutonsActions;
    [SerializeField]
    GameObject _canvasDestinations;
    [SerializeField]
    GameObject[] _boutonsProchainArret;


    int[] _ressourceGauges = new int[4];

    GameManager _gameManager;
    bool _noResources = false;
    bool _trainRunning;
    int _boutonParcoursChoisi;

    string[] _nomBiomes = { "Forest", "Desert", "Moutains" };
    string[] _nomRessources = { "Food", "Water", "Morale" };

    //Cette matrice indique les bonus de rssource et malus de jour pour chaque action dans chaque biome. 
    //Chaque paire de valeurs correspond à la paire (bonus ressource, malus jours) d'une action.
    //Les actions sont listées dans l'ordre de biome et, dans chaque biome, dans l'ordre food, water, morale
    int[,] _biomeBonusMatrix = {{30, -3}, {20, -2}, {10,-2}, //biome 0
                                {0,0},{10,-3},{15,-2},      // biome 1
                                {15,-4},{25,-3},{10,-1},};    //biome 2

    //Cette matrice indique, pour chaque biome, combien le joueur perd de chaque ressource en y entrant
    // L'ordre des ressources est food, water, morale
    int[,] _biomeMalusMatrix = { { -25,-25,0},//biome 0
                                {-25,-45,-35 },//biome 1
                                {-45,-25,-20 }};//biome 2

    //Cette matrice donne les informations pour le parcours du train. 
    // La première dimension indique le niveau
    //La deuxième dimension indique le node dans ce niveau
    // La troisième dimension indique, pour chaque porchain node, le nb de jours bonus si on y va.
    // si la valeur est nulle, c'est que le trajet est impossible

    int[,,] _matriceParcours = {{{ 3,7,5}, { 4, 2, 4 }, { 3, 4, 4 } },
                                {{ 5,3,3}, { 3, 6, 5 }, { 2, 5, 3 } },
                                {{ 2,5,6}, { 3, 3, 6 }, { 4, 2, 5 } },
                                {{ 3,4,4}, { 3, 5, 3 }, { 2, 2, 3 } },
                                {{ 1,2,2}, { 1, 4, 2 }, { 1, 3, 3 } }};
    int _niveauParcours;
    int _currentBiomeID;

    // Dans cette matrice, chaque ligne correspond à  un bouton de l'UI. La colonne 1 indique à quel biome il mène, la colonne 2 cb de jours il fait gagner.
    int[,] _matriceCrspdcBoutonParcours = new int[3, 2];


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = this.GetComponent<GameManager>();
        _endingScreens[3].SetActive(true);

        // Début de partie
        _ressourceGauges[0] = 100;
        _ressourceGauges[1] = 100;
        _ressourceGauges[2] = 100;
        _ressourceGauges[3] = 10;

        for (int i = 0; i < 3; i++) {
            UpdateDisplay(_ressourceDisplays[i], _ressourceGauges[i]);
        }
        UpdateDisplay(_ressourceDisplays[3], _ressourceGauges[3], true);

        _trainRunning = false;
        _niveauParcours = 0;
        UpdateBiome();
        UpdateBoutonsActions();
        UpdateBoutonsProchainArret();

    }

    private void Update()//Vérifie quand le trian s'est arreté pour changer l'UI
    {
         if (_trainRunning){
            if ( _gameManager.TrainActuallyStopped()) {
                LeTrainArrive(); }
         }
    }

    //Change lUI pour les ressources
    void UpdateDisplay(TMP_Text _display, int _valueGauge, bool time = false)
    {
        if (time) {
            _display.text = _valueGauge.ToString() + " days";
        }
        else
        {

            _display.text = _valueGauge.ToString() + "/100";
        }
    }

    void UpdateBiome(){
        _currentBiomeID = _gameManager.GetBiomeID();
    }

     void AddRessource(int _ressourceType, int _ressourceValue) {
        _ressourceGauges[_ressourceType] += _ressourceValue;
        if (_ressourceGauges[_ressourceType] < 0) { _ressourceGauges[_ressourceType] = 0; }
        if (_ressourceGauges[_ressourceType] > 100) { _ressourceGauges[_ressourceType] = 100; }
        if (_ressourceGauges[_ressourceType] == 0) { GameOver(); }

        if (_ressourceType != 3)
        {

            UpdateDisplay(_ressourceDisplays[_ressourceType], _ressourceGauges[_ressourceType]);
        }
        else
        {
            UpdateDisplay(_ressourceDisplays[_ressourceType], _ressourceGauges[_ressourceType],true);
        }

    }

    public void ActionOnRessource(int _actionID)
    {
        AddRessource(_actionID, _biomeBonusMatrix[_currentBiomeID * 3 + _actionID, 0]);
        AddRessource(3, _biomeBonusMatrix[_currentBiomeID * 3 + _actionID, 1]);
    }


    int  GetDayCostForAction(int _actionID)
    {
        return _biomeBonusMatrix[_currentBiomeID * 3 + _actionID, 1];
    }

    /// <summary>
    /// Met à jours les 3 boutons de prochain arret 
    /// </summary>
     void UpdateBoutonsProchainArret()
    {

        Array.Clear(_matriceCrspdcBoutonParcours, 0, _matriceCrspdcBoutonParcours.Length);
        int _nbBoutons = 0;
        for(int i=0; i<3; i++)
        {
            int _joursAProchainNoeud = _matriceParcours[_niveauParcours, _currentBiomeID, i];
            string _nomProchainNoeud = _nomBiomes[i];
            if (_joursAProchainNoeud != 0) {
                _boutonsProchainArret[_nbBoutons].gameObject.SetActive(true);
                _boutonsProchainArret[_nbBoutons].GetComponentInChildren<TMP_Text>().text = _nomProchainNoeud + " " + _joursAProchainNoeud;

                _matriceCrspdcBoutonParcours[_nbBoutons, 0] = i;
                _matriceCrspdcBoutonParcours[_nbBoutons, 1] = _joursAProchainNoeud;

                _nbBoutons++;
            }

        }
    }

    void UpdateBoutonsActions()
    {
            int _nbBoutons = 0;
        for (int i = 0; i < 3; i++)
        {
            int _gainRessource = _biomeBonusMatrix[_currentBiomeID * 3 + i, 0];
            int _perteJours = _biomeBonusMatrix[_currentBiomeID * 3 + i, 1] * -1 ;
            if (_gainRessource != 0)
            {
                _boutonsActions[_nbBoutons].gameObject.SetActive(true);
                _boutonsActions[_nbBoutons].GetComponentInChildren<TMP_Text>().text = "Get " + _gainRessource + " "+  _nomRessources[i] + " in " + _perteJours + " days";


                _nbBoutons++;
            }
            else
            {
                _boutonsActions[_nbBoutons].gameObject.SetActive(false);
                _nbBoutons++;
            }

        }
    }

    public void ClickOnLeave()
    {
        _canvasActions.SetActive(false);
        _canvasDestinations.SetActive(true);
    }

    public void LeTrainPart(int _boutonIndex)
    {
        _boutonParcoursChoisi = _boutonIndex;
        _trainRunning  = true;
        
        _gameManager.StartTrain( _matriceCrspdcBoutonParcours[_boutonIndex, 0]);
        _canvasDestinations.SetActive(false);
    }

    /// <summary>
    /// fonction d'volution des paramètres quand le train s'arrete dans el nouveau biome
    /// </summary>
    void LeTrainArrive()
    {
        _trainRunning = false;
        AddRessource(3,_matriceCrspdcBoutonParcours[_boutonParcoursChoisi, 1]);
        _currentBiomeID = _gameManager.GetBiomeID();

        _canvasActions.SetActive(true);
        UpdateBoutonsActions();
        UpdateBoutonsProchainArret();

        ConsommationInterBiome();

        _niveauParcours++;
        if(_niveauParcours == 5)
        {
            GameWin();
        }
    }

    /// <summary>
    /// Fonction pour l'effet sur ressourcs du changement de biome
    /// </summary>
    void ConsommationInterBiome()
    {
        for(int i = 0; i< 3; i++)
        {
            AddRessource(i, _biomeMalusMatrix[_currentBiomeID,i]);
        }
    }

    /// <summary>
    /// Méthode qui désactive tout le contenu de l'interface graphique en cas de victoire/défaite.
    /// Prend les éléments graphiques du PermanentContentCanvas (à l'exception des écrans de début et de fin) et les 3 autres Canvas au sein d'une liste.
    /// </summary>
    void DisablePermanentContent()
    {
        foreach (var item in _permanentContentList)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// Méthode qui affiche les écrans de fin "Losing1" et "Losing2" en cas de perte en les réactivant au sein du PermanentContentCanvas
    /// Écran Losing1 : Perte à cause des ressources
    /// Écran Losing1 : Perte à cause de la catastrophe
    /// </summary>
    void GameOver()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log( _nomRessources[i] + "- Valeur :" + _ressourceGauges[i]);

            if (_ressourceGauges[i] == 0) { _noResources = true;}
            Debug.Log(_noResources);

            if (_noResources)
            {
                //FadeIn(_endingScreens[0]);
                _endingScreens[0].SetActive(true);
                _endingScreens[1].SetActive(false);

            } else {
                _endingScreens[0].SetActive(false);
                //FadeIn(_endingScreens[1]);
                _endingScreens[1].SetActive(true);    
            }
            DisablePermanentContent();

        }
    }

    /// <summary>
    /// Méthode qui affiche l'écran de victoire en cas de survie à la catastrophe.
    /// </summary>
    void GameWin()
    {
        _endingScreens[2].SetActive(true);
        //FadeIn(_endingScreens[2]);
        DisablePermanentContent();
    }

    public int getRemainingDays()
    {
        return _ressourceGauges[3];
    }


}
