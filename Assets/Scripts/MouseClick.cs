using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    bool intro = true;

    [SerializeField]
    GameObject _startingScreen;

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Debug.Log("Quitting Application...");
            Application.Quit();
        }
    }

    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        if (intro == true)
        {
            _startingScreen.SetActive(false);
            intro = false;
        }

    }
}
