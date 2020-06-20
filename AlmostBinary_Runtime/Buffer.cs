using System.Collections.Generic;

namespace AlmostBinary_Runtime
{
    class Buffer
    {
        public List<object> buffer = new List<object>();
        public int pos = 0;

        public int ReadInt()
        {
            return (int)buffer[pos++];
        }

        public string ReadString()
        {
            return (string)buffer[pos++];
        }

        public void Write(int data)
        {
            buffer.Add(data);
        }

        public void Write(string data)
        {
            buffer.Add(data);
        }
    }
}
