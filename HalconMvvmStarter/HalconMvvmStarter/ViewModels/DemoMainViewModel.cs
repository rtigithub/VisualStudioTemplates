//-----------------------------------------------------------------------
// <copyright file="DemoMainViewModel.cs" company="Resolution Technology, Inc.">
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
    /// DemoMainViewModel class is a Demo MainViewModel. 
    /// </summary>
    public sealed class DemoMainViewModel : MainViewModelBase
    {
        #region Private Declarations

        /// <summary>
        /// The child view model for loading images.
        /// </summary>
        private LoadProcessViewModel loadImageVM;

        //// Add additional view model fields for each view model owned by the main view model class.

        /// <summary>
        /// The child view model for processing boundaries.
        /// </summary>
        private BoundaryProcessViewModel boundaryProcessVM;

        /// <summary>
        /// The child view model for changing the display color.
        /// </summary>
        private ChangeColorProcessViewModel changeColorVM;

        /// <summary>
        /// A boolean value indicating that the class is disposed. 
        /// </summary>
        private bool isDisposed = false;

        #endregion Private Declarations

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DemoMainViewModel class.
        /// </summary>
        public DemoMainViewModel()
        {
            this.SetUpDataSet();

            this.loadImageVM = new LoadProcessViewModel(this, new LoadImageProcessor());

            //// Instatiate additional view models by passing "this" and an instance of the processor class model for the view model.
            this.boundaryProcessVM = new BoundaryProcessViewModel(this, new BoundaryProcessor());

            this.changeColorVM = new ChangeColorProcessViewModel(this, new ChangeColorProcessor());
                        

            this.DisposeCollection.Add(this.WhenAnyValue(x => x.AppState)
                .Where(x => x != 0)
                .StartWith(1)
                .Subscribe(x => this.LastAppState = x));
        }

        #endregion Constructors

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

        /// <summary>
        /// Gets the boundaryProcessVM. 
        /// </summary>
        public BoundaryProcessViewModel BoundaryProcessVM
        {
            get 
            {
               return this.boundaryProcessVM; 
            }
        }

        /// <summary>
        /// Gets the changeColorVM. 
        /// </summary>
        public ChangeColorProcessViewModel ChangeColorProcessVM
        {
            get 
            {
               return this.changeColorVM; 
            }
        }

        #endregion Properties

        #region public Methods

        #endregion public Methods

        #region internal methods

        #endregion internal methods

        #region Protected Methods

        /// <summary>
        /// Sets up the data set for output. This is only for example and should be changed as needed. 
        /// </summary>
        protected sealed override void SetUpDataSet()
        {
            this.ProcessingResultsDataSet = new DataSet();

            DataTable resultsDataTable = new DataTable();

            //// Add fields here.
            resultsDataTable.Columns.Add("Item", Type.GetType("System.Int32"));
            resultsDataTable.Columns.Add("Area", Type.GetType("System.Double"));

            this.ProcessingResultsDataSet.Tables.Add(resultsDataTable);
        }

        /// <summary>
        /// Resets the MenuItems according to the state of the application. 
        /// </summary>
        protected override void SetMenuItems()
        {
            this.MenuItems.Clear();

            //// Add menu items with display text and a reactive command to execute for the appropriate AppState.
            //// Command must be of type ReactiveCommand<ProcessingResult>. 

            switch (this.AppState)
            {
                case 0:
                    break;
                case 1:
                    this.MenuItems.Add(new MenuItemVM("Display in red.", this.changeColorVM.Command));
                    break;
                case 2:
                    this.MenuItems.Add(new MenuItemVM("Display in green.", this.changeColorVM.Command));
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
                    if (this.changeColorVM != null)
                    {
                        this.changeColorVM.Dispose();
                    }

                    if (this.loadImageVM != null)
                    {
                        this.loadImageVM.Dispose();
                    }

                    if (this.boundaryProcessVM != null)
                    {
                        this.boundaryProcessVM.Dispose();
                    }
                }

                //// Dispose of unmanaged resorces here.

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
