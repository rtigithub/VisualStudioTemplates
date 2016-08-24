//-----------------------------------------------------------------------
// <copyright file="ChangeColorProcessViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.ViewModels
{
    using System;
    using Model;
    using ReactiveUI;
    using Rti.DisplayUtilities;
    using Rti.ViewRoiCore;

    /// <summary>
    /// ChangeColorProcessViewModel class for change color processing. 
    /// </summary>
    public class ChangeColorProcessViewModel : ProcessViewModelBase<DemoMainViewModel, ChangeColorProcessor>
    {
        #region Private Declarations

        /// <summary>
        /// Stores the ProcessingResult returned from ProcessAsync.ToProperty call. 
        /// </summary>
        private ObservableAsPropertyHelper<ProcessingResult> processingResults;

        /// <summary>
        /// A boolean value indicating that the class is disposed. 
        /// </summary>
        private bool isDisposed = false;
        #endregion Private Declarations

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ChangeColorProcessViewModel class. 
        /// </summary>
        /// <param name="mainVM">A reference to the main view model that owns this class.</param>
        /// <param name="processor">Processor class for this process.</param>
        public ChangeColorProcessViewModel(IMainViewModel mainVM, IProcessor processor)
            : base(mainVM, processor)
        {
            this.Command.ToProperty(this, x => x.ProcessingResults, out this.processingResults);

            this.DisposeCollection.Add(this.Command
                .Subscribe(_ =>
                {
                    if (this.MainViewModelRef.AppState == 1)
                    {
                        this.MainViewModelRef.AppState = 2;
                    }
                    else if (this.MainViewModelRef.AppState == 2)
                    {
                        this.MainViewModelRef.AppState = 1;
                    }

                    this.SetDisplay();
                }));

            this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ErrorMessage)
                .Subscribe(x => this.MainViewModelRef.StatusText = x));
        }

        #endregion Constructors

        #region Destructors
                
        #endregion Destructors

        #region Properties

        /// <summary>
        /// Gets the current display color.
        /// </summary>
        public HalconColors CurrentDisplayColor
        {
            get 
            { 
                return this.Processor.CurrentDisplayColor; 
            }
        }

        /// <summary>
        /// Gets the processing results. 
        /// </summary>
        public ProcessingResult ProcessingResults
        {
            get 
            { 
                return this.processingResults.Value; 
            }
        }

        #endregion Properties

        #region public methods

        #endregion public methods

        #region Protected Methods

        /// <summary>
        /// Builds a DisplayCollection. 
        /// </summary>
        /// <returns>the DisplayCollection.</returns>
        protected override DisplayCollection BuildDisplayItem()
        {
            DisplayCollection tempDC = new DisplayCollection();
            tempDC.ClearDisplayFirst = true;

            tempDC.AddDisplayObject(this.MainViewModelRef.LoadImageVM.Image.CopyObj(1, -1));
            tempDC.AddDisplayObject(
                this.MainViewModelRef.BoundaryProcessVM.WaferRegion.CopyObj(1, -1),
                this.CurrentDisplayColor,
                1,
                DrawModes.Margin);  //"margin");

            return tempDC;
        }

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

        #region Private Methods

        #endregion Private Methods
    }
}
