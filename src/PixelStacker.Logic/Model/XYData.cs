namespace PixelStacker.Logic.Model
{
    public class XYData<DataType>
    {
        public XYData()
        {
        }

        public XYData(int x, int y, DataType data)
        {
            this.X = x;
            this.Y = y;
            this.Data = data;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public DataType Data { get; set; }
    }
}
