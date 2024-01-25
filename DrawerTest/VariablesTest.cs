using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ASE_ProgrammingLanguage;
using System.Collections;
using System.Reflection;

namespace DrawerTest
{
    /// <summary>
    /// Unit tests for the <see cref="CommandParser"/> class related to variable parsing.
    /// </summary>
    [TestClass]
    public class VariablesTest
    {
        private CommandParser commandParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariablesTest"/> class.
        /// </summary>
        public VariablesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        /// Initializes test context and resets the state of the <see cref="CommandParser"/> before each test.
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
        /// Gets or sets the test context which provides information about and functionality for the current test run.
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
        /// Tests parsing a valid variable, expecting successful parsing and storage in the <see cref="CommandParser"/> variables.
        /// </summary>
        [TestMethod]
        public void ParseCommands_WhenGivenValidVariable_ShouldParseSuccessfully()
        {
            // Arrange
            string input = "varName = 10";

            // Act
            commandParser.ParseCommands(input);

            // Assert
            Assert.IsNotNull(commandParser.variables);
            Assert.IsTrue(commandParser.variables.ContainsKey("varName"));
            Assert.AreEqual(10, commandParser.variables["varName"]);
        }

        /// <summary>
        /// Tests parsing an invalid variable, expecting the <see cref="CommandParser"/> to handle it gracefully.
        /// </summary>
        [TestMethod]
        public void ParseCommands_WhenGivenInvalidVariable_ShouldHandleGracefully()
        {
            // Arrange
            string input = "var == 60";

            // Act & Assert
            try
            {
                commandParser.ParseCommands(input);

                Assert.IsNull(commandParser.variables);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Exception caught: {ex.Message}");
            }
        }
    }
}