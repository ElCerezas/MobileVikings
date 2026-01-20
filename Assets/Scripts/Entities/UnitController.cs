using UnityEngine;

public class UnitController : MonoBehaviour
{
    Unit unit;
    [SerializeField] Collider unitCollider;
    private void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public void TurnCollider(bool on)
    {
        unitCollider.enabled = on;
    }
    private void OnMouseDown()
    {
        UnitPlacementManager.instance.OnUnitSelected(unit);
    }
}
