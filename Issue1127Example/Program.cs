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
        static XNamespace xmlNamespaceUri = "https://example.com/schema/1.0";
        protected const string xmlNamespacePrefix = "example";

        static void Main(string[] args)
        {
            string[] schemaFileNames = new string[] { 
                "character_group_word.xsd",
                "character_group_xml_unicode_ranges.xsd",
                "single_ascii_character.xsd",
                "single_escaped_unicode_character.xsd"
            };

            foreach (var fileName in schemaFileNames)
            {
                try
                {
                    DirectoryInfo schemasPath = new DirectoryInfo($"{currentDir}{separator}..{separator}..{separator}..{separator}");   
                    Console.WriteLine($"\n\nAttemping schema compilation with \"{schemasPath.FullName}{fileName}\"");
                    XmlSchemaSet schema = new XmlSchemaSet();
                    schema.Add(xmlNamespaceUri.ToString(), $"{schemasPath.FullName}{fileName}");
                    schema.Compile();

                    XDocument sample = new XDocument(
                        new XElement(xmlNamespaceUri + "letterA", "B")
                    );

                    bool errors = false;
                    sample.Validate(schema, (o, e) => {
                        Console.WriteLine($"\t{fileName}: {e.Message}");
                        errors = true;
                    });

                    Console.WriteLine("file {0}", errors ? "did not validate" : "validated");
                }
                catch (Exception exs)
                {
                    string msg = exs.Message;
                    Console.Error.WriteLine($"\t Exception with {fileName}: \"{msg}\"");
                }

            }
        }
    }
}