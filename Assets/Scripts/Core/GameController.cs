using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum LevelPhase { Generate, SelectClass, SelectSubClass, PlaceHeroes, Combat, EndLevel}
public class GameController : MonoBehaviour
{
    public static GameController Instance;


    public LevelLoader levelLoader;
    //public PlacementPhaseManager placementManager;
    //public CombatManager combatManager;
    //public TickManager tickManager;

    public LevelPhase CurrentPhase { get; private set; } = LevelPhase.Generate;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            StartLevel(1);
        }
    }
    public void StartLevel(int id)
    {
        ChangePhase(LevelPhase.Generate);


        LevelData data = levelLoader.LoadLevelFromResources("Levels",id);
        if (data == null)
        {
            Debug.LogError($"Cannot start level {id}: not found.");
            return;
        }


        levelLoader.GenerateLevel(data);
        //BeginHeroSelection(data);
    }
    #region PhasesTriggers
    public void ChangePhase(LevelPhase newPhase)
    {
        CurrentPhase = newPhase;
        Debug.Log($"Phase changed to: {newPhase}");
    }
    /*
    private void BeginHeroSelection(LevelData data)
    {
        ChangePhase(LevelPhase.SelectClass);
    }
    public void OnHeroesSelected()
    {
        ChangePhase(LevelPhase.SelectSubClass);
    }
    public void OnSubClassesChosen()
    {
        ChangePhase(LevelPhase.PlaceHeroes);
    }
    public void OnPlacementFinished()
    {
        ChangePhase(LevelPhase.Combat);
        tickManager.StartTicks();
    }
    public void EndLevel()
    {
        ChangePhase(LevelPhase.EndLevel);
        tickManager.StopTicks();
    }
    */
    #endregion
}
