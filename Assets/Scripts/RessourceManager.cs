using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RessourceManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text _foodDisplay;
    [SerializeField]
    TMP_Text _waterDisplay;
    [SerializeField]
    TMP_Text _moraleDisplay;
    [SerializeField]
    TMP_Text _daysDisplay;

    [SerializeField]
    GameObject[] _boutonsProchainArret;

    int _foodGauge;
    int _waterGauge;
    int _moraleGauge;
    int _daysGauge;

    int _foodBiomeBonus;
    int _waterBiomeBonus;
    int _moraleBiomeBonus;

    GameManager _gameManager;

    int _currentBiomeID;

    string[] _nomBiomes = { "Forest", "Desert", "Moutains" };

    //Cette matrice indique les bonus de rssource et malus de jour pour chaque action dans chaque biome. 
    //Chaque paire de valeurs correspond � la apire (bonus ressource, malus jours) d'une action.
    //Les actions sont list�es dans l'ordre de biome et, dans chaque biome, dans l'ordre food, water, morale
    int[,] _biomeBonusMatrix = {{1, -1}, {1, -2}, {1,-3}, //biome 0
                                {1,-2},{1,-2},{1,-2},      // biome 1
                                {1,-2},{1,-2},{1,-2},};    //biome 2

    //Cette matrice indique, pour chaque biome, combien le joueur perd de chaque ressource en y entrant
    // L'ordre des ressources est food, water, morale
    int[,] _biomeMalusMatrix = { { 1,2,3},//biome 0
                                {2,3,4 },//biome 1
                                {3,4,5 }};//biome 2

    //Cette matrice donne les informations pour le parcours du train. 
    // La premi�re dimension indique le niveau
    //La deuxi�me dimension indique le node dans ce niveau
    // La troisi�me dimension indique, pour chaque porchain node, le nb de jours bonus si on y va.
    // si la valeur est nulle, c'est que le trajet est impossible

    int[,,] _matriceParcours = {{{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } },
                                {{ 0,1,2}, { 1, 2, 3 }, { 0, 0, 2 } }};


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = this.GetComponent<GameManager>();

        // D�but de partie
        _foodGauge = 70;
         _waterGauge = 70;
         _moraleGauge = 70;
         _daysGauge = 10;

        UpdateDisplay(_foodDisplay, _foodGauge);
        UpdateDisplay(_waterDisplay, _waterGauge);
        UpdateDisplay(_moraleDisplay, _moraleGauge);
        UpdateDisplay(_daysDisplay, _daysGauge);

        // R�cup�ration info biome
        UpdateBiome();

        UpdateBoutonsProchainArret();

    }

    void UpdateDisplay(TMP_Text _display, int _valueGauge)
    {
        _display.text = _valueGauge.ToString();
    }

    void UpdateBiome(){
        _currentBiomeID = _gameManager.GetBiomeID();

        _foodBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 0, 0];
        _waterBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 1, 0];
        _moraleBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 2, 0];
    }

    public void AddFood() {
        _foodGauge += _foodBiomeBonus;
        if (_foodGauge < 0) { _foodGauge = 0; }
        if (_foodGauge == 0) { GameOver(); }

        UpdateDisplay(_foodDisplay, _foodGauge);

        AddDays(GetDayCostForAction(0));
    }

    public void AddWater()
    {
        _waterGauge += _waterBiomeBonus;
        if (_waterGauge < 0) { _waterGauge = 0; }
        if (_waterGauge == 0) { GameOver(); }

        UpdateDisplay(_waterDisplay, _waterGauge);

        AddDays(GetDayCostForAction(1));
    }

    public void AddMorale()
    {
        _moraleGauge += _moraleBiomeBonus;
        if (_moraleGauge < 0) { _moraleGauge = 0; }
        if (_moraleGauge == 0) { GameOver(); }
        UpdateDisplay(_moraleDisplay, _moraleGauge);

        AddDays(GetDayCostForAction(2));
    }

    int  GetDayCostForAction(int _actionID)
    {
        return _biomeBonusMatrix[_currentBiomeID * 3 + _actionID, 1];
    }

    public void AddDays(int _daysBiomeBonus)
    {
        _daysGauge += _daysBiomeBonus;
        if (_daysGauge < 0) { _daysGauge = 0; }
        if (_daysGauge == 0) { GameOver(); }

        UpdateDisplay(_daysDisplay, _daysGauge);
    }

     void UpdateBoutonsProchainArret()
    {
        int _nbBoutons = 0;
        for(int i=0; i<3; i++)
        {
            int _joursAProchainNoeud = _matriceParcours[1, 1, i];
            string _nomProchainNoeud = _nomBiomes[i];
            if(_joursAProchainNoeud != 0) {
                _boutonsProchainArret[_nbBoutons].gameObject.SetActive(true);
                _boutonsProchainArret[_nbBoutons].GetComponentInChildren<TMP_Text>().text = _nomProchainNoeud + " " + _joursAProchainNoeud;
                _nbBoutons++;
            }
            
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }

}
