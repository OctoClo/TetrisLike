using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int boardWidth = 10;
    private int boardHeigt = 19;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public bool IsInsideBoard (Vector2 position)
    {
        Debug.Log("X : " + (int)position.x + " - Y : " + (int)position.y);
        return ((int)position.x >= 0 && (int)position.x < boardWidth && (int)position.y < boardHeigt);
    }

    public Vector2 Round (Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
