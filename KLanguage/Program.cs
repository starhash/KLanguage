using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infinity.Grammar;
using Infinity.Grammar.Utility;
using Infinity.Utility;
using Infinity.Scripting.IGS;
using Infinity.Grammar.Parsing;

namespace KLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) Console.WriteLine("KLanguage ?filename.k -- runs a K Language file\nKLanguage -manual -- Displays manual");
            else if (args[0] == "-manual")
            {
                string[] lines = Properties.Resources.KGrammar.Split(new char[] { '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
                lines = lines.Select((x) => x.Replace("GrammarElement", "")
                                             .Replace("SymbolSet", "")
                                             .Replace("Symbol", "")
                                             .Replace("<C>", "")
                                             .Replace("<M>", "")
                                             .Replace("<R>", "")
                                             .Trim()
                                             ).ToArray();
                Console.WriteLine("READ THE GRAMMAR PLEASE\n\n");
                foreach (string s in lines)
                    Console.Write(s.Trim() + "\n");
            }
            else
            {
                Infinity.Engine.Runtime.RuntimeAssemblyManager.InitAssemblies();
                Grammar kgrammar = new Grammar();
                InfinityGrammarScriptEngine.LoadGrammar(kgrammar, Properties.Resources.KGrammar);
                FileInfo input = new FileInfo(args[0]);
                if (input.Exists)
                {
                    string code = input.OpenText().ReadToEnd();
                    string backup = code;
                    TestResult parse = kgrammar.Parse(code, true);
                    if (parse.Result)
                    {
                        new KEnv().Interpret((ParseTreeNode)parse.Data["$PARSETREENODE$"]);
                    }
                }
                Console.WriteLine("[INTERPRETATION COMPLETE: PRESS ANY KEY TO EXIT]");
                Console.ReadKey();
            }
        }
    }
}
