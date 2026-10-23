namespace Game
{
    public readonly struct GridPos
    {
        public readonly int Row;
        public readonly int Col;

        public GridPos(int row, int col) { Row = row; Col = col; }

        public bool Equals(GridPos other) => Row == other.Row && Col == other.Col;

        // ★ 新增：曼哈顿距离
        public int ManhattanTo(GridPos other)
            => System.Math.Abs(Row - other.Row) + System.Math.Abs(Col - other.Col);
    }
}
