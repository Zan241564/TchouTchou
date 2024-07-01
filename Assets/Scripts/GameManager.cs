using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    Train _train;
    [SerializeField]
    Biome _biome;
    [SerializeField]
    float _trainVisualSpeed; // multiply the the scrolling speed of the background to create an illusion of speed for the train

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _biome.transform.position += new Vector3(0.0f, 0.0f, -1.0f) * Time.deltaTime * _trainVisualSpeed;
    }
}
