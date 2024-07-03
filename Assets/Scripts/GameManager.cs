using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.UIElements;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    Train _train;
    [SerializeField]
    Biome _biome;
    int _currentBiomeID;
    Biome _biomeDupe;
    [SerializeField]
    float _trainVisualSpeedMemo;
    float _trainVisualSpeed; // multiply the the scrolling speed of the background to create an illusion of speed for the train
    [SerializeField]
    float _trainAccelerationFactor; // also used for deceleration
    bool _trainStop = false;
    [SerializeField]
    float _testBiomeZScrollLimit;
    [SerializeField]
    UnityEngine.UIElements.Button _startStop;
    [SerializeField]
    UnityEngine.UI.Image _fadeToBlack;
    [SerializeField]
    float _fadeToBlackSpeed;
    bool _swappingBiome = false;
    float _biomeSwappingTimer;
    [SerializeField]
    Material _nextBiome;
    [SerializeField]
    Material _nextBiomeGround;
    int _nextBiomeID = 1;
    [SerializeField]
    GameObject _clouds;
    GameObject _cloudsDupe;
    [SerializeField]
    float _cloudsSpeedFactor;

    // Start is called before the first frame update
    void Start()
    {
        _biomeDupe = Instantiate(_biome, new Vector3(-5.0f, 4.0f, 50.0f), Quaternion.identity);
        _biomeDupe.transform.Rotate(90.0f, 0.0f, -90.0f);
        _trainVisualSpeed = _trainVisualSpeedMemo;
        _currentBiomeID = 0;
        Vector3 cloudsDupePos = _clouds.gameObject.transform.position + new Vector3(0.0f, 0.0f, 50.0f);
        _cloudsDupe = Instantiate(_clouds, cloudsDupePos, Quaternion.identity);
        _cloudsDupe.gameObject.transform.Rotate(0.0f, 0.0f, -90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        _biome.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed;
        _biomeDupe.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed;
        _clouds.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed * _cloudsSpeedFactor;
        _cloudsDupe.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed * _cloudsSpeedFactor;
        if (_biome.transform.position.z < _testBiomeZScrollLimit)
        {
            _biome.transform.position = _biomeDupe.transform.position + new Vector3(0.0f, 0.0f, 50.0f);
        }
        if (_biomeDupe.transform.position.z < _testBiomeZScrollLimit)
        {
            _biomeDupe.transform.position = _biome.transform.position + new Vector3(0.0f, 0.0f, 50.0f);
        }
        if(_clouds.transform.position.z < _testBiomeZScrollLimit)
        {
            Debug.Log("_clouds respawn");
            _clouds.transform.position = _cloudsDupe.transform.position + new Vector3(0.0f, 0.0f, 50.0f);
        }
        if (_cloudsDupe.transform.position.z < _testBiomeZScrollLimit)
        {
            Debug.Log("_cloudsDupe respawn");
            _cloudsDupe.transform.position = _clouds.transform.position + new Vector3(0.0f, 0.0f, 50.0f);
        }

        if (_trainStop)
        {
            _trainVisualSpeed -= 1.0f * Time.deltaTime * _trainAccelerationFactor;
            if(_trainVisualSpeed < 0.0f)
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
        if(_swappingBiome)
        {
            _biomeSwappingTimer -= 1.0f * Time.deltaTime;
            if(_biomeSwappingTimer > 2.5f)
            {
                Color temp = _fadeToBlack.color;
                temp.a += 1.0f * Time.deltaTime * _fadeToBlackSpeed;
                _fadeToBlack.color = temp;
            }
            else if(_biomeSwappingTimer > 0.0f)
            {
                if (_currentBiomeID != _nextBiomeID)
                {
                    _biome.GetComponent<MeshRenderer>().material = _nextBiome;
                    _biomeDupe.GetComponent<MeshRenderer>().material = _nextBiome;
                    _biome.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = _nextBiomeGround;
                    _biomeDupe.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = _nextBiomeGround;
                    _currentBiomeID = _nextBiomeID;
                }
                Color temp = _fadeToBlack.color;
                temp.a -= 1.0f * Time.deltaTime * _fadeToBlackSpeed;
                _fadeToBlack.color = temp;
            }
            else
            {
                _swappingBiome = false;
            }
        }
    }

    public void StartStop()
    {
        _trainStop = !_trainStop;
    }

    public void ChangeBiome()
    {
        _swappingBiome = true;
        _biomeSwappingTimer = 5.0f;
        // set value of _nextBiome and _nextBiomeID. It's hard set for now.
    }

    public int GetBiomeID()
    {
        return _currentBiomeID;
    }

    // This is called automatically after a while when a new biome is loaded to stop the train and start the "management part" of the gameplay loop
    public void StopTrain()
    {
        _trainStop = true;
        // do things
    }

    // This is called when the player chooses the next biome to go. It calls all the "narrative part" of the gameplay loop
    public void StartTrain()
    {
        // do things
    }
}
