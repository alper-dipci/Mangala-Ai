using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.MLAgents;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Agent firstAgent;
    [SerializeField] private Agent secondAgent;
    public int[] FirstPlayerDeck= new int[7];
    public int[] SecondPlayerDeck = new int[7];
    [SerializeField] TextMeshProUGUI p1,p2,p3,p4,p5,p6,pbank,e1,e2,e3,e4,e5,e6,ebank;
    public Turn turn;

    private string firstPlayerMoves;
    private string secondPlayerMoves;
    void Start()
    {
        ResetGame();
        firstAgent.RequestDecision();
    }

    void Update()
    {
        //showlist();
        showDeckNumbers();
    }
    void showlist()
    {
        string result = "List contents: ";
        foreach (int i in FirstPlayerDeck)
            result += i.ToString() + ", ";
        Debug.Log(result);
        
    }

    private void showDeckNumbers()
    {
        p1.text = FirstPlayerDeck[0].ToString();
        p2.text = FirstPlayerDeck[1].ToString();
        p3.text = FirstPlayerDeck[2].ToString();
        p4.text = FirstPlayerDeck[3].ToString();
        p5.text = FirstPlayerDeck[4].ToString();
        p6.text = FirstPlayerDeck[5].ToString();
        pbank.text = FirstPlayerDeck[6].ToString();

        e1.text = SecondPlayerDeck[0].ToString();
        e2.text = SecondPlayerDeck[1].ToString();
        e3.text = SecondPlayerDeck[2].ToString();
        e4.text = SecondPlayerDeck[3].ToString();
        e5.text = SecondPlayerDeck[4].ToString();
        e6.text = SecondPlayerDeck[5].ToString();
        ebank.text = SecondPlayerDeck[6].ToString();
    }

    public void makeMoveAtIndex(int index,bool isFirstPlayer)
    {
        if (isFirstPlayer)
            firstPlayerMoves += index + ", ";
        else
            secondPlayerMoves += index + ", ";
        //DECLARE DECKS
        int lastBox = 0;
        int[] selectedArray = isFirstPlayer == true ? FirstPlayerDeck : SecondPlayerDeck;
        int[] enemyArray = isFirstPlayer == true ? SecondPlayerDeck  : FirstPlayerDeck;
        Agent selectedAgent = isFirstPlayer == true ? firstAgent : secondAgent;
        Agent enemyAgent = isFirstPlayer == true ? secondAgent  : firstAgent;
        int amountInBox = selectedArray[index];
        int bankStartPoint = selectedArray[6];
        // IF THE BOX IS EMPTY GIVE ERROR
        if (amountInBox == 0)
        {
            selectedAgent.AddReward(-1000f);
            selectedAgent.RequestDecision();
            return ;
        }
        // IF ITS 1 SPECIAL CASE 
        else if (amountInBox == 1){
            selectedArray[index] = 0;
            selectedArray[index+1] += 1;
            lastBox = index + 1;
        }
        // DO A LOOP UNTIL AMOUNT IS OVER
        else if (amountInBox > 1)
        {
            selectedArray[index] = 0;
            while (amountInBox > 0) { 
                for (int i = index; i < selectedArray.Length; i++)
                {
                    if (amountInBox > 0){
                        
                        lastBox = i;
                        selectedArray[i] += 1;
                        amountInBox--;
                    }                        
                }
                index=0;
                for (int i = index; i < enemyArray.Length-1; i++)
                {
                    if (amountInBox > 0)
                    {
                        lastBox = -i;
                        enemyArray[i] += 1;
                        amountInBox--;
                    }
                }
                index = 0;
                
            }
        }
        //if the lastBox is empty (we put 1) then take enemy box with you
        if (lastBox >= 0 && selectedArray[lastBox]==1&& lastBox!=6 && enemyArray[5 - lastBox] != 0) {
            selectedArray[6] += selectedArray[lastBox];
            selectedArray[lastBox] = 0;
            selectedArray[6] += enemyArray[5 - lastBox];
            enemyArray[5 - lastBox] = 0;
        }
        // if last box is even and in the enemy boxes take them
        if (lastBox < 0)
        {
            lastBox=Mathf.Abs(lastBox);
            if (enemyArray[lastBox] % 2 == 0)
            {
                selectedArray[6]+=enemyArray[lastBox];
                enemyArray[lastBox] = 0;
            }
        }
        
        //DECLARE DECKS AGAIN
        if (isFirstPlayer) {
            FirstPlayerDeck = selectedArray;
            SecondPlayerDeck = enemyArray;
        }
        else
        {
            FirstPlayerDeck = enemyArray ;
            SecondPlayerDeck = selectedArray;
        }
        
        //ADD REWARD
            selectedAgent.AddReward(selectedArray[6] - bankStartPoint);


        //DECIDE WHOSE TURN IT IS 
        turn = isFirstPlayer == true ? Turn.Player2Turn : Turn.Player1Turn;
        if(lastBox==6)
            turn = isFirstPlayer == true ? Turn.Player1Turn  : Turn.Player2Turn;

        //CHECKH GAME OVER
        if (checkIsGameOver() != null)
        {
            Agent winningAgent = checkIsGameOver();
            winningAgent.AddReward(+100f);
            Agent losingAgent = winningAgent == firstAgent ? secondAgent : firstAgent;
            losingAgent.AddReward(-100f);
            firstAgent.EndEpisode();
            secondAgent.EndEpisode();
            //Debug.Log("first agent Score: "+ FirstPlayerDeck[6] +" first agent moves: " + firstPlayerMoves);
            //Debug.Log("second agent Score: " + SecondPlayerDeck[6]+"second agent moves: " + secondPlayerMoves);
            //firstPlayerMoves = "";
            //secondPlayerMoves = "";
            ResetGame();
        }

        //SEND  REQUEST
        if (turn == Turn.Player1Turn) {
            //Debug.Log("sending player 1 req decision");
            firstAgent.RequestDecision();
        }            
        if (turn == Turn.Player2Turn)
        {
            //Debug.Log("sending player 1 req decision");
            secondAgent.RequestDecision();
        }
            
        
        

    }
    private Agent  checkIsGameOver()
    {
        bool isFirstDeckEmpty = true;
        bool isSecondDeckEmpty = true;
        for ( int i = 0; i < 6; i++)
        {
            if (FirstPlayerDeck[i] != 0)
                isFirstDeckEmpty = false;
            if (SecondPlayerDeck[i] != 0)
                isSecondDeckEmpty = false;
        }
        if(isFirstDeckEmpty)
        {
            for(int i = 0; i < 6; i++)
            {
                FirstPlayerDeck[6] += SecondPlayerDeck[i];
                SecondPlayerDeck[i]=0;
            }
            return firstAgent ;
        }              
        else if (isSecondDeckEmpty)
        {
            for (int i = 0; i < 6; i++)
            {
                SecondPlayerDeck[6] += FirstPlayerDeck[i];
                FirstPlayerDeck[i] = 0;
            }
            return secondAgent;
        }
        else
            return null;
    }
    public void ResetGame()
    {
        turn = Turn.Player1Turn;

        FirstPlayerDeck[6] = 0;
        SecondPlayerDeck[6] = 0;

        for (int i = 0; i < 6; i++)
        {
            FirstPlayerDeck[i] = 4;
            SecondPlayerDeck[i] = 4;
        }
    }
}
public enum Turn {
    Player1Turn,
    Player2Turn
}
