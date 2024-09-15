public sealed class ParentQuestValidator : QuestValidatorBase
{
    public override PuzzleOutcome CheckPuzzle(Quest quest)
    {
        int softItemCount = 0;
        int otherItemCount = 0;

        foreach (NestItem item in quest.Board)
        {
            if (item == null)
                continue;

            if (item.IsSoft)
                softItemCount++;
            else
                otherItemCount++;
        }

        if (otherItemCount > 0)
            return PuzzleOutcome.WrongItemsPresent;

        if (softItemCount < 3)
            return PuzzleOutcome.NotEnoughItems;

        return PuzzleOutcome.Perfect;
    }
}
