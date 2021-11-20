namespace PixelStacker.Logic.Model
{
    public class ThreadSafe<T>
    {
        private object Padlock = new { };
        private T _val;
        public T Value
        {
            get { lock (Padlock) { return _val; } }
            set { lock (Padlock) { _val = value; } }
        }

        public ThreadSafe(T val)
        {
            this.Value = val;
        }
    }
}
