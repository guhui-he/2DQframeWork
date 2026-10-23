namespace Game
{
    public readonly struct MoveRecord
    {
        public readonly int HeroId;
        public readonly GridPos From;
        public readonly GridPos To;
        public readonly int StepCost;
        public readonly int PrevCurId;

        public MoveRecord(int heroId, GridPos from, GridPos to, int stepCost, int prevCurId)
        {
            HeroId = heroId; From = from; To = to;
            StepCost = stepCost; PrevCurId = prevCurId;
        }
    }
}
