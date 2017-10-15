﻿using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RubberduckTests.Mocks;
using Rubberduck.Inspections.Concrete;
using Rubberduck.Parsing.Inspections.Resources;

namespace RubberduckTests.Inspections
{
    [TestClass, Ignore]
    public class EmptyForLoopBlockInspectionTests
    {
        [TestMethod]
        [TestCategory("Inspections")]
        public void EmptyForLoopBlock_InspectionType()
        {
            var inspection = new EmptyForLoopBlockInspection(null);
            var expectedInspection = CodeInspectionType.CodeQualityIssues;

            Assert.AreEqual(expectedInspection, inspection.InspectionType);
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void EmptyForLoopBlock_InspectionName()
        {
            const string expectedName = nameof(EmptyForLoopBlockInspection);
            var inspection = new EmptyForLoopBlockInspection(null);

            Assert.AreEqual(expectedName, inspection.Name);
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void EmptyForLoopBlock_DoesNotFiresOnImplementedLoopBlocks()
        {
            const string inputCode =
@"Sub Foo()
    Dim idx As Integer
    for idx = 1 to 100
        idx = idx + 2
    next idx
End Sub";
            CheckActualEmptyBlockCountEqualsExpected(inputCode, 0);
        }

        [TestMethod]
        [TestCategory("Inspections")]
        public void EmptyForLoopBlock_FiresOnEmptyLoopBlocks()
        {
            const string inputCode =
@"Sub Foo()
    Dim idx As Integer
    for idx = 1 to 100
    next idx
End Sub";
            CheckActualEmptyBlockCountEqualsExpected(inputCode, 1);
        }

        private void CheckActualEmptyBlockCountEqualsExpected(string inputCode, int expectedCount)
        {
            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out _);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new EmptyForLoopBlockInspection(state);
            var inspector = InspectionsHelper.GetInspector(inspection);
            var actualResults = inspector.FindIssuesAsync(state, CancellationToken.None).Result;

            Assert.AreEqual(expectedCount, actualResults.Count());
        }
    }
}
