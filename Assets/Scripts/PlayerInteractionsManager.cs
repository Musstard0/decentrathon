using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerInteractionsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Vova")
        {
            Application.Quit();
            Debug.Log("a");
        }
        if(other.gameObject.tag == "Exit")
        {
            SceneManager.LoadScene(1);
        }
    }
}
