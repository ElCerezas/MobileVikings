using System.Collections.Generic;
using UnityEngine;

/*public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    private IPhase currentPhase;
    private int currentIndex = 0;

    private List<IPhase> phases = new List<IPhase>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        phases.Add(new GenerationPhase());
        phases.Add(new CardSelectionPhase(cardSelectionUI, levelData.selectableHeroes));
        phases.Add(new SubclassSelectionPhase(subclassSelectionUI, selectedClasses));
        phases.Add(new PlacementPhase());
        phases.Add(new CombatPhase());

        StartPhase(0);
    }

    private void Update()
    {
        currentPhase?.UpdatePhase();
    }

    public void StartPhase(int index)
    {
        currentPhase?.ExitPhase();

        currentIndex = index;
        currentPhase = phases[currentIndex];

        currentPhase.EnterPhase();
    }

    public void NextPhase()
    {
        StartPhase(currentIndex + 1);
    }
}
public class GenerationPhase : IPhase
{
    public void EnterPhase()
    {
        Debug.Log("Generation Phase START");

        LevelManager.instance.LoadLevel();
        PhaseManager.instance.NextPhase();
    }
    public void UpdatePhase()
    {
    }
    public void ExitPhase()
    {
        Debug.Log("Generation Phase END");
    }
}
public class CardSelectionPhase : IPhase
{
    private CardSelectionUI ui;
    private int maxSelectable;
    private List<CharacterClass> selectedClasses;

    public CardSelectionPhase(CardSelectionUI uiRef, int maxSelectable)
    {
        ui = uiRef;
        this.maxSelectable = maxSelectable;
    }

    public void EnterPhase()
    {
        ui.OnSelectionConfirmed += HandleSelection;
        ui.ShowClasses(GetNineRandomClasses(), maxSelectable);
    }

    public void UpdatePhase() { }

    public void ExitPhase()
    {
        ui.OnSelectionConfirmed -= HandleSelection;
    }

    private void HandleSelection(List<CharacterClass> classes)
    {
        selectedClasses = classes;
        PhaseManager.instance.NextPhase();
    }

    private List<CharacterClass> GetNineRandomClasses()
    {
        // Se obtiene de tu ClassDatabase.
        return ClassDatabaseLoader.Instance.GetRandom(9);
    }
}
public class SubclassSelectionPhase : IPhase
{
    private SubclassUI ui;
    private List<CharacterClass> classes;
    private List<CharacterSubclass> chosen;
    private int index = 0;

    public SubclassSelectionPhase(SubclassUI uiRef, List<CharacterClass> selectedClasses)
    {
        ui = uiRef;
        classes = selectedClasses;
        chosen = new List<CharacterSubclass>();
    }

    public void EnterPhase()
    {
        ui.OnSubclassChosen += HandleSubclass;
        ShowNext();
    }

    public void UpdatePhase() { }

    public void ExitPhase()
    {
        ui.OnSubclassChosen -= HandleSubclass;
    }

    private void HandleSubclass(CharacterSubclass s)
    {
        chosen.Add(s);
        index++;

        if (index >= classes.Count)
        {
            PhaseManager.instance.NextPhase();
            return;
        }

        ShowNext();
    }

    private void ShowNext()
    {
        ui.Show(classes[index].subclasses);
    }
}*/