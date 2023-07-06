using System;
using System.Collections.Generic;
using System.Text;

namespace UserCreator
{
    public interface IDataConverter<T>
    {
        public bool TryConvertData(string input, out T data);
    }
}
