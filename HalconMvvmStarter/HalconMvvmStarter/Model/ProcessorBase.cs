//-----------------------------------------------------------------------
// <copyright file="ProcessorBase.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.Model
{
    using System;
    using ReactiveUI;
    using Rti.DisplayUtilities;

    /// <summary>
    /// Base class for specific process models. 
    /// </summary>
    public abstract class ProcessorBase : ReactiveObject, IDisposable, IProcessor
    {
        #region private fields

        /// <summary>
        /// Stores the display collection for any debug output. 
        /// </summary>
        private DisplayCollection debugDisplay = new DisplayCollection();

        /// <summary>
        /// Stores the CompositeDisposable that holds all subscription disposables. 
        /// </summary>                
        private System.Reactive.Disposables.CompositeDisposable disposeCollection =
            new System.Reactive.Disposables.CompositeDisposable();

        /// <summary>
        /// Stores the error message. 
        /// </summary>
        private string errorMessage = "No Errors";

        /// <summary>
        /// Stores the error code. 
        /// </summary>
        private ProcessingErrorCode errorCode = ProcessingErrorCode.NoError;

        /// <summary>
        /// Stores a value indicating whether the class has been disposed. 
        /// </summary>
        private bool isDisposed = false; 

        #endregion private fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ProcessorBase class. 
        /// </summary>
        public ProcessorBase()
        {            
        }

        #endregion Constructors

        #region Destructors

        /// <summary>
        /// Finalizes an instance of the ProcessorBase class.
        /// </summary>
        ~ProcessorBase()
        {
            this.Dispose(false);
        }

        #endregion Destructors

        #region Properties

        /// <summary>
        /// Gets or sets the display collection for any debug output. 
        /// </summary>
        public DisplayCollection DebugDisplay
        {
            get
            {
                return this.debugDisplay;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.debugDisplay, value);
            }
        }

        /// <summary>
        /// Gets or sets the error message. 
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.errorMessage, value);
            }
        }

        /// <summary>
        /// Gets or sets the error code. 
        /// </summary>
        public ProcessingErrorCode ErrorCode
        {
            get
            {
                return this.errorCode;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.errorCode, value);
            }
        }

        /// <summary>
        /// Gets or sets the CompositeDisposable object. 
        /// </summary>
        public System.Reactive.Disposables.CompositeDisposable DisposeCollection
        {
            get
            {
                return this.disposeCollection;
            }

            set
            {
                this.disposeCollection = value;
            }
        }

        #endregion Properties

        #region public methods

        #region IDisposable Members

        /// <summary>
        /// Implements the Dispose method of IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members

        /// <summary>
        /// Performs the process for this class.
        /// </summary>
        /// <returns>A structure containing the processing results and error information.</returns>
        public virtual ProcessingResult Process()
        {
            this.ErrorCode = ProcessingErrorCode.NoError;
            this.ErrorMessage = "No errors detected.";
            ProcessingResult result = new ProcessingResult();

            try
            {
                if (this.errorCode == ProcessingErrorCode.NoError)
                {
                    // Call sub methods here. 
                    if (this.errorCode == ProcessingErrorCode.NoError)
                    {
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

            return result;
        }

        #endregion public methods

        #region Protected Methods

        /// <summary>
        /// Implements the Dispose method of IDisposable that actually disposes of managed resources. 
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the class is being disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // Code to dispose the managed resources
                    // held by the class                    
                    if (this.disposeCollection != null)
                    {
                        this.disposeCollection.Dispose();
                    }
                }

                if (this.DebugDisplay != null)
                {
                    this.DebugDisplay.Dispose();
                }
            }

            this.isDisposed = true;
        }

        #endregion Protected Methods

        #region Private Methods

        #endregion Private Methods
    }
}
