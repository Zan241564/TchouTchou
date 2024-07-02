using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    Button _startStop;

    // Start is called before the first frame update
    void Start()
    {
        _biomeDupe = Instantiate(_biome, new Vector3(-5.0f, 4.0f, 50.0f), Quaternion.identity);
        _biomeDupe.transform.Rotate(90.0f, 0.0f, -90.0f);
        _trainVisualSpeed = _trainVisualSpeedMemo;
        _currentBiomeID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _biome.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed;
        _biomeDupe.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed;
        if(_biome.transform.position.z < _testBiomeZScrollLimit)
        {
            _biome.transform.position = _biomeDupe.transform.position + new Vector3(0.0f, 0.0f, 50.0f);
        }
        if (_biomeDupe.transform.position.z < _testBiomeZScrollLimit)
        {
            _biomeDupe.transform.position = _biome.transform.position + new Vector3(0.0f, 0.0f, 50.0f);
        }

        if(_trainStop)
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
    }

    public void StartStop()
    {
        _trainStop = !_trainStop;
    }

    public int GetBiomeID()
    {
        return _currentBiomeID;
    }
}
