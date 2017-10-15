using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rubberduck.Inspections.Concrete;
using Rubberduck.Parsing.Inspections.Resources;
using RubberduckTests.Mocks;

namespace RubberduckTests.Inspections
{
    [TestClass]
    public class ModuleScopeDimKeywordInspectionTests
    {
        [TestMethod]
        [TestCategory("Inspections")]
        public void ModuleScopeDimKeyword_ReturnsResult()
        {
            const string inputCode =
@"Dim foo As String";
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new ModuleScopeDimKeywordInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var inspectionResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.AreEqual(1, inspectionResults.Count());
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void ModuleScopeDimKeyword_ReturnsResult_Multiple()
        {
            const string inputCode =
@"Dim foo
Dim bar";
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new ModuleScopeDimKeywordInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var inspectionResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.AreEqual(2, inspectionResults.Count());
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void ModuleScopeDimKeyword_DoesNotReturnResult()
        {
            const string inputCode =
@"Private foo";
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new ModuleScopeDimKeywordInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var inspectionResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.IsFalse(inspectionResults.Any());
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void ModuleScopeDimKeyword_IgnoreModule_All_YieldsNoResult()
        {
            const string inputCode =
@"'@IgnoreModule

Dim foo";
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new ModuleScopeDimKeywordInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var inspectionResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.IsFalse(inspectionResults.Any());
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void ModuleScopeDimKeyword_IgnoreModule_AnnotationName_YieldsNoResult()
        {
            const string inputCode =
@"'@IgnoreModule ModuleScopeDimKeyword

Dim foo";
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new ModuleScopeDimKeywordInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var inspectionResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.IsFalse(inspectionResults.Any());
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void ModuleScopeDimKeyword_IgnoreModule_OtherAnnotationName_YieldsResults()
        {
            const string inputCode =
@"'@IgnoreModule VariableNotUsed

Dim foo";
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new ModuleScopeDimKeywordInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var inspectionResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.IsTrue(inspectionResults.Any());
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void ModuleScopeDimKeyword_Ignored_DoesNotReturnResult()
        {
            const string inputCode =
@"'@Ignore ModuleScopeDimKeyword
Dim foo";
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new ModuleScopeDimKeywordInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var inspectionResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.IsFalse(inspectionResults.Any());
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void InspectionType()
        {
            var inspection = new ModuleScopeDimKeywordInspection(null);
            Assert.AreEqual(CodeInspectionType.LanguageOpportunities, inspection.InspectionType);
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void InspectionName()
        {
            const string inspectionName = "ModuleScopeDimKeywordInspection";
            var inspection = new ModuleScopeDimKeywordInspection(null);

            Assert.AreEqual(inspectionName, inspection.Name);
        }
    }
}
