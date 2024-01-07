using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class mangalaAgent : Agent
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool isFirstPlayer;

    public override void OnActionReceived(ActionBuffers actions)
    {

        //StartCoroutine(gameManager.makeMoveAtIndex(actions.DiscreteActions[0], isFirstPlayer));
        gameManager.makeMoveAtIndex(actions.DiscreteActions[0], isFirstPlayer);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> action = actionsOut.DiscreteActions;
        if (Input.GetKeyDown(KeyCode.A))
            action[0] = 0;
        if (Input.GetKeyDown(KeyCode.S))
            action[0] = 1;
        if (Input.GetKeyDown(KeyCode.D))
            action[0] = 2;
        if (Input.GetKeyDown(KeyCode.F))
            action[0] = 3;
        if (Input.GetKeyDown(KeyCode.G))
            action[0] = 4;
        if (Input.GetKeyDown(KeyCode.H))
            action[0] = 5;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        for(int i =0; i < 7; i++)
        {
            sensor.AddObservation(gameManager.FirstPlayerDeck[i]);
            sensor.AddObservation(gameManager.SecondPlayerDeck[i]);
        }
    }
    
}
