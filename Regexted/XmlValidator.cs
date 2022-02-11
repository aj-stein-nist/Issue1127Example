using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

namespace Regexted
{
    public class XmlValidator
    {
        private XNamespace namespaceUri;
        private string namespacePrefix;
        private XmlSchemaSet schemaSet;
        private XDocument document;
        public bool hasValidationErrors = false;
        public List<string> validationErrors { get; private set; }
        public bool hasCompletedValidation = false;
        public bool hasRuntimeErrors = false;
        public List<string> runtimeErrors { get; private set; }

        public XmlValidator(XNamespace namespaceUri, string namespacePrefix, string schema, XDocument document)
        {
            this.namespaceUri = namespaceUri;
            this.namespacePrefix = namespacePrefix;
            this.schemaSet = new XmlSchemaSet();
            this.runtimeErrors = new List<string>();
            this.validationErrors = new List<string>();
            this.document = document;

            try
            {
                schemaSet.Add(namespaceUri.ToString(), schema);
                schemaSet.Compile();
            }
            catch (Exception e)
            {
                this.runtimeErrors.Add(e.Message);
                this.hasRuntimeErrors = true;
            }
        }

        public void Validate()
        {
            try
            {
                this.document.Validate(schemaSet, (o, e) =>
                {
                    this.validationErrors.Add(e.Message);
                    this.hasValidationErrors = true;
                });

                hasCompletedValidation = true;
            }
            catch (Exception e)
            {
                this.runtimeErrors.Add(e.Message);
                this.hasRuntimeErrors = true;
            }
        }
    }
}