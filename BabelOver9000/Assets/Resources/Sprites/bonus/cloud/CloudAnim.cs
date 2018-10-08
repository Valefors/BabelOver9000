using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnim : MonoBehaviour {

    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private GameObject godHand;


    public void LaunchTransition()
    {
        levelManager.LaunchTransition();
    }

    public void AnimateGodHand()
    {
        StartCoroutine(LaunchHandAnimation());
    }

    private IEnumerator LaunchHandAnimation()
    {
        godHand.transform.position = new Vector3(godHand.transform.position.x, levelManager.floorPosY + 10f, 0.0f);

        float t = 0;

        while (t <= 0.5f)
        {
            t += Time.deltaTime;
            godHand.transform.position = new Vector3(godHand.transform.position.x, godHand.transform.position.y - t * 0.5f, 0.0f);
            yield return new WaitForFixedUpdate();
        }

        t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime;
            godHand.transform.position = new Vector3(godHand.transform.position.x, godHand.transform.position.y + t * 0.5f, 0.0f);
            yield return new WaitForFixedUpdate();
        }

        godHand.transform.position = new Vector3(godHand.transform.position.x, 120f, 0.0f);

        yield return null;
    }
}
