public sealed class NullQuestValidator : QuestValidatorBase
{
    public override PuzzleOutcome CheckPuzzle(Quest quest)
        => PuzzleOutcome.Perfect;
}
