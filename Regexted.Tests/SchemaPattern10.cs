using Xunit;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Regexted;

namespace Regexted.Tests
{
    public class SchemaPattern10 : IClassFixture<TestDataFixture>
    {
        TestDataFixture fixture;
        const string pattern = "SchemaPattern10";

        public SchemaPattern10(TestDataFixture fixture)
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
                !validator.hasValidationErrors &&
                !validator.hasCompletedValidation &&
                validator.hasRuntimeErrors &&
                validator.runtimeErrors.Any(e => e.Contains("The Pattern constraining facet is invalid"))
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
                !validator.hasValidationErrors &&
                !validator.hasCompletedValidation &&
                validator.hasRuntimeErrors &&
                validator.runtimeErrors.Any(e => e.Contains("The Pattern constraining facet is invalid"))
                );
        }
    }
}