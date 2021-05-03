namespace Game.Scripts.SkakBoard
{
    /// <summary>
    /// Sorting in skak is based on board lines (y coord)
    /// </summary>
    public static class Sorting
    {
        public static int BaseLineSortingStep = -16;

        public static int Square(int line) => line * BaseLineSortingStep;
    }
}