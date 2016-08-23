//-----------------------------------------------------------------------
// <copyright file="ProcessingResult.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// This class encapsulates the results from a generic process. 
    /// </summary>
    public class ProcessingResult : ProcessingResultsBase
    {
        #region Private Fields
                
        /// <summary>
        /// Stores a dictionary of results. 
        /// </summary>
        private Dictionary<string, object> resultsCollection = new Dictionary<string, object>();

        /// <summary>
        /// Stores a value indicating whether the class has been disposed. 
        /// </summary>
        private bool isDisposed = false;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the ProcessingResult class.
        /// </summary>
        public ProcessingResult()
        {
        }

        #endregion Public Constructors

        #region Private Destructors
                
        #endregion Private Destructors

        #region Public Properties

        /// <summary>
        /// Gets or sets a dictionary of results. 
        /// </summary>
        public override Dictionary<string, object> ResultsCollection
        {
            get
            {
                return this.resultsCollection;
            }

            set
            {
                this.resultsCollection = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

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

                this.isDisposed = true;
            }

            // Call base.Dispose, passing parameter. 
            base.Dispose(disposing);
        }

        #endregion Protected Methods
    }
}
