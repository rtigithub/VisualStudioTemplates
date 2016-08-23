//-----------------------------------------------------------------------
// <copyright file="ProcessViewModelBase.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Model;
    using ReactiveUI;
    using Rti.DisplayUtilities;

    /// <summary>
    /// ProcessViewModelBase is the base class for chile ViewModel classes. 
    /// </summary>
    /// <typeparam name="MainVMClass">Class type of the main view model class.</typeparam>
    /// <typeparam name="ProcessorModelClass">Class type of the processor class.</typeparam>
    public abstract class ProcessViewModelBase<MainVMClass, ProcessorModelClass> : ReactiveObject, IDisposable
        where MainVMClass : IMainViewModel
        where ProcessorModelClass : IProcessor
    {
        #region Protected Fields

        /// <summary>
        /// Stores the boolean returned from Command.ToProperty call. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
            Justification = "ReactiveUI requires an out field in ToProperty. Making this Protected avoids " +
                            "reproducing the storage in every drived class.")]
        protected ObservableAsPropertyHelper<bool> isLoading;

        #endregion Protected Fields

        #region Private Fields
                
        /// <summary>
        /// Stores the CompositeDisposable that holds all subscription disposables. 
        /// </summary>                
        private System.Reactive.Disposables.CompositeDisposable disposeCollection =
            new System.Reactive.Disposables.CompositeDisposable();

        /// <summary>
        /// Stores the display collection for the loaded image.
        /// </summary>
        private DisplayCollection display = new DisplayCollection();

        /// <summary>
        /// Stores the display collection for any debug output. 
        /// </summary>
        private DisplayCollection debugDisplay = new DisplayCollection();

        /// <summary>
        /// Stores a value indicating whether the class has been disposed. 
        /// </summary>
        private bool isDisposed = false;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the ProcessViewModelBase class in the derived class. 
        /// </summary>
        /// <param name="mainViewModel">A reference to the main view model that owns the derived class.</param>
        /// <param name="processor">Processor class for this process.</param>    
        public ProcessViewModelBase(IMainViewModel mainViewModel, IProcessor processor)
        {
            this.MainViewModelRef = (MainVMClass)mainViewModel;

            this.Processor = (ProcessorModelClass)processor;

            this.Command = ReactiveCommand.CreateAsyncTask(_ => this.ProcessAsync());            
        }

        #endregion Public Constructors

        #region Private Destructors

        /// <summary>
        /// Finalizes an instance of the ProcessViewModelBase class.
        /// </summary>
        ~ProcessViewModelBase()
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
        /// Gets of sets a reference to the MainViewModel.
        /// </summary>
        public MainVMClass MainViewModelRef
        {
            get;

            private set;
        }

        /// <summary>
        /// Gets a dynamic reference to the MainViewModel.
        /// </summary>
        public dynamic MainViewModelRefDynamic
        {
            get
            {
                return this.MainViewModelRef;
            }
        }

        /// <summary>
        /// Gets of sets a reference to the processor model for this class.
        /// </summary>
        public ProcessorModelClass Processor
        {
            get;

            private set;
        }

        /// <summary>
        /// Gets or sets an observable that indicates whether the command can execute. 
        /// </summary>
        public IObservable<bool> CanExecute
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the command for processing an image. 
        /// </summary>
        public ReactiveCommand<ProcessingResult> Command
        {
            get;

            protected set;
        }

        /// <summary>
        /// Gets a value indicating whether the LoadImageCommand is executing. 
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading.Value;
            }
        }

        /// <summary>
        /// Gets or sets the image display. 
        /// </summary>
        public DisplayCollection Display
        {
            get
            {
                return this.display;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.display, value);
            }
        }

        /// <summary>
        /// Gets or sets the debug display. 
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

        #endregion Public Properties

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

        /// <summary>
        /// Sets the ImageDisplay to a new value, triggering property changed. 
        /// </summary>
        public void SetDisplay()
        {
            this.Display.Dispose();
            this.Display = this.BuildDisplayItem();
        }

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

                    if (this.display != null)
                    {
                        this.display.Dispose();
                    }

                    if (this.debugDisplay != null)
                    {
                        this.debugDisplay.Dispose();
                    }
                }
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Creates a new display collection containing the loaded image. 
        /// </summary>
        /// <returns>The DisplayCollection created.</returns>
        protected virtual DisplayCollection BuildDisplayItem()
        {
            DisplayCollection tempDC = new DisplayCollection();
            tempDC.ClearDisplayFirst = true;

            //// Add display elements here. 

            return tempDC;
        }

        /// <summary>
        /// Implements the asynchronous process method for this process. 
        /// </summary>
        /// <returns>A ProcessingResult instance.</returns>
        protected virtual async Task<ProcessingResult> ProcessAsync()
        {
            ////return await Task.Factory.StartNew(() => new ProcessingResult());
            return await Task.Factory.StartNew(() => this.Processor.Process());
        }

        #endregion Protected Methods

        #region Private Methods

        #endregion Private Methods
    }
}
