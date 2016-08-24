//-----------------------------------------------------------------------
// <copyright file="BoundaryProcessor.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.Model
{
    using System;
    using HalconDotNet;
    using ReactiveUI;

    /// <summary>
    /// Processor class for Finding and processing a wafer boundary.
    /// </summary>
    public class BoundaryProcessor : ProcessorBase
    {
        #region Private Declarations

        /// <summary>
        /// A test region for demonstration.
        /// </summary>
        private HRegion waferRegion = new HRegion();

        /// <summary>
        /// Stores the loaded unprocessed image. 
        /// </summary>
        private HImage image = new HImage();

        /// <summary>
        /// Stores a value indicating whether the class has been disposed. 
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Stores the radio button selection.
        /// </summary>
        private RadioButtonSelection selectedOption = RadioButtonSelection.Large;

        /// <summary>
        /// Stores the dilation size.
        /// </summary>
        private double dilationSize = 400.5;

        /// <summary>
        /// Stores the intermediate processed region. 
        /// </summary>
        private HRegion processedRegion = new HRegion();
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BoundaryProcessor class. 
        /// </summary>
        public BoundaryProcessor()
            : base()
        {
            this.DisposeCollection.Add(this.WhenAnyValue(x => x.SelectedOption)
                .Subscribe(x => this.SelectedOptionToDilationSize(x))); 
        }

        #endregion

        #region Destructors
                
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the loaded unprocessed image. 
        /// </summary>
        public HImage Image
        {
            get
            { 
                return this.image; 
            }

            set 
            { 
                this.image = value; 
            }
        }

        /// <summary>
        /// Gets or sets the test region. 
        /// </summary>
        public HRegion WaferRegion
        {
            get 
            { 
                return this.waferRegion; 
            }

            set 
            {
                this.RaiseAndSetIfChanged(ref this.waferRegion, value); 
            }
        }

        /// <summary>
        /// Gets or sets the selected channel value. Non-reactive.
        /// </summary>
        public RadioButtonSelection SelectedOption
        {
            get
            {
                return this.selectedOption;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.selectedOption, value);
            }
        }         

        #endregion

        #region public methods

        /// <summary>
        /// Performs the process for this class. 
        /// </summary>
        /// <returns>A structure containing the processing results and error information.</returns>
        public override ProcessingResult Process()
        {
            this.ErrorCode = ProcessingErrorCode.NoError;
            this.ErrorMessage = "No errors detected.";
            ProcessingResult result = new ProcessingResult();

            try
            {
                if (this.ErrorCode == ProcessingErrorCode.NoError)
                {
                    // Call sub methods here. 
                    this.FindBoundary();
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                        result.ResultsCollection.Add("Area", this.CircularizeAndShrinkRegion(this.processedRegion));
                    }
                }

                // These lines pass any accumulated error information to the result class. 
                result.ErrorMessage = this.ErrorMessage;
                result.StatusCode = this.ErrorCode;
            }
            catch (Exception ex)
            {
                // If an exception gets here it is unexpected. 
                result.StatusCode = ProcessingErrorCode.UndefinedError;
                result.ErrorMessage = "An error occurred during processing: " + ex.Message;
            }
            finally
            {
                this.processedRegion.Dispose();
            }

            return result;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Overrides the Dispose method of IDisposable that actually disposes of managed resources. 
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the class is being disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                   //// Dispose of managed resorces here. 
                }

                //// Dispose of unmanaged resorces here.

                if (this.waferRegion != null)
                {
                    this.waferRegion.Dispose();
                }

                if (this.image != null)
                {
                    this.image.Dispose();
                }

                if (this.processedRegion != null)
                {
                    this.processedRegion.Dispose();
                }

                this.isDisposed = true;
            }

            // Call base.Dispose, passing parameter. 
            base.Dispose(disposing);
        }

        #endregion Protected Methods

        #region Private Methods       

        /// <summary>
        /// Example test method 1 for the Process call.
        /// </summary>
        private void FindBoundary()
        {
            HRegion lighter = new HRegion();
            HRegion filledRgn = new HRegion();
            HRegion closedRgn = new HRegion();
            HRegion connectedRgn = new HRegion();
            HRegion largestRegion = new HRegion();
            HRegion circleRegion = new HRegion();

            try
            {
                this.WaferRegion.Dispose();
                lighter = this.Image.Threshold(5.0, 255.0);
                filledRgn = lighter.FillUp();
                closedRgn = filledRgn.ClosingCircle(5.0);
                connectedRgn = closedRgn.Connection();
                largestRegion = connectedRgn.SelectShapeStd("max_area", 0.0);

                // System.Threading.Thread.Sleep(2000); // Does not block. 
                this.processedRegion = largestRegion.ShapeTrans("convex");

                // Debug display test. 
                // Uncomment and block processed display in View to see the debug output. 
                // Comment out call to CircularizeAndShrinkRegion for a persistent display. 
                ////this.DebugDisplay.Dispose();
                ////DisplayCollection tempDC = new DisplayCollection();
                ////tempDC.AddDisplayObject(lighter.CopyObj(1, -1), "red");
                ////this.DebugDisplay = tempDC;
            }
            catch (HalconException)
            {
                this.ErrorMessage = "Halcon Exception in FindBoundary";
                this.ErrorCode = ProcessingErrorCode.ProcessingStep1Error;
            }
            catch (Exception)
            {
                this.ErrorMessage = "System Exception in FindBoundary";
                this.ErrorCode = ProcessingErrorCode.ProcessingStep1Error;
            }
            finally
            {
                lighter.Dispose();
                filledRgn.Dispose();
                closedRgn.Dispose();
                connectedRgn.Dispose();
                largestRegion.Dispose();
                circleRegion.Dispose();
            }
        }

        /// <summary>
        /// Example test method 2 for the Process call.
        /// </summary>
        /// <param name="region">The region to modify.</param>
        /// <returns>The area of the region.</returns>
        private double CircularizeAndShrinkRegion(HRegion region)
        {
            HRegion circleRegion = new HRegion();
            double unusedDouble;
            double area = 0;

            try
            {
                // Test code. 
                // System.Threading.Thread.Sleep(2000); // Does not block. 
                // throw new Exception("Test Exception"); 
                circleRegion = region.ShapeTrans("circle");
                if (this.dilationSize > 0)
                {
                    this.WaferRegion = circleRegion.ErosionCircle(this.dilationSize);    // (400.5);
                }
                else
                {
                    this.WaferRegion = circleRegion.CopyObj(1, -1);
                }

                area = this.waferRegion.AreaCenter(out unusedDouble, out unusedDouble);
            }
            catch (HalconException)
            {
                this.ErrorMessage = "Halcon Exception in FillAndShrinkRegion";
                this.ErrorCode = ProcessingErrorCode.ProcessingStep2Error;
            }
            catch (Exception)
            {
                this.ErrorMessage = "System Exception in FillAndShrinkRegion";
                this.ErrorCode = ProcessingErrorCode.ProcessingStep2Error;
            }
            finally
            {
                circleRegion.Dispose();
            }

            return area;
        }

        /// <summary>
        /// Sets the dilation size according to the selected enumeration.
        /// </summary>
        /// <param name="selection">The selected enumeration.</param>
        private void SelectedOptionToDilationSize(RadioButtonSelection selection)
        {
            switch (selection)
            {
                case RadioButtonSelection.Large:
                    this.dilationSize = 400.5;
                    break;
                case RadioButtonSelection.Medium:
                    this.dilationSize = 100.5;
                    break;
                case RadioButtonSelection.NoErosion:
                    this.dilationSize = 0.0;
                    break;
            }
        }

        #endregion
    }
}
