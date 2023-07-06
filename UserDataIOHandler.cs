using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UserCreator
{
    public class UserDataIOHandler : IUserDataEnterer, IAsyncDisposable
    {
        private static int _nextId;

        private static readonly object _locker = new object();

        private static UserDataIOHandler _instance;

        private static int _maxCountOfBatch = 3; // should be fine-tuned

        private static readonly object _maxLocker = new object();

        private FileStream _fileStream;

        private StreamWriter _streamWriter;

        private UserDataIOHandler(string fileName)
        {
            _fileStream = File.OpenWrite(fileName);
            if (_streamWriter == null ) {
                _streamWriter = new StreamWriter(_fileStream);
            }
        }

        public static UserDataIOHandler GetInstance(string fileName)
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new UserDataIOHandler(fileName);
                    }
                }
            }
            return _instance;
        }


        public async Task WriteDataToCsv(string fieldName, object data)
        {
            try
            {
                await _streamWriter.WriteLineAsync($"{_instance.GetNextId()},{fieldName},{data}");

                lock (_maxLocker)
                {
                    // if number of line reachs the limit, flush the buffer and write to file
                    if (_nextId % _maxCountOfBatch == 0)
                    {
                        _streamWriter.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occurs: {e}");
            }
        }

        private int GetNextId()
        {
            return Interlocked.Increment(ref _nextId);
        }

        public async ValueTask DisposeAsync()
        {
            _instance = null;
            await _streamWriter.DisposeAsync();
            await _fileStream.DisposeAsync();
        }

        public async Task<string> GetFieldType()
        {
            await Console.Out.WriteLineAsync($"Please enter field, or enter to exit");
            return await Console.In.ReadLineAsync();
        }

        public async Task<string> GetData(string fieldName)
        {
            await Console.Out.WriteLineAsync($"Please enter user's {fieldName}:");
            return await Console.In.ReadLineAsync();
        }
    }
}
