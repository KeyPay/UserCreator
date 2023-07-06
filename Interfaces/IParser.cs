using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserCreator
{
    public interface IParser
    {
        public Task HandleData(string fieldType, string dataAsString);
    }
}
