using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineMaterial : MonoBehaviour
{
    private Material[] mats;
    private Material OutlineMat;

    // Start is called before the first frame update
    void Start()
    {
        mats = this.GetComponent<Renderer>().materials;

        if(mats.Length > 1)
        {
            OutlineMat = mats[1];
            Debug.Log(OutlineMat.name);
            OutlineMat.SetFloat("OutlineSize", 2f);
            OutlineMat.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
