using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserCreator
{
    public class Parser: IParser
    {
        private static readonly object _locker = new object();

        private readonly IUserDataEnterer _userDataIOHandler;

        private static Parser _instance;
        public Parser(IUserDataEnterer userDataIOHandler)
        {
            _userDataIOHandler = userDataIOHandler;
        }

        public static Parser GetInstance(IUserDataEnterer userDataIOHandler)
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new Parser(userDataIOHandler);
                    }
                }
            }
            return _instance;
        }

        public async Task HandleData(string fieldType, string dataAsString)
        {
            if (string.Equals("DateOfBirth", fieldType, StringComparison.CurrentCultureIgnoreCase))
            {
                await WriteUserDataToFile<DateTime>(fieldType, dataAsString);
            }
            else if (string.Equals("Salary", fieldType, StringComparison.CurrentCultureIgnoreCase))
            {
                await WriteUserDataToFile<decimal>(fieldType, dataAsString);
            }
            else
            {
                await WriteUserDataToFile<string>(fieldType, dataAsString);
            }
        }

        private async Task WriteUserDataToFile<TDataType>(string fieldName, string dataAsString)
        {

            var userDataConverter = new UserDataConverter<TDataType>();

            if (userDataConverter.TryConvertData(dataAsString, out var data))
            {
                await _userDataIOHandler.WriteDataToCsv(fieldName, data);
            }
        }
    }
}
