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

    // Start is called before the first frame update
    void Start()
    {
         _foodGauge = 70;
         _waterGauge = 70;
         _moraleGauge = 70;
         _daysGauge = 70;

        UpdateDisplay(_foodDisplay, _foodGauge);
        UpdateDisplay(_waterDisplay, _waterGauge);
        UpdateDisplay(_moraleDisplay, _moraleGauge);
        UpdateDisplay(_daysDisplay, _daysGauge);

        _foodBiomeBonus = 1;
         _waterBiomeBonus = 1;
         _moraleBiomeBonus = 1;
         _daysBiomeBonus = 1;


    }

    // Update is called once per frame
    void UpdateDisplay(TMP_Text _display, int _valueGauge)
    {
        _display.text = _valueGauge.ToString();
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
