using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private CopAgent cop;
    [SerializeField] private RobberAgent robber;
    [SerializeField] private Transform escapeTarget;
    [SerializeField] private float roundDuration = 30f;
    [SerializeField] private float spawnRange = 7f;

    private float timer;
    private bool roundEnding;

    private void Start()
    {
        ResetArena();
    }

    private void FixedUpdate()
    {
        if (roundEnding) return;

        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            EndRound(0);
        }
    }

    public void CopCaughtRobber()
    {
        EndRound(1);
    }

    public void RobberEscaped()
    {
        EndRound(2);
    }

    private void EndRound(int result)
    {
        if (roundEnding) return;

        roundEnding = true;

        if (result == 1)
        {
            cop.AddReward(1f);
            robber.AddReward(-1f);
        }
        else if (result == 2)
        {
            cop.AddReward(-1f);
            robber.AddReward(1f);
        }

        cop.EndEpisode();
        robber.EndEpisode();

        ResetArena();
    }

    private void ResetArena()
    {
        timer = roundDuration;

        cop.transform.localPosition = new Vector3(-3f, 1.25f, 0f);
        robber.transform.localPosition = new Vector3(0f, 1.25f, 0f);
        escapeTarget.localPosition = new Vector3(3f, 0.75f, 0f);

        roundEnding = false;
    }
}

