using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGameManager : MonoBehaviour
{
    public bool _trainStop = false;
    public int biomeID = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public int GetBiomeID()
    {
        return biomeID;
    }

    public IEnumerator SetNextBiomeID(int newbiomeID)
    {
        Debug.Log("le trian part");
        biomeID = newbiomeID;
        _trainStop = false;
        yield return new WaitForSeconds(1.0f);
        _trainStop = true;
        Debug.Log("le trian est arrivé");
    }

}
