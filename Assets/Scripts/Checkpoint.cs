using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameManager gm;
    SceneData data;

    [SerializeField]
    float debugDrawRadius = 3.0F;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //playerRef = GameObject.Find("PlayerBody");
        //gm.lastCheckpointPosition = playerRef.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.SaveData(transform.position);
            StartCoroutine(CheckpointSavedTextDisplayRoutine());
        }
    }

    IEnumerator CheckpointSavedTextDisplayRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(2);

        gm.checkpointReachedText.SetActive(true);

        while (true)
        {
            yield return wait;
            gm.checkpointReachedText.SetActive(false);
        }
    }
}
