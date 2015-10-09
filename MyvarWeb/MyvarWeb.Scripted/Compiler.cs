using Microsoft.CSharp;
using MyvarWeb.Scripted.Properties;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.CodeDom;

namespace MyvarWeb.Scripted
{
    public static class Compiler
    {

        public static bool ReCompile { get; set; }
        public static bool DebugMode { get; set; }
  

        public static string Compile(string inpath, string outpath)
        {
           
            string ret = "";
            using (var codeProvider = new CSharpCodeProvider())
            {

                var parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.OutputAssembly = outpath;
                parameters.ReferencedAssemblies.Add("System.Net.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                string Bootstrap = Resources.BootStrap;

                string code = "";
                string raw = File.ReadAllText(inpath);
                bool inCode = false;
                string s1 = raw;

                string tmp = "";

                for (int i = 0; i < s1.Length; i++)
                {
                    var x = s1[i];
                    if (!inCode)
                    {
                        if (x == '<')
                        {
                            if (s1[i + 1] == '?')
                            {
                                if (s1[i + 2] == 'c')
                                {
                                    if (s1[i + 3] == 's')
                                    {
                                        code += "\nret += " + ToLiteral(tmp) + ";\n";
                                        tmp = "";
                                        inCode = true;
                                        i += 3;
                                        goto Done;
                                    }
                                }

                            }
                        }

                        tmp += x;
                    }
                    else
                    {
                        if (x == '?')
                        {
                            if (s1[i + 1] == '>')
                            {
                                inCode = false;

                                i += 2;
                                goto Done;
                            }
                        }

                        code += x;
                    }
                    Done:
                    var xyz = 10;

                    if (i == s1.Length - 1)
                    {
                        code += "\nret += " + ToLiteral(tmp) + ";\n";
                        tmp = "";
                    }

                }
                var xx = Bootstrap.Replace("/* Code */", code);
                CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, xx);
                codeProvider.Dispose();
               
                if (DebugMode)
                {
                    if (results.Errors.Count != 0)
                    {
                        foreach (CompilerError i in results.Errors)
                        {
                            ret += i.ToString() + Environment.NewLine;
                        }
                    }
                }
            }
           
            return ret;
        }

        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }


        public static object Execute(string path, HttpListenerRequest req, bool cash = false)
        {
            string ret = "";
            if(!Directory.Exists("tmp"))
            {
                Directory.CreateDirectory("tmp");
            }

            var info = new FileInfo(path);
            var tmpf = Path.Combine("tmp", info.Name.Split('.')[0] + ".dll");

            if (ReCompile)
            {
                bool doitt = true;
                if (File.Exists(tmpf))
                {
                    if (cash)
                    {
                        doitt = false;
                    }
                    else
                    {
                        File.Delete(tmpf);
                    }
                }
                else
                {
                    if (cash)
                    {
                        ret += Compile(path, tmpf) + "\n";
                    }

                }
                if (!cash)
                {
                    ret += Compile(path, tmpf) + "\n";
                }
            }
            if (File.Exists(tmpf))
            {
               
                var ta = Assembly.Load(File.ReadAllBytes(tmpf));
            
                var type = ta.GetType("Page.Main");
                var metod = type.GetMethods()[0];
             
                ret += metod.Invoke(null, null);
                if (!cash)
                {
                    File.Delete(tmpf);
                }
            }
            else
            {
                ret += "Error Chould not find compiled page";
            }
            return ret;
        }
    }
}
