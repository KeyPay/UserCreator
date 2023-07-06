using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace UserCreator
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            string fieldType;
            if (args.Length != 1)
            {
                await Console.Out.WriteLineAsync($"Usage: UserCreator [outputfile]");
                return 1;
            }

            await using var userDataIOHandler = UserDataIOHandler.GetInstance(args[0]);

            var parser = new Parser(userDataIOHandler);

            while (!string.IsNullOrEmpty(fieldType = await userDataIOHandler.GetFieldType()))
            {
                var dataAsString = await userDataIOHandler.GetData(fieldType);

                await parser.HandleData(fieldType, dataAsString);

                Console.WriteLine($"============");
            }

            return 0;
        }
    }
}
