public sealed class ChristmasQuestValidator : QuestValidatorBase
{
    public override PuzzleOutcome CheckPuzzle(Quest quest)
    {
        int christmasItemCount = 0;
        int otherItemCount = 0;

        NestItem middleItem = quest.Board[1, 1];
        bool treeIsInMiddle = middleItem != null && middleItem.IsChristmasTree;

        foreach (NestItem item in quest.Board)
        {
            if (item == null || item.IsChristmasTree)
                continue;

            if (item.IsChristmas)
                christmasItemCount++;
            else
                otherItemCount++;
        }

        if (!treeIsInMiddle)
            return PuzzleOutcome.IncorrectPlacement;

        if (otherItemCount > 0)
            return PuzzleOutcome.WrongItemsPresent;

        if (christmasItemCount < 3)
            return PuzzleOutcome.NotEnoughItems;

        return PuzzleOutcome.Perfect;
    }
}
