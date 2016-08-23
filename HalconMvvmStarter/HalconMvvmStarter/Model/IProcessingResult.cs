﻿//-----------------------------------------------------------------------
// <copyright file="IProcessingResult.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for ProcessingResult
    /// </summary>
    public interface IProcessingResult
    {
        /// <summary>
        /// Gets or sets the error message. 
        /// </summary>
        string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error code. 
        /// </summary>
        ProcessingErrorCode StatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a dictionary of results. 
        /// </summary>
        Dictionary<string, object> ResultsCollection
        {
            get;
            set;
        }

        /// <summary>
        /// Implements the Dispose method of IDisposable.
        /// </summary>
        void Dispose();
    }
}
