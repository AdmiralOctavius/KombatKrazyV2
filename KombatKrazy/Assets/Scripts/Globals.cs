﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Globals : MonoBehaviour {

    public float timeLeft = 30f;
    public AudioSource audioPl;
    public GameObject player;
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            player.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            player.gameObject.GetComponent<HealthBar>().dead = true;
            Debug.Log("Game Over");
            if(audioPl.isPlaying == false)
            {
               audioPl.Play();

            }
        }

        if (player.gameObject.GetComponent<HealthBar>().dead)
        {
            LoadMenu();
        }
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("loss");
    }
}
