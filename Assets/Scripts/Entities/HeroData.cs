using UnityEngine;

public class HeroData : ScriptableObject
{
    string name;
    [SerializeField] SubClass subclassA;
    [SerializeField] SubClass subclassB;
}
