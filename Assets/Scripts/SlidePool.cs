﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePool : MonoBehaviour {
	public static SlidePool instance;

	// private GameObject[] slides;
	private GameObject[][] levelSections;
	private GameObject[][] enemiesSections;

	private GameObject checkpoint;
	private int slidePoolSize = 6;
	private int enemyPoolSize = 6;

	private int levelSectionBufferSize = 3;
	public GameObject slidePrefab;
	public GameObject enemyPrefab;

	public GameObject checkpointPrefab;
	public float spawnRate = 2f;
	public float ySlideMin = -5f;
	public float ySlideMax = 5f;
	public float xSlideMin = 0f;
	public float xSlideMax = 10f;

	private Vector2 objectPoolPosition = new Vector2(-15f, -25f);
	private int currentLevelSection = 0;
	private Quaternion spawnRotation = Quaternion.Euler(0,0,270);

	private float timeSinceLastSpawned;

	//Use this for initialization
	void Awake () {
		// Singleton pattern
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		timeSinceLastSpawned = spawnRate;
		levelSections = new GameObject[levelSectionBufferSize][];
		enemiesSections = new GameObject[levelSectionBufferSize][];
		checkpoint = (GameObject) Instantiate(checkpointPrefab, objectPoolPosition, Quaternion.identity);
		// Instantiate the level sections.
		for (int i = 0; i < levelSectionBufferSize; i++) {
			var slides = new GameObject[slidePoolSize];
			var enemies = new GameObject[slidePoolSize];
			for (int j = 0; j < slidePoolSize; j++) {
				slides[j] = (GameObject) Instantiate(slidePrefab, objectPoolPosition, spawnRotation);
				enemies[j] = (GameObject) Instantiate(enemyPrefab, objectPoolPosition, Quaternion.identity);
			}
			levelSections[i] = slides;
			enemiesSections[i] = enemies;
		}
	}

	void FormatLevelSection(GameObject[] levelSection, GameObject[] enemies) {
		var positions = new Vector2[]{
			new Vector2(3f, -1f),
            new Vector2(10f, -7f),
            new Vector2(9f, 3f),
            new Vector2(13f, 1f),
            new Vector2(16f, -5f),
            new Vector2(21f, -1f),
		};
		// move it offscreen
		var xOffset = 24f + Random.Range(-4f, 4f);
		var yOffset = Random.Range(-4f, 4f);
		var enemyYOffset = yOffset + 1.0f;

		for (int i = 0; i < slidePoolSize; i++) {
			var position = positions[i];
			var enemyXOffset = xOffset + Random.Range(-5f,5f);

			var shouldSpawnSlide = Random.Range(-1,2);
			if (shouldSpawnSlide >= 0) {
				levelSection[i].transform.position = new Vector2(position.x + xOffset, position.y + yOffset);
				var shouldSpawn = Random.Range(-1,1);
				if (shouldSpawn >= 0){
					enemies[i].transform.position = new Vector2(position.x + enemyXOffset, position.y + enemyYOffset);
					enemies[i].transform.rotation = Quaternion.Euler(0, 0, 15.4f);
					// enable sprite for enemy
					var esr = enemies[i].GetComponent<SpriteRenderer>();
					esr.enabled = true;
					// disgusting hacks
                    var eanim = enemies[i].GetComponent<Animator>();
					eanim.SetBool("Alive", true);
				}
			}
		}

		// place the checkpoint
		checkpoint.transform.position = new Vector2(24f, 0);
	}

	// Update is called once per frame
	void Update () {
		timeSinceLastSpawned += Time.deltaTime;
		// if (GameController.instance.gameOver == false && timeSinceLastSpawned >= spawnRate) {
		// 	timeSinceLastSpawned = 0;


		// }
	}

	public void PlaceNextLevelSection() {
		// Debug.Log(levelSections[currentLevelSection].Length);
        FormatLevelSection(levelSections[currentLevelSection], enemiesSections[currentLevelSection]);

        // float spawnYPos = Random.Range(ySlideMin, ySlideMax);
        // float spawnXPos = Random.Range(xSlideMin, xSlideMax);
        // slides[currentSlide].transform.position = new Vector2(spawnXPos, spawnYPos);
        currentLevelSection = (currentLevelSection + 1) % levelSectionBufferSize;
    }
}
