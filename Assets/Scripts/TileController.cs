using System.Collections;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public IEnumerator SummonTile(float timeToSpawn, AnimationCurve spawnCurve)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Vector3.up * 1);
        float elapsed = 0f;

        while (elapsed < timeToSpawn)
        {
            float t = spawnCurve.Evaluate(elapsed / timeToSpawn);
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        yield return null;
    }
}