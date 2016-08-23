//-----------------------------------------------------------------------
// <copyright file="ChangeColorProcessor.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.Model
{
    using System;
    using HalconDotNet;
    using ReactiveUI;

    /// <summary>
    /// Processor class for changing the display color and redisplaying overlays.  
    /// </summary>
    public class ChangeColorProcessor : ProcessorBase
    {
        #region Private Declarations

        /// <summary>
        /// Stores the string for passing a green color. For demonstration only. To be modified. 
        /// </summary>
        private readonly string green = "green";

        /// <summary>
        /// Stores the string for passing a red color. For demonstration only. To be modified. 
        /// </summary>
        private readonly string red = "red";

        /// <summary>
        /// Stores the current display color string. For demonstration only. To be modified. 
        /// </summary>
        private string currentDisplayColor;

        #endregion Private Declarations

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ChangeColorProcessor class.
        /// </summary>
        public ChangeColorProcessor()
            : base()
        {
            this.currentDisplayColor = this.green;
        }

        #endregion Constructors
        
        #region Events

        #endregion Events

        #region Enumerations

        #endregion Enumerations

        #region Properties

        /// <summary>
        /// Gets or sets the current display color.
        /// </summary>
        public string CurrentDisplayColor
        {
            get 
            { 
                return this.currentDisplayColor; 
            }

            set 
            { 
                this.currentDisplayColor = value; 
            }
        }

        #endregion Properties

        #region public Methods

        /// <summary>
        /// Calls the change color methods.
        /// </summary>
        /// <returns>A ProcessingResult instance. </returns>
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
                    this.ChangeDisplayColor();
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

            return result;
        }

        #endregion public Methods

        #region internal methods

        #endregion internal methods

        #region Protected Methods

        /// <summary>
        /// Overrides the Dispose method of IDisposable that actually disposes of managed resources. 
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the class is being disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    //// Dispose of managed resorces here. 
                }

                //// Dispose of unmanaged resorces here.

                this.IsDisposed = true;
            }

            // Call base.Dispose, passing parameter. 
            base.Dispose(disposing);
        }

        #endregion Protected Methods

        #region private methods

        /// <summary>
        /// Changes the display color. 
        /// </summary>
        private void ChangeDisplayColor()
        {
            if (this.currentDisplayColor == this.green)
            {
                this.currentDisplayColor = this.red;
            }
            else
            {
                this.currentDisplayColor = this.green;
            }
        }

        #endregion private methods
    }
}
