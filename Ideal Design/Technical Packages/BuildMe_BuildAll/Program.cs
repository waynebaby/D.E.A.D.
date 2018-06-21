using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildMe_BuildAll
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancel = new CancellationTokenSource();
            var _ = Task.Run(() => ConsoleInputCircle(cancel)); // if need console input.

            ArgumentsHandlingAsync(args, cancel.Token).Wait(cancel.Token);

        }
        /// <summary>
        /// Circling read input. if command met, execute.
        /// </summary>
        /// <param name="endExecutionTokenSource"></param>
        /// <returns></returns>
        static async Task ConsoleInputCircle(CancellationTokenSource endExecutionTokenSource)
        {
            for (; ; )
            {
                switch (Console.ReadLine())
                {


                    case string cmd when string.Compare(cmd, "quit", true) == 0:
                        endExecutionTokenSource.Cancel();
                        break;
                    default:
                        break;
                }

            }
        }
        static async Task ArgumentsHandlingAsync(string[] args, CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {

                switch (args.FirstOrDefault()?.ToLower())
                {
                    case "gatherpackages":
                        if (args.Length == 3)
                        {
                            var source = args[1];
                            var target = args[2];
                            if (Directory.Exists(source))
                            {
                                if (!Directory.Exists(target))
                                {
                                    Directory.CreateDirectory(target);
                                }

                                await Task.Run(() =>
                                {
                                    var di = new DirectoryInfo(source);
                                    foreach (var fileInfo in di.EnumerateFiles("*.nupkg",  SearchOption.AllDirectories))
                                    {
                                        var file = fileInfo.FullName;
                                        if (cancellationToken.IsCancellationRequested)
                                        {
                                            Console.WriteLine($"Operation cancelled by user.");
                                        }

                                        if (Directory.GetParent(file).Parent.Name.StartsWith("DEAD."))
                                        {
                                            var destFile = Path.Combine(target, Path.GetFileName(file));
                                            Console.WriteLine($"{file} is moving to {destFile}");

                                            if (!string.Equals(file, destFile, StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                File.Copy(file, destFile, true);
                                            }

                                            Console.WriteLine($"{file} have been moved to {destFile}");
                                        }
                                    }
                                });


                                Console.WriteLine($"all packages from {source} have been moved to {target}");

                                break;
                            }
                            else
                            {
                                Console.WriteLine("Source path not exists");
                                break;
                            }

                        }
                        break;
                    default:
                        break;
                }





            }


            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
