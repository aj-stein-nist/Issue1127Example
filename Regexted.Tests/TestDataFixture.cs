using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Regexted.Tests
{
    public class TestDataFixture : IDisposable
    {
        public DirectoryInfo contextPath { get; private set; }
        public XNamespace namespaceUri { get; private set; }
        public string namespacePrefix { get; private set; }

        public TestDataFixture()
        {
            string sep = Path.DirectorySeparatorChar.ToString();
            this.contextPath = new DirectoryInfo(
                $"{System.AppDomain.CurrentDomain.BaseDirectory.ToString()}{sep}..{sep}..{sep}..{sep}..{sep}..{sep}Fixtures{sep}"
            );
            this.namespaceUri = "https://example.com/schema/1.0";
            this.namespacePrefix = "example";
        }

        public void Dispose()
        {
            // Context data with configuration vars, nothing to destroy yet.
            // This is need for the IDisposable interface though.
        }
    }
}
