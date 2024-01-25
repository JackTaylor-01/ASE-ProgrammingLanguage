using ASE_ProgrammingLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DrawerTest
{
    /// <summary>
    /// Unit tests for the <see cref="CommandParser"/> class related to while statements.
    /// </summary>
    [TestClass]
    public class WhileTest
    {
        private CommandParser commandParser;
        private TestContext testContextInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhileTest"/> class.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            commandParser = CommandParser.Instance;
            ResetCommandParserState();
        }

        private void ResetCommandParserState()
        {
            commandParser.GetType().GetField("instance", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, null);
        }

        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        /// Asserts iteration when a while statement has a true condition, expecting the result to be "true".
        /// </summary>
        public void AssertIteration_WhenWhileStatementWithTrueCondition_ShouldReturnTrue()
        {
            // Arrange
            commandParser.variables.Add("var1", 5); // Add any necessary variables

            string whileStatement = "while var1 == 5";

            // Act
            string result = commandParser.AssertIteration(whileStatement);

            // Assert
            Assert.AreEqual("true", result);
        }

        /// <summary>
        /// Asserts iteration when a while statement has a false condition, expecting the result to be "NonMatch".
        /// </summary>
        [TestMethod]
        public void AssertIteration_WhenWhileStatementWithFalseCondition_ShouldReturnNonMatch()
        {
            // Arrange
            commandParser.variables.Add("var1", 10); // Add any necessary variables

            string whileStatement = "while var1 == 5";

            // Act
            string result = commandParser.AssertIteration(whileStatement);

            // Assert
            Assert.AreEqual("NonMatch", result);
        }

        /// <summary>
        /// Asserts iteration when a while statement references an undefined variable, expecting the result to be "NonVar".
        /// </summary>
        [TestMethod]
        public void AssertIteration_WhenWhileStatementWithUndefinedVariable_ShouldReturnNonVar()
        {
            // Arrange
            string whileStatement = "while undefinedVar == 5";

            // Act
            string result = commandParser.AssertIteration(whileStatement);

            // Assert
            Assert.AreEqual("NonVar", result);
        }

        /// <summary>
        /// Asserts iteration when an invalid while statement is provided, expecting an <see cref="OtherException"/> to be thrown.
        /// </summary>
        [TestMethod]
        public void AssertIteration_WhenInvalidWhileStatement_ShouldThrowOtherException()
        {
            // Arrange
            string invalidWhileStatement = "while invalid statement";

            // Act & Assert
            Assert.ThrowsException<OtherException>(() => commandParser.AssertIteration(invalidWhileStatement));
        }
    }
}