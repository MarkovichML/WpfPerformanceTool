using System.Collections.Generic;

namespace Core
{
    public interface IDataInteraction<T>
    {
        void WriteData(List<T> data);

        List<T> ReadData();
    }
}
