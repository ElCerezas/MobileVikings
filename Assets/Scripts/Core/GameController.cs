using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum LevelPhase { Generate, SelectClass, SelectSubClass, PlaceHeroes, Combat, EndLevel}
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    PlayerData pd;

    public LevelLoader levelLoader;
    public SelectionPhaseManager selectionManager;
    //public PlacementPhaseManager placementManager;
    //public CombatManager combatManager;
    //public TickManager tickManager;

    public LevelPhase CurrentPhase { get; private set; } = LevelPhase.Generate;
    private void Awake()
    {
        Instance = this;
        pd = PlayerData.instance;
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            StartLevel(pd.GetLevel());
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
    public HeroData[] GetBaseDeck()
    {
        return pd.deck;
    }

    #region PhasesTriggers
    public void ChangePhase(LevelPhase newPhase)
    {
        CurrentPhase = newPhase;
        Debug.Log($"Phase changed to: {newPhase}");
        if( CurrentPhase == LevelPhase.SelectClass)
        {
            selectionManager.StartClassSelection();
        }
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
