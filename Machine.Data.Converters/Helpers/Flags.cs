namespace Machine.Data.Converters.Helpers
{
    public class Flags
    {
        int _count;
        int _size;
        bool[] _flags;

        public Flags(int size)
        {
            _size = size;
            _flags = new bool[size];
        }

        public bool Add(bool value = true)
        {
            bool result = false;

            if (_count < _size)
            {
                _flags[_count] = value;
                _count++;

                if ((_count == _size) && AllTrue()) result = true;
            }

            return result;
        }

        public bool AllTrue()
        {
            bool result = true;

            for (int i = 0; i < _size; i++)
            {
                if (!_flags[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }

}
