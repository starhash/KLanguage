using Infinity.Grammar.Parsing;
using Infinity.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLanguage
{
    public class KEnv
    {
        public KEnv()
        {
            
        }

        public void Interpret(ParseTreeNode ptn)
        {
            switch(ptn.Name)
            {
                case "PROGRAM":
                    Interpret((ParseTreeNode)ptn.Children[1]);
                    break;
                case "CODE":
                    ParseTreeNode node = (ParseTreeNode)ptn.Children[0];
                    foreach(ParseTreeNode p in node.Children)
                    {
                        Interpret(p);
                    }
                    break;
                case "STMT":
                    Interpret((ParseTreeNode)ptn.Children[0]);
                    break;
                case "DECLR":
                    string number = ((ParseTreeNode)ptn.Children[1]).InternalValue.Substring(2);
                    number = number.Replace('k', '0').Replace('K', '1');
                    int idx = Convert.ToInt32(number, 2);
                    if (!VariableEngine.GetPool("/").HasVariableWithName("var" + idx))
                    {
                        VariableEngine.Put("/var" + idx, new Variable(0, "/Integer"));
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Already DECLARED 0x"+number);
                    }
                    break;
                case "PRINT":
                    ParseTreeNode data = (ParseTreeNode)ptn.Children[1];
                    foreach(ParseTreeNode p in data.Children)
                    {
                        number = p.InternalValue;
                        number = number.Replace('k', '0').Replace('K', '1');
                        idx = Convert.ToInt32(number, 2);
                        if (idx > 255)
                        {
                            int var = idx % 256;
                            if (VariableEngine.GetPool("/").HasVariableWithName("var" + var))
                            {
                                Variable v = VariableEngine.Get("/var" + var);
                                Console.Write(v.Value);
                            }
                            else
                            {
                                Console.Write("[ERROR:0x" + number + " NOT FOUND]");
                            }
                        }
                        else
                        {
                            Console.Write((char)idx);
                        }
                    }
                    Console.Write("\n");
                    break;
                case "INPUT":
                    number = ((ParseTreeNode)ptn.Children[1]).InternalValue;
                    number = number.Replace('k', '0').Replace('K', '1');
                    idx = Convert.ToInt32(number, 2);
                    if (VariableEngine.GetPool("/").HasVariableWithName("var" + idx))
                    {
                        Variable v = VariableEngine.Get("/var" + idx);
                        bool ok; byte bt;
                        while(!(ok = byte.TryParse(Console.ReadLine(), out bt)))
                        {
                            Console.WriteLine("[INPUT ERROR: PLEASE ENTER value BETWEEN 0 and 255]");
                        }
                        v.Value = (int)bt;
                    }
                    else
                    {
                        Console.WriteLine("[NOT DECLARED: 0x" + number + "]");
                    }
                    break;
            }
        }
    }
}
