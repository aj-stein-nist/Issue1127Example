using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

namespace Issue1127ExampleConsoleApp
{
    class Program
    {
        static string currentDir = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
        static string separator = Path.DirectorySeparatorChar.ToString();

        protected const string XMLNamespace = @"https://example.com/schema/1.0";

        static void Main(string[] args)
        {
            try
            {
                DirectoryInfo schemasPath = new DirectoryInfo($"{currentDir}{separator}..{separator}..{separator}..{separator}");
                Console.WriteLine($"Attemping schema compilation with \"{schemasPath.FullName}bad.xsd\"");
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add(XMLNamespace, $"{schemasPath.FullName}bad.xsd");
                schema.Compile();
            }
            catch (System.Xml.Schema.XmlSchemaException exs)
            {
                string msg = exs.Message;
                Console.WriteLine($"We got the intended exception, reproducting the error: \"{msg}\"");
            }

            try
            {
                DirectoryInfo schemasPath = new DirectoryInfo($"{currentDir}{separator}..{separator}..{separator}..{separator}");
                Console.WriteLine($"Attemping schema compilation with \"{schemasPath.FullName}good.xsd\"");
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add(XMLNamespace, $"{schemasPath.FullName}good.xsd");
                schema.Compile();
            }
            catch (Exception exs)
            {
                string msg = exs.Message;
                Console.Error.WriteLine($"We don't intended an exception this uses the good schema, but we got: \"{msg}\"");
            }
        }
    }
}