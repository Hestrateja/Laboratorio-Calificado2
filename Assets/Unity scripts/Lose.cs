using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Lose : MonoBehaviour
{
    public bool Lost = false;
    public GameObject gameOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            Lost = true;
            gameOver.SetActive(Lost);
        }
    }
}
