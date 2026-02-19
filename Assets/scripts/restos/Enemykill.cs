using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Enemykill : MonoBehaviour
{
    public string Samplescene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(Samplescene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
