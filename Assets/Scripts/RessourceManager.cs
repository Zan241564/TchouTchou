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
    TMP_Text[] _ressourceDisplays;

    [SerializeField]
    GameObject[] _boutonsProchainArret;

    int[] _ressourceGauges = new int[4];

    GameManager _gameManager;
    bool _trainRunning;
    int _boutonParcoursCliqué;

    string[] _nomBiomes = { "Forest", "Desert", "Moutains" };

    //Cette matrice indique les bonus de rssource et malus de jour pour chaque action dans chaque biome. 
    //Chaque paire de valeurs correspond à la apire (bonus ressource, malus jours) d'une action.
    //Les actions sont listées dans l'ordre de biome et, dans chaque biome, dans l'ordre food, water, morale
    int[,] _biomeBonusMatrix = {{1, -1}, {1, -2}, {1,-3}, //biome 0
                                {1,-2},{1,-2},{1,-2},      // biome 1
                                {1,-2},{1,-2},{1,-2},};    //biome 2

    //Cette matrice indique, pour chaque biome, combien le joueur perd de chaque ressource en y entrant
    // L'ordre des ressources est food, water, morale
    int[,] _biomeMalusMatrix = { { 1,2,3},//biome 0
                                {2,3,4 },//biome 1
                                {3,4,5 }};//biome 2

    //Cette matrice donne les informations pour le parcours du train. 
    // La première dimension indique le niveau
    //La deuxième dimension indique le node dans ce niveau
    // La troisième dimension indique, pour chaque porchain node, le nb de jours bonus si on y va.
    // si la valeur est nulle, c'est que le trajet est impossible

    int[,,] _matriceParcours = {{{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } }};
    int _niveauParcours;
    int _currentBiomeID;

    // Dans cette matrice, chaque ligne correspond à  un bouton de l'UI. La colonne 1 indique à quel biome il mène, la colonne 2 cb de jours il fait gagner.
    int[,] _matriceCrspdcBoutonParcours = new int[3, 2];


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = this.GetComponent<GameManager>();

        // Début de partie
        _ressourceGauges[0] = 70;
        _ressourceGauges[1] = 70;
        _ressourceGauges[2] = 70;
        _ressourceGauges[3] = 10;

        for (int i = 0; i < 3; i++) {
            UpdateDisplay(_ressourceDisplays[i], _ressourceGauges[i]);
        }

        _trainRunning = false;
        _niveauParcours = 0;
        _currentBiomeID = 0;
        UpdateBoutonsProchainArret();

    }

    private void Update()
    {
        /*
         * if (_trainRunning){
         * if (! _gameManager.getvalue _trainrunnning) { LeTrainArrive(); }
         * }
         * 
         * 
         */
    }

    void UpdateDisplay(TMP_Text _display, int _valueGauge)
    {
        _display.text = _valueGauge.ToString();
    }

    void UpdateBiome(){
        _currentBiomeID = _gameManager.GetBiomeID();
        /*
        _foodBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 0, 0];
        _waterBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 1, 0];
        _moraleBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 2, 0];*/
    }

     void AddRessource(int _ressourceType, int _ressourceValue) {
        _ressourceGauges[_ressourceType] += _ressourceValue;
        if (_ressourceGauges[_ressourceType] < 0) { _ressourceGauges[_ressourceType] = 0; }
        if (_ressourceGauges[_ressourceType] == 0) { GameOver(); }

        UpdateDisplay(_ressourceDisplays[_ressourceType], _ressourceGauges[_ressourceType]);

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
                _nbBoutons++;

                _matriceCrspdcBoutonParcours[_nbBoutons, 0] = i;
                _matriceCrspdcBoutonParcours[_nbBoutons, 1] = _joursAProchainNoeud;

            }

        }
    }

    public void LeTrainPart(int _boutonIndex)
    {
        _boutonParcoursCliqué = _boutonIndex;
        _trainRunning  = true;
        
        //gameManager._nextBiomeID = _matriceCrspdcBoutonParcours[_boutonIndex, 0];
    }

    void LeTrainArrive()
    {
        _trainRunning = false;
        AddRessource(3,_matriceCrspdcBoutonParcours[_boutonParcoursCliqué, 1]);
        _currentBiomeID = _gameManager.GetBiomeID();
        UpdateBoutonsProchainArret();

        ConsommationInterBiome();
        _niveauParcours++;
    }

    void ConsommationInterBiome()
    {

    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }

}
