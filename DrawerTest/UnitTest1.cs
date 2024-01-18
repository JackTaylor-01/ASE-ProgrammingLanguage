using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using ASE_ProgrammingLanguage;


namespace DrawerTest
{
    [TestClass]

    public class UnitTest1
    {
        Drawer drawer;
        /// <summary>
        /// Setup initalises whats needed before each test. In this case a drawer object
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Initialize your Drawer object before each test
            drawer = new Drawer(Graphics.FromImage(new Bitmap(451, 375)));
        }

        /// <summary>
        /// Tests drawline method giving it valid arguments
        /// </summary>
        [TestMethod]
        public void DrawLine_ValidArguments_ShouldNotThrowException()
        {
            try
            {
                drawer.DrawLine(50, 50);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown: {ex.Message}");
            }
        }
        /// <summary>
        /// Tests MoveTo method giving it valid arguments
        /// </summary>
        [TestMethod]
        public void MoveTo_ValidArguments_ShouldSetCoordinates()
        {
            drawer.MoveTo(30, 40);
            Assert.AreEqual(30, drawer.xPos);
            Assert.AreEqual(40, drawer.yPos);
        }
        /// <summary>
        /// Tests the DrawTo method giving it valid arguments
        /// </summary>
        [TestMethod]
        public void DrawTo_ValidArguments_ShouldDrawAndSetCoordinates()
        {
            drawer.DrawTo(150, 80);
            Assert.AreEqual(150, drawer.xPos);
            Assert.AreEqual(80, drawer.yPos);
        }
        /// <summary>
        /// Tests the clear method
        /// </summary>
        [TestMethod]
        public void Clear_ShouldClearPictureBox()
        {
            drawer.Clear();
        }
        /// <summary>
        /// Tests the reset method
        /// </summary>
        [TestMethod]
        public void Reset_ShouldResetCoordinatesToZero()
        {
            drawer.MoveTo(50, 60);
            drawer.Reset();
            Assert.AreEqual(0, drawer.xPos);
            Assert.AreEqual(0, drawer.yPos);
        }
        /// <summary>
        /// Tests the DrawRectangle method with valid arguments
        /// </summary>
        [TestMethod]
        public void DrawRectangle_ValidArguments_ShouldNotThrowException()
        {
            try
            {
                drawer.DrawRectangle(30, 40);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown: {ex.Message}");
            }
        }

        /// <summary>
        /// Tests DrawCircle method with valid arguments
        /// </summary>
        [TestMethod]
        public void DrawCircle_ValidArguments_ShouldNotThrowException()
        {
            try
            {
                drawer.DrawCircle(20);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown: {ex.Message}");
            }
        }

        /// <summary>
        /// Tests DrawTriangle method with valid arguments
        /// </summary>
        [TestMethod]
        public void DrawTriangle_ValidArguments_ShouldNotThrowException()
        {
            try
            {
                drawer.DrawTriangle(30,30);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown: {ex.Message}");
            }
        }

        /// <summary>
        /// Tests SetPenColour method with valid arguments
        /// </summary>
        [TestMethod]
        public void SetPenColour_ValidColour_ShouldSetPenColour()
        {
            drawer.SetPenColour("Blue");
            Assert.AreEqual(Color.Blue, drawer.pen.Color);
        }

        /// <summary>
        /// Tests Enablefill method
        /// </summary>
        [TestMethod]
        public void EnableFill_ShouldSetBrushForFilling()
        {
            drawer.EnableFill();
        }

        /// <summary>
        /// Tests DisableFill method
        /// </summary>
        [TestMethod]
        public void DisableFill_ShouldSetBrushToNull()
        {
            drawer.DisableFill();
        }

    }
}
