using System;
using System.Collections.Generic;
using CommandLine;
using GPart;
using System.Diagnostics;


namespace Host
{
    static class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);      
        }

        static void RunOptions(CommandLineOptions opts)
        {
            var graph = GraphFactory.LoadFrom(opts.Path);        

            var alg = AlgorithmBuilder.Create()
                //.UseGraphBooster()
                .UseRadiusMod()
                .Build();

            for (var i = 0; i < 50; i++)
            {
                Stopwatch stopWatch = new Stopwatch();

                stopWatch.Start();
                var sln = alg.Run(graph, opts.Section, opts.Level);
                stopWatch.Stop();

                Console.WriteLine(sln.CrossPower);

                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);

                
            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle
        }
    }
}