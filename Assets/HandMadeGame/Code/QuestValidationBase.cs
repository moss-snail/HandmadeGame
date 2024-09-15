using UnityEngine;

public abstract class QuestValidationBase : MonoBehaviour
{
    public abstract PuzzleOutcome CheckPuzzle(Quest quest);
}
