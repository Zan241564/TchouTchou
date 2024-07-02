using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    Train _train;
    [SerializeField]
    Biome _biome;
    Biome _biomeDupe;
    [SerializeField]
    float _trainVisualSpeed; // multiply the the scrolling speed of the background to create an illusion of speed for the train
    [SerializeField]
    float _testBiomeZScrollLimit;

    // Start is called before the first frame update
    void Start()
    {
        _biomeDupe = Instantiate(_biome, new Vector3(-5.0f, 4.0f, 50.0f), Quaternion.identity);
        _biomeDupe.transform.Rotate(90.0f, 0.0f, -90.0f);
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

    }
}
