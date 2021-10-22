namespace PixelStacker.Core.Model
{
    public class XYData<T>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public T Data { get; set; }

        public XYData(int x, int y, T t)
        {
            Data = t;
            Y = y;
            X = x;
        }
    }
}
