//-----------------------------------------------------------------------
// <copyright file="BoundaryProcessViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter.ViewModels
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;    
    using HalconDotNet;
    using Model;
    using ReactiveUI;
    using Rti.ViewRoiCore; 
    using Rti.DisplayUtilities;

    /// <summary>
    /// ProcessorViewModel class for border processing. Demo. 
    /// </summary>
    public class BoundaryProcessViewModel : ProcessViewModelBase<DemoMainViewModel, BoundaryProcessor>
    {
        #region Private Declarations

        /// <summary>
        /// Stores the ProcessingResult returned from ProcessAsync.ToProperty call. 
        /// </summary>
        private ObservableAsPropertyHelper<ProcessingResult> processingResults;

        /// <summary>
        /// Stores an item count for the number of objects processed. 
        /// This is only for demonstrating output capabilities. 
        /// </summary>
        private int itemCount = 0;

        /// <summary>
        /// A boolean value indicating that the class is disposed. 
        /// </summary>
        private bool isDisposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BoundaryProcessViewModel class. 
        /// </summary>
        /// <param name="mainVM">A reference to the main view model that owns this class.</param>
        /// <param name="processor">Processor class for this process.</param>
        public BoundaryProcessViewModel(IMainViewModel mainVM, IProcessor processor)
            : base(mainVM, processor)
        {
            // tie to loading state.             
            this.CanExecute = this.WhenAny(x => x.MainViewModelRef.LoadImageVM.IsLoading, x => x.Value == false);

            // The Command must be recreated with the new CanExecute observable for it to be used. 
            this.Command = ReactiveCommand.CreateAsyncTask(this.CanExecute, _ => this.ProcessAsync());

            this.Command.ToProperty(this, x => x.ProcessingResults, out this.processingResults);

            this.DisposeCollection.Add(
                this.WhenAnyValue(x => x.Processor.DebugDisplay)
                .Where(x => x.DisplayList.Count > 0)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(x =>
                {
                    this.DebugDisplay.Dispose();
                    this.DebugDisplay = x;
                }));

            this.DisposeCollection.Add(
                this.WhenAnyValue(x => x.Processor.WaferRegion)
                .Where(x => x != null)
                .Where(x => x.IsInitialized())
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(_ =>
                {
                    this.SetDisplay();
                }));

            this.DisposeCollection.Add(this.Command
                .Subscribe(_ =>
                    {
                        this.MainViewModelRef.AppState = this.MainViewModelRef.AppState == 0 ? this.MainViewModelRef.LastAppState : this.MainViewModelRef.AppState;
                    }));

            this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ErrorMessage)
                .Subscribe(x => this.MainViewModelRef.StatusText = x));

            this.DisposeCollection.Add(
                this.WhenAnyValue(x => x.ProcessingResults)
                .Where(x => x != null)
                .Where(x => x.ResultsCollection.ContainsKey("Area"))
                .Subscribe(x => this.MainViewModelRef.ProcessingResultsDataSet.Tables[0].Rows.Add(++this.itemCount, x.ResultsCollection["Area"])));
        }

        #endregion

        #region Destructors

        #endregion

        #region Properties

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

        /// <summary>
        /// Gets the test region. 
        /// </summary>
        public HRegion WaferRegion
        {
            get 
            { 
                return this.Processor.WaferRegion; 
            }
        }

        /// <summary>
        /// Gets or sets the selected option. 
        /// </summary>
        public RadioButtonSelection SelectedOption
        {
            get
            {
                return this.Processor.SelectedOption;
            }

            set
            {                
                this.Processor.SelectedOption = value;
            }
        }

        #endregion

        #region public methods

        #endregion

        #region Protected Methods

        /// <summary>
        /// Implements the asynchronous process method for this process. 
        /// </summary>
        /// <returns>A ProcessingResult instance.</returns>
        protected override async Task<ProcessingResult> ProcessAsync()
        {
            this.Processor.Image = this.MainViewModelRef.LoadImageVM.Image;
            return await base.ProcessAsync();
        }

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
                this.Processor.WaferRegion.CopyObj(1, -1),
                this.MainViewModelRef.ChangeColorProcessVM.CurrentDisplayColor,
                1,
                DrawModes.Margin);

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
                    this.processingResults.Dispose();
                }

                //// Dispose of unmanaged resorces here.

                this.isDisposed = true;
            }

            // Call base.Dispose, passing parameter. 
            base.Dispose(disposing);
        }

        #endregion Protected Methods

        #region Private Methods

        #endregion
    }
}
