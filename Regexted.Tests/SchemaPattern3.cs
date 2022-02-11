using Xunit;
using System;
using System.IO;
using System.Xml.Linq;
using Regexted;

namespace Regexted.Tests
{
    public class SchemaPattern3 : IClassFixture<TestDataFixture>
    {
        TestDataFixture fixture;
        const string pattern = "SchemaPattern3";

        public SchemaPattern3(TestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Valid()
        {
            string schema = $"{fixture.contextPath}{pattern}.xsd";
            XDocument document = XDocument.Load($"{fixture.contextPath}{pattern}Valid.xml");
            XmlValidator validator = new XmlValidator(fixture.namespaceUri, fixture.namespacePrefix, schema, document);
            validator.Validate();
            Assert.True(
                !validator.hasRuntimeErrors &&
                !validator.hasValidationErrors &&
                validator.hasCompletedValidation
                );
        }

        [Fact]
        public void Invalid()
        {
            string schema = $"{fixture.contextPath}{pattern}.xsd";
            XDocument document = XDocument.Load($"{fixture.contextPath}{pattern}Invalid.xml");
            XmlValidator validator = new XmlValidator(fixture.namespaceUri, fixture.namespacePrefix, schema, document);
            validator.Validate();
            Assert.True(
                !validator.hasRuntimeErrors &&
                validator.hasCompletedValidation &&
                validator.hasValidationErrors
                );
        }
    }
}