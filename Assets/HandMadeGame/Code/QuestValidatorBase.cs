using UnityEngine;

public abstract class QuestValidatorBase : MonoBehaviour
{
    public abstract PuzzleOutcome CheckPuzzle(Quest quest);
}
