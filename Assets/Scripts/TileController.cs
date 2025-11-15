using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class TileController : MonoBehaviour
{
    public Transform terrainFloor;
    [SerializeField] bool playerCanSpawn = false;
    [SerializeField] bool isEmpty = false;
    [SerializeField] int[] coords = new int[2];
    [SerializeField] CharacterBase inTileCharacter;

    Coroutine longPressCoroutine;
    float longPressDuration = 1f;


    public bool PlayerCanSpawn { get => playerCanSpawn; set => playerCanSpawn = value; }
    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }

    /*private void OnMouseDown()
    {
        if (LevelManager.instance.actualPhase == levelPhase.Placement)
        {
            if (!isEmpty && playerCanSpawn)
            {
                longPressCoroutine = StartCoroutine(LongPressDetect());
            }
            else if (isEmpty && playerCanSpawn)
            {
                // intenta col·locar si hi ha una carta seleccionada
                //PlacementController.Instance.TryPlaceOnTile(this);
            }
        }
    }*/
    private void OnMouseUp()
    {
        if (longPressCoroutine != null)
        {
            StopCoroutine(longPressCoroutine);
            longPressCoroutine = null;
        }
    }
    IEnumerator LongPressDetect()
    {
        float elapsed = 0f;
        while (elapsed < longPressDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator SummonTile(float timeToSpawn,float waitUntil, AnimationCurve spawnCurve)
    {
        yield return new WaitForSeconds(waitUntil);
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
    public void SetCoord(int x, int y)
    {
        coords[0] = x; coords[1] = y;
    }
    public void SetCharacter(CharacterBase characterToPlace)
    {
        inTileCharacter = characterToPlace;
        isEmpty = false;
    }
    public void DestroyCharacterOnTile()
    {
        Destroy(inTileCharacter);
        isEmpty = true;
    }
}
