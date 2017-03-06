using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {
    private float previousFallTime;
    private float speed = 1f;

	void Start ()
    {
		
	}

    void Update ()
    {
        processInput();
	}

    void processInput ()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            transform.position += new Vector3(-1, 0, 0);

        else if (Input.GetKeyDown(KeyCode.RightArrow))
            transform.position += new Vector3(1, 0, 0);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            transform.Rotate(0, 0, 90);

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousFallTime >= speed)
        {
            transform.position += new Vector3(0, -1, 0);
            previousFallTime = Time.time;
        }
    }
}
