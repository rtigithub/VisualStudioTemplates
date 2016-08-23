//-----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.ViewModels
{
    using System;
    using System.Data;
    using System.Reactive.Linq;
    using Model;
    using ReactiveUI;

    /// <summary>
    /// Main view model that contains all the others and provides interconnectivity. 
    /// </summary>
    public class MainViewModel : MainViewModelBase
    {
        #region Private Declarations

        /// <summary>
        /// Stores a value indicating whether the class has been disposed. 
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// The child view model for loading images.
        /// </summary>
        private LoadProcessViewModel loadImageVM;

        //// Add additional view model fields for each view model owned by the main view model class.

        #endregion Private Declarations

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.SetUpDataSet();

            this.loadImageVM = new LoadProcessViewModel(this, new LoadImageProcessor());

            //// Instatiate additional view models by passing "this" and an instance of the processor class model for the view model.
                        
            this.DisposeCollection.Add(this.WhenAnyValue(x => x.AppState)
                .Where(x => x != 0)
                .StartWith(1)
                .Subscribe(x => this.LastAppState = x));
        }

        #endregion Constructors

        #region Destructors

        #endregion Destructors

        #region Events

        #endregion Events

        #region Enumerations

        #endregion Enumerations

        #region Properties

        /// <summary>
        /// Gets or sets the last application state. 
        /// </summary>
        public int LastAppState
        {
            get;

            set;
        }

        /// <summary>
        /// Gets the loadImageVM. 
        /// </summary>
        public LoadProcessViewModel LoadImageVM
        {
            get
            {
                return this.loadImageVM;
            }
        }

        //// Add additional view model properties for each view model owned by the main view model class.

        #endregion Properties

        #region public Methods

        #endregion public Methods

        #region internal methods

        #endregion internal methods

        #region Protected Methods

        /// <summary>
        /// Sets up the data set for output. 
        /// </summary>
        protected sealed override void SetUpDataSet()
        {
            this.ProcessingResultsDataSet = new DataSet();

            DataTable resultsDataTable = new DataTable();

            //// Add fields here.
            //// Template: resultsDataTable.Columns.Add("My Column", Type.GetType("System.<the type>"));

            this.ProcessingResultsDataSet.Tables.Add(resultsDataTable);
        }

        /// <summary>
        /// Resets the MenuItems according to the state of the application. 
        /// </summary>
        protected override void SetMenuItems()
        {
            this.MenuItems.Clear();

            // Add menu items with display text and a reactive command to execute for the appropriate AppState.
            // Command must be of type ReactiveCommand<ProcessingResult>. 
            ////this.MenuItems.Add(new MenuItemVM("Menu Text.", this.SomeProcessorViewModel.Command));

            switch (this.AppState)
            {
                case 0:
                    break;
                case 1:
                    break;
            }
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
                if (this.loadImageVM != null)
                {
                    this.loadImageVM.Dispose();
                }

                this.isDisposed = true;
            }

            // Call base.Dispose, passing parameter. 
            base.Dispose(disposing);
        }

        #endregion Protected Methods

        #region private methods

        #endregion private methods
    }
}
