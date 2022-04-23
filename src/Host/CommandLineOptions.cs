using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Host
{
    internal class CommandLineOptions
    {
        [Option(
            'p',
            "path",
            Required = true,
            HelpText = "Path to the file to be processed."
        )]
        public string Path { get; set; }

        [Option(
            'k', 
            "ksections", 
            Required = false,
            HelpText = "Partitions number.",
            Default = 2
        )]
        public int Section { get; set; }

        [Option(
            'l',
            "levels",
            Required = false,
            Default = 2,
            HelpText = "ML algorithm levels number.")]
        public int Level { get; set; }
    }
}
