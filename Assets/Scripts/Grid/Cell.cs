namespace Assets.Scripts.Grid
{
    public class Cell
    {
        private readonly int _x;
        private readonly int _y;
        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public bool IsEmpty = true;
        public bool IsSelected = false;
        public Cell(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}