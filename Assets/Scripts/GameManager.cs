using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.UIElements;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject _train;
    [SerializeField] GameObject _biome;
    int _currentBiomeID;
    GameObject _biomeDupe;
    [SerializeField] GameObject _ground;
    GameObject _groundDupe;
    [SerializeField] float _groundSpeedFactor;
    [SerializeField] float _trainVisualSpeedMemo;
    float _trainVisualSpeed; // multiply the the scrolling speed of the background to create an illusion of speed for the train
    [SerializeField] float _trainAccelerationFactor; // also used for deceleration
    public bool _trainStop = false;
    [SerializeField] float _testBiomeZScrollLimit;
    [SerializeField] UnityEngine.UIElements.Button _startStop;
    [SerializeField] UnityEngine.UI.Image _fadeToBlack;
    [SerializeField] float _fadeToBlackSpeed;
    bool _swappingBiome = false;
    float _biomeSwappingTimer;
    [SerializeField] float _swappingBiomeExitingDuration;
    [SerializeField] float _swappingBiomeTransitionDuration;
    [SerializeField] float _swappingBiomeEnteringDuration;
    int _nextBiomeID = 1;
    [SerializeField] GameObject _sky;
    [SerializeField] GameObject _clouds;
    GameObject _cloudsDupe;
    [SerializeField] float _cloudsSpeedFactor;
    [SerializeField] BiomeData _forestData;
    [SerializeField] BiomeData _desertData;
    [SerializeField] BiomeData _mountainData;
    BiomeData _currentBiomeData;

    // Start is called before the first frame update
    void Start()
    {
        _currentBiomeData = _forestData;
        _biomeDupe = Instantiate(_biome, new Vector3(-5.0f, 5.0f, 80.0f), Quaternion.identity);
        _biomeDupe.transform.Rotate(90.0f, 0.0f, -90.0f);
        _trainVisualSpeed = _trainVisualSpeedMemo;
        _currentBiomeID = 0;
        Vector3 groundDupePos = _ground.gameObject.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        _groundDupe = Instantiate(_ground, groundDupePos, Quaternion.identity);
        _groundDupe.gameObject.transform.Rotate(90.0f, 0.0f, -90.0f);
        Vector3 cloudsDupePos = _clouds.gameObject.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        _cloudsDupe = Instantiate(_clouds, cloudsDupePos, Quaternion.identity);
        _cloudsDupe.gameObject.transform.Rotate(90.0f, 0.0f, -90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTrainSpeed();
        SceneryScrolling();
        if (_swappingBiome)
        {
            BiomeSwap();
        }
    }

    private void SceneryScrolling()
    {
        _biome.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed;
        _biomeDupe.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed;
        _ground.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed * _groundSpeedFactor;
        _groundDupe.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed * _groundSpeedFactor;
        _clouds.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed * _cloudsSpeedFactor;
        _cloudsDupe.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed * _cloudsSpeedFactor;
        if (_biome.transform.position.z < _testBiomeZScrollLimit)
        {
            _biome.transform.position = _biomeDupe.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        }
        if (_biomeDupe.transform.position.z < _testBiomeZScrollLimit)
        {
            _biomeDupe.transform.position = _biome.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        }
        if (_ground.transform.position.z < _testBiomeZScrollLimit)
        {
            _ground.transform.position = _groundDupe.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        }
        if (_groundDupe.transform.position.z < _testBiomeZScrollLimit)
        {
            _groundDupe.transform.position = _ground.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        }
        if (_clouds.transform.position.z < _testBiomeZScrollLimit)
        {
            _clouds.transform.position = _cloudsDupe.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        }
        if (_cloudsDupe.transform.position.z < _testBiomeZScrollLimit)
        {
            _cloudsDupe.transform.position = _clouds.transform.position + new Vector3(0.0f, 0.0f, 80.0f);
        }
    }

    private void UpdateTrainSpeed()
    {
        if (_trainStop)
        {
            _trainVisualSpeed -= 1.0f * Time.deltaTime * _trainAccelerationFactor;
            if (_trainVisualSpeed < 0.0f)
            {
                _trainVisualSpeed = 0.0f;
            }
        }
        else
        {
            _trainVisualSpeed += 1.0f * Time.deltaTime * _trainAccelerationFactor;
            if (_trainVisualSpeed > _trainVisualSpeedMemo)
            {
                _trainVisualSpeed = _trainVisualSpeedMemo;
            }
        }
    }

    private void BiomeSwap()
    {
        _biomeSwappingTimer -= 1.0f * Time.deltaTime;
        if(_biomeSwappingTimer > _swappingBiomeTransitionDuration + _swappingBiomeEnteringDuration)
        {
            // do exiting messages things
        }
        else if (_biomeSwappingTimer > _swappingBiomeTransitionDuration*0.5f + _swappingBiomeEnteringDuration)
        {
            // stop exiting messages and do the fade to black
            _fadeToBlack.gameObject.SetActive(true);
            Color temp = _fadeToBlack.color;
            temp.a += 1.0f * Time.deltaTime * _fadeToBlackSpeed;
            _fadeToBlack.color = temp;
        }
        else if (_biomeSwappingTimer > _swappingBiomeEnteringDuration)
        {
            // do the switcheroo then undo the fade to black
            if (_currentBiomeID != _nextBiomeID)
            {
                Debug.Log(_currentBiomeData);
                _sky.gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.sky;
                _clouds.gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.clouds;
                _clouds.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.clouds1;
                _cloudsDupe.gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.clouds;
                _cloudsDupe.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.clouds1;
                _biome.GetComponent<MeshRenderer>().material.mainTexture =  _currentBiomeData.background;
                _biome.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.background1;
                _biomeDupe.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.background;
                _biomeDupe.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.background1;
                _ground.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.ground;
                _groundDupe.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.ground;
                _train.gameObject.GetComponent<MeshRenderer>().material.mainTexture = _currentBiomeData.train;
                _currentBiomeID = _nextBiomeID;
            }
            Color temp = _fadeToBlack.color;
            temp.a -= 1.0f * Time.deltaTime * _fadeToBlackSpeed;
            _fadeToBlack.color = temp;
        }
        else if (_biomeSwappingTimer > 0.0f)
        {
            _fadeToBlack.gameObject.SetActive(false);
            // do the entering messages stuff
        }
        else
        {
            _swappingBiome = false;
            StopTrain();
        }
    }

    public void StartStop()
    {
        _trainStop = !_trainStop;
    }

    public void ChangeBiome()
    {
        //_swappingBiome = true;
        //_biomeSwappingTimer = 5.0f;
        StartTrain(1);
    }

    public int GetBiomeID()
    {
        return _currentBiomeID;
    }

    private void SetNextBiomeID(int nextBiomeID)
    {
        _nextBiomeID = nextBiomeID;
    }

    // This is called automatically after a while when a new biome is loaded to stop the train and start the "management part" of the gameplay loop
    public void StopTrain()
    {
        _trainStop = true;
        // do things
    }

    // This is called when the player chooses the next biome to go. It calls all the "narrative part" of the gameplay loop and the biome swapping logic.
    public void StartTrain(int nextBiome)
    {
        SetNextBiomeID(nextBiome);
        _trainStop = false;
        _swappingBiome = true;
        _biomeSwappingTimer = _swappingBiomeExitingDuration + _swappingBiomeTransitionDuration + _swappingBiomeEnteringDuration;
        switch (_nextBiomeID)
        {
            case 0:
                _currentBiomeData = _forestData;
                break;
            case 1:
                _currentBiomeData = _desertData;
                break;
            case 2:
                _currentBiomeData = _mountainData;
                break;
            default:
                _currentBiomeData = _forestData;
                break;
        }
    }

    public bool TrainActuallyStopped()
    {
        if(_trainVisualSpeed <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
