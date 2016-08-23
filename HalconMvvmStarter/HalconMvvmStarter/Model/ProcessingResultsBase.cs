﻿//-----------------------------------------------------------------------
// <copyright file="ProcessingResultsBase.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for processing results. 
    /// </summary>
    public abstract class ProcessingResultsBase : IDisposable, IProcessingResult
    {
        #region Private Fields
               
        /// <summary>
        /// Error message associated with an exception thrown.
        /// </summary>
        private string errorMessage = "No Errors";

        /// <summary>
        /// An enumeration member indicating either success or the error encountered.
        /// </summary>
        private ProcessingErrorCode statusCode = ProcessingErrorCode.NoError;

        /// <summary>
        /// Stores a value indicating whether the class has been disposed. 
        /// </summary>
        private bool isDisposed = false;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the ProcessingResultsBase class.
        /// </summary>
        public ProcessingResultsBase()
        {
        }

        #endregion Public Constructors

        #region Private Destructors

        /// <summary>
        /// Finalizes an instance of the ProcessingResultsBase class.
        /// </summary>
        ~ProcessingResultsBase()
        {
            this.Dispose(false);
        }

        #endregion Private Destructors

        #region Public Properties

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
                this.errorMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the error code. 
        /// </summary>
        public ProcessingErrorCode StatusCode
        {
            get
            {
                return this.statusCode;
            }

            set
            {
                this.statusCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the ResultsCollection Dictionary.
        /// </summary>
        public abstract Dictionary<string, object> ResultsCollection
        {
            get;

            set;
        }        

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Implements the Dispose method of IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

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
                    //// Code to dispose the managed resources
                    //// held by the class.
                }
            }

            this.isDisposed = true;
        }

        #endregion Protected Methods
    }
}
