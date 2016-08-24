//-----------------------------------------------------------------------
// <copyright file="Enums.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName",
    Justification = "Allow the name of the file to be used.")]

namespace HalconMVVMStarter.Model
{
    /// <summary>
    /// Enumeration of error codes.
    /// </summary>
    public enum ProcessingErrorCode
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        NoError,

        /// <summary>
        /// Example error code. To be changed or removed.
        /// </summary>
        ProcessingStep1Error,

        /// <summary>
        /// Example error code. To be changed or removed.
        /// </summary>
        ProcessingStep2Error,

        /// <summary>
        /// An undefined error occurred. 
        /// </summary>
        UndefinedError
    }

    /// <summary>
    /// Enumeration of radio button options.
    /// </summary>
    public enum RadioButtonSelection
    {
        /// <summary>
        /// No selection. This is used to toggle the selection so that a resets can be done regardless of the current setting.
        /// </summary>
        Large,

        /// <summary>
        /// A Medium erosion.
        /// </summary>
        Medium,

        /// <summary>
        /// No erosion.
        /// </summary>
        NoErosion,
        None
    }
}