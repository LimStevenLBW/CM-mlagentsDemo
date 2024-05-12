using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class MoveToGoalAgent : Agent
{
    public Material winMat;
    public Material loseMat;
    public MeshRenderer floorMesh;
    public Transform target;

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.ContinuousActions[0]);

        float xDirection = actions.ContinuousActions[0];
        float yDirection = actions.ContinuousActions[1];

        float speed = 4;

        transform.localPosition += new Vector3(xDirection, 0, yDirection) * Time.deltaTime * speed;
    }

    public override void OnEpisodeBegin()
    {
        Vector3 randomPos = new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2));
        Vector3 randomPos2 = new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2));
        transform.localPosition = randomPos;
        target.localPosition = randomPos2;

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if we touched a gameobject with the target script
        if(collision.gameObject.TryGetComponent(out Target target))
        {
            floorMesh.material = winMat;


            SetReward(+1);
            EndEpisode();
        }
        else if(collision.gameObject.TryGetComponent(out Wall wall))
        {
            floorMesh.material = loseMat;
            SetReward(-1);
            EndEpisode();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
