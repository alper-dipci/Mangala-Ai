using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayController : MonoBehaviour {
    [SerializeField] GameManager gameManager;
    

    // Update is called once per frame
    void Update()
    {
        if(gameManager.turn == Turn.Player1Turn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                gameManager.makeMoveAtIndex(0, true);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                gameManager.makeMoveAtIndex(1, true);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                gameManager.makeMoveAtIndex(2, true);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                gameManager.makeMoveAtIndex(3, true);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                gameManager.makeMoveAtIndex(4, true);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                gameManager.makeMoveAtIndex(5, true);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
                gameManager.makeMoveAtIndex(0, false);
            if (Input.GetKeyDown(KeyCode.S))
                gameManager.makeMoveAtIndex(1, false);
            if (Input.GetKeyDown(KeyCode.D))
                gameManager.makeMoveAtIndex(2, false);
            if (Input.GetKeyDown(KeyCode.F))
                gameManager.makeMoveAtIndex(3, false);
            if (Input.GetKeyDown(KeyCode.G))
                gameManager.makeMoveAtIndex(4, false);
            if (Input.GetKeyDown(KeyCode.H))
                gameManager.makeMoveAtIndex(5, false);
        }

        
    }
}
