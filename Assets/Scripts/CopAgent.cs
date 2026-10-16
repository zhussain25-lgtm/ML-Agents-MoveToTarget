using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class CopAgent : Agent
{
    [SerializeField] private Transform robber;
    [SerializeField] private Transform escapeTarget;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private ArenaManager arenaManager;

    private Vector3 startingPosition;

    public override void Initialize()
    {
        startingPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(robber.localPosition);
        sensor.AddObservation(escapeTarget.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 move = new Vector3(moveX, 0f, moveZ);
        move = Vector3.ClampMagnitude(move, 1f);

        Vector3 nextPosition = transform.localPosition + move * moveSpeed * Time.fixedDeltaTime;

        nextPosition.x = Mathf.Clamp(nextPosition.x, -5.25f, 5.25f);
        nextPosition.z = Mathf.Clamp(nextPosition.z, -5.25f, 5.25f);

        transform.localPosition = nextPosition;

        if (MaxStep > 0)
        {
            AddReward(-1f / MaxStep);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuous = actionsOut.ContinuousActions;
        var kb = Keyboard.current;

        continuous[0] = 0f;
        continuous[1] = 0f;

        if (kb == null) return;

        continuous[0] = (kb.dKey.isPressed ? 1f : 0f)
                      - (kb.aKey.isPressed ? 1f : 0f);

        continuous[1] = (kb.wKey.isPressed ? 1f : 0f)
                      - (kb.sKey.isPressed ? 1f : 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robber"))
        {
            arenaManager.CopCaughtRobber();
        }
    }
}

