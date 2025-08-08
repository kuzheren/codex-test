using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SimpleAgent : Agent
{
    [Header("Agent Settings")] 
    public float moveSpeed = 5f;
    public float reachThreshold = 1.2f;

    [Header("References")]
    public Transform targetTransform;
    public SimpleArea area;

    private Rigidbody agentRigidbody;
    private Vector3 startingPosition;
    private float previousDistanceToTarget;

    public override void Initialize()
    {
        agentRigidbody = GetComponent<Rigidbody>();
        startingPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        // hi
        // Reset velocity and position
        if (agentRigidbody != null)
        {
            agentRigidbody.velocity = Vector3.zero;
            agentRigidbody.angularVelocity = Vector3.zero;
        }

        if (area != null)
        {
            transform.localPosition = area.GetRandomPointOnFloor(y: startingPosition.y);
            if (targetTransform != null)
            {
                targetTransform.localPosition = area.GetRandomPointOnFloor(y: targetTransform.localPosition.y);
            }
        }
        else
        {
            transform.localPosition = startingPosition;
        }

        previousDistanceToTarget = GetDistanceToTarget();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 agentPos = transform.localPosition;
        Vector3 targetPos = targetTransform != null ? targetTransform.localPosition : Vector3.zero;

        // Observations: agent position (x,z), target position (x,z), agent velocity (x,z)
        sensor.AddObservation(agentPos.x);
        sensor.AddObservation(agentPos.z);
        sensor.AddObservation(targetPos.x);
        sensor.AddObservation(targetPos.z);

        if (agentRigidbody != null)
        {
            sensor.AddObservation(agentRigidbody.velocity.x);
            sensor.AddObservation(agentRigidbody.velocity.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 move = new Vector3(moveX, 0f, moveZ) * moveSpeed;

        if (agentRigidbody != null)
        {
            agentRigidbody.AddForce(move, ForceMode.VelocityChange);
        }
        else
        {
            transform.localPosition += move * Time.fixedDeltaTime;
        }

        float distanceToTarget = GetDistanceToTarget();

        // Shaping reward: reward improvement towards the target
        float distanceDelta = previousDistanceToTarget - distanceToTarget;
        AddReward(distanceDelta * 0.05f);
        previousDistanceToTarget = distanceToTarget;

        // Small step penalty to encourage faster solutions
        AddReward(-1f / MaxStep);

        // Success condition
        if (distanceToTarget < reachThreshold)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        // Fell off platform (optional, if elevated)
        if (transform.localPosition.y < -1f)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private float GetDistanceToTarget()
    {
        if (targetTransform == null)
        {
            return 0f;
        }
        return Vector3.Distance(transform.localPosition, targetTransform.localPosition);
    }
}