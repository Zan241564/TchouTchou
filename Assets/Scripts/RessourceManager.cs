using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

    int _foodGauge;
    int _waterGauge;
    int _moraleGauge;
    int _daysGauge;

    int _foodBiomeBonus;
    int _waterBiomeBonus;
    int _moraleBiomeBonus;
    int _daysBiomeBonus;

    int _currentBiomeID;

    //Cette matrice indique les bonus de rssource et malus de jour pour chaque action dans chaque biome. 
    //Chaque paire de valeurs correspond à la apire (bonus ressource, malus jours) d'une action.
    //Les actions sont listées dans l'ordre de biome et, dans chaque biome, dans l'ordre food, water, morale
    int[,] _biomeBonusMatrix = {{1, 2}, {1, 2}, {1, 2}, //biome 0
                                {1,2},{1,2},{1,2},      // biome 1
                                {1,2},{1,2},{1,2},};    //biome 2

    // Start is called before the first frame update
    void Start()
    {
        // Début de partie
         _foodGauge = 70;
         _waterGauge = 70;
         _moraleGauge = 70;
         _daysGauge = 70;

        UpdateDisplay(_foodDisplay, _foodGauge);
        UpdateDisplay(_waterDisplay, _waterGauge);
        UpdateDisplay(_moraleDisplay, _moraleGauge);
        UpdateDisplay(_daysDisplay, _daysGauge);

        // Récupération info biome
        UpdateBiome();

    }

    // Update is called once per frame
    void UpdateDisplay(TMP_Text _display, int _valueGauge)
    {
        _display.text = _valueGauge.ToString();
    }

    void UpdateBiome(){
        _currentBiomeID = this.GetComponent<GameManager>().GetBiomeID();

        _foodBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 0, 0];
        _waterBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 1, 0];
        _moraleBiomeBonus = _biomeBonusMatrix[_currentBiomeID * 3 + 2, 0];

    }

    public void AddFood() {
        _foodGauge += _foodBiomeBonus;
        UpdateDisplay(_foodDisplay, _foodGauge);
        if (_foodGauge < 0) { _foodGauge = 0; }
        if (_foodGauge == 0) { GameOver(); }
    }

    public void AddWater()
    {
        _waterGauge += _waterBiomeBonus;
        UpdateDisplay(_waterDisplay, _waterGauge);
        if (_waterGauge < 0) { _waterGauge = 0; }
        if (_waterGauge == 0) { GameOver(); }
    }

    public void AddMorale()
    {
        _moraleGauge += _moraleBiomeBonus;
        UpdateDisplay(_moraleDisplay, _moraleGauge);
        if (_moraleGauge < 0) { _moraleGauge = 0; }
        if (_moraleGauge == 0) { GameOver(); }
    }

    public void AddDays()
    {
        _daysGauge += _daysBiomeBonus;
        UpdateDisplay(_daysDisplay, _daysGauge);
        if (_daysGauge < 0) { _daysGauge = 0; }
        if (_daysGauge == 0) { GameOver(); }
    }
    void GameOver()
    {
        Debug.Log("Game Over");
    }

}
