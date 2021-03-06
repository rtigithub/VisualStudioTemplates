﻿//-----------------------------------------------------------------------
// <copyright file="MainViewModelBase.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.ViewModels
{
    using System;
    using System.Data;
    using System.Reactive.Linq;
    using ReactiveUI;

    /// <summary>
    /// MainViewModelBase is the base class for MainViewModel. 
    /// </summary>
    public abstract class MainViewModelBase : ReactiveObject, IDisposable, ViewModels.IMainViewModel
    {
        #region Private Fields

        /// <summary>
        /// A boolean value indicating that the class is disposed. 
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Stores the application state. For demonstration only. To be modified. 
        /// </summary>
        private int appState = 0;

        /// <summary>
        /// Stores the data set for the processing results. 
        /// </summary>
        private DataSet processingResultsDataSet;

        /// <summary>
        /// Stores the status text.
        /// </summary>
        private string statusText = "Status: No Errors";

        /// <summary>
        /// Stores the CompositeDisposable that holds all subscription disposables. 
        /// </summary>                
        private System.Reactive.Disposables.CompositeDisposable disposeCollection =
            new System.Reactive.Disposables.CompositeDisposable();

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the MainViewModelBase class.
        /// </summary>
        public MainViewModelBase()
        {
            this.MenuItems = new ReactiveList<MenuItemVM>();
            this.MenuItems.ChangeTrackingEnabled = true;

            this.DisposeCollection.Add(this.WhenAny(x => x.AppState, x => x.Value)
                .ObserveOnDispatcher()
                .Subscribe(_ => this.SetMenuItems()));
        }

        #endregion Public Constructors

        #region Private Destructors

        /// <summary>
        /// Finalizes an instance of the MainViewModelBase class.
        /// </summary>
        ~MainViewModelBase()
        {
            this.Dispose(false);
        }

        #endregion Private Destructors

        #region Public Properties

        /// <summary>
        /// Gets the disposeCollection. 
        /// </summary>
        public System.Reactive.Disposables.CompositeDisposable DisposeCollection
        {
            get
            {
                return this.disposeCollection;
            }
        }

        /// <summary>
        /// Gets or sets the application state. 
        /// </summary>
        public int AppState
        {
            get
            {
                return this.appState;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.appState, value);
            }
        }

        /// <summary>
        /// Gets or sets the processing results DataSet. 
        /// </summary>
        public DataSet ProcessingResultsDataSet
        {
            get
            {
                return this.processingResultsDataSet;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.processingResultsDataSet, value);
            }
        }

        /// <summary>
        /// Gets or sets the status text. 
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.statusText, value);
            }
        }

        /// <summary>
        /// Gets or sets the MenuItems reactive list. 
        /// </summary>
        public ReactiveList<MenuItemVM> MenuItems
        {
            get;

            set;
        }

        #endregion Public Properties

        #region Protected Properties
                
        #endregion Protected Properties

        #region Public Methods

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
                    // Code to dispose the managed resources
                    // held by the class.
                    if (this.disposeCollection != null)
                    {
                        this.disposeCollection.Dispose();
                    }

                    if (this.processingResultsDataSet != null)
                    {
                        this.processingResultsDataSet.Dispose();
                    }
                }
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Sets up the data set for output. This is only for example and should be changed as needed. 
        /// </summary>
        protected virtual void SetUpDataSet()
        {
            this.processingResultsDataSet = new DataSet();

            DataTable resultsDataTable = new DataTable();

            //// Add fields here. 

            this.processingResultsDataSet.Tables.Add(resultsDataTable);
        }

        /// <summary>
        /// Resets the MenuItems according to the state of the application. 
        /// </summary>
        protected virtual void SetMenuItems()
        {
            this.MenuItems.Clear();

            switch (this.appState)
            {
                default: break;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        #endregion Private Methods
    }
}
