//-----------------------------------------------------------------------
// <copyright file="DemoProcessingResult.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.Model
{
    using System.Collections.Generic;

    public class DemoProcessingResult : ProcessingResultsBase
    {
        /// <summary>
        /// Stores the area result.
        /// </summary>
        private double area = 0;

        /// <summary>
        /// A boolean value indicating that the class is disposed.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Stores a dictionary of results. 
        /// </summary>
        private Dictionary<string, object> resultsCollection = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the ProcessingResult class.
        /// </summary>
        public DemoProcessingResult()
        {
        }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        public double Area
        {
            get { return this.area; }
            set { this.area = value; }
        }

        /// <summary>
        /// Gets or sets a dictionary of results. 
        /// </summary>
        public override Dictionary<string, object> ResultsCollection
        {
            get { return this.resultsCollection; }
            set { this.resultsCollection = value; }
        }

        /// <summary>
        /// Implements the Dispose method of IDisposable that actually disposes of managed resources.
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the class is being disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // Code to dispose the managed resources
                    // held by the class
                }

                // Code to dispose the unmanaged resources
                // held by the class
            }

            this.isDisposed = true;
        }
    }
}
