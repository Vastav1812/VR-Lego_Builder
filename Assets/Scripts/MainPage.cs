using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPage : MonoBehaviour
{
    public GameObject legoSprite1;
    public GameObject legoSprite2;
    public GameObject legoSprite3;
    public GameObject legoSprite4;
    public static bool starting = true;
    public GameObject startPanel;
    // Start is called before the first frame update
    void Start()
    {
        legoSprite1.GetComponent<Renderer>().enabled = false;
        legoSprite2.GetComponent<Renderer>().enabled = false;
        legoSprite3.GetComponent<Renderer>().enabled = false;
        legoSprite4.GetComponent<Renderer>().enabled = false;
        StartCoroutine(SpawnIcons());
    }

    // Update is called once per frame
    void Update()
    {
        if(starting)
        {
            if(Input.GetButtonUp("B"))
            {
                starting = false;
                startPanel.SetActive(false);
                SceneManager.LoadScene("LegoScene");
            }
        }
    }
    private IEnumerator SpawnIcons()
    {
        while(starting)
        {
            yield return new WaitForSeconds(0.75f);
            legoSprite1.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.75f);
            legoSprite2.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.75f);
            legoSprite3.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.75f);
            legoSprite4.GetComponent<Renderer>().enabled = true;

            yield return new WaitForSeconds(0.75f);
            legoSprite1.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.75f);
            legoSprite2.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.75f);
            legoSprite3.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.75f);
            legoSprite4.GetComponent<Renderer>().enabled = false;
        }
    }
}
