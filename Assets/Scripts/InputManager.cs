using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.StickyNote;
using static UnityEngine.ParticleSystem;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    bool intro = true;

    [SerializeField]
    GameObject _startingScreen;

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Quitting Application...");
            Application.Quit();
        }

        if ((Input.GetKeyDown("right alt") && Input.GetKey("r")) || (Input.GetKey("left alt") && Input.GetKeyDown("r")))
        {
            Debug.Log("Restarting Application...");
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        if (intro == true)
        {
            //FadeOut(_startingScreen);
            _startingScreen.SetActive(false);
            intro = false;
        }

    }

    /*void FadeOut(GameObject fadingObject, float duration = 10.0f)
    {
        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            var temp = transform.Find("SampleScene/PermanentContentCanvas/EndingScreens/Starting Panel").GetComponent<Image>();
            temp.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), t / duration);
        }
        fadingObject.SetActive(false);
    }*/
}
