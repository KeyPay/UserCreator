using System.IO;
using System.Threading.Tasks;

namespace UserCreator
{
    public interface IUserDataEnterer
    {
        Task WriteDataToCsv(string fieldName, object data);
    }
}