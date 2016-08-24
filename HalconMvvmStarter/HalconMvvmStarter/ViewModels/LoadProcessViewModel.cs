//-----------------------------------------------------------------------
// <copyright file="LoadProcessViewModel.cs" company="Resolution Technology, Inc.">
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
    using Rti.DisplayUtilities;

    /// <summary>
    /// LoadProcessViewModel class for image load processing.
    /// </summary>
    public class LoadProcessViewModel : ProcessViewModelBase<DemoMainViewModel, LoadImageProcessor>
    {
        #region Private Declarations

        /// <summary>
        /// Stores the file name string. 
        /// </summary>
        private string fileName = "No File Loaded";

        /// <summary>
        /// Stores the image width.
        /// </summary>
        private ObservableAsPropertyHelper<int> imageWidth;

        /// <summary>
        /// Stores the image height.
        /// </summary>
        private ObservableAsPropertyHelper<int> imageHeight;

        /// <summary>
        /// Stores a value indicating whether the class has been disposed. 
        /// </summary>
        private bool isDisposed = false;

        #endregion Private Declarations

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LoadProcessViewModel class. 
        /// </summary>
        /// <param name="mainVM">A reference to the main view model that owns this class.</param>
        /// <param name="processor">Processor class for this process.</param>    
        public LoadProcessViewModel(IMainViewModel mainVM, IProcessor processor)
            : base(mainVM, processor)
        {
            this.CanExecute = this.WhenAny(x => x.FileName, x => (x.Value != string.Empty) && (x.Value != null));

            // The Command must be recreated with the new CanExecute observable for it to be used. 
            this.Command = ReactiveCommand.CreateAsyncTask(this.CanExecute, _ => this.ProcessAsync());

            // add this and the property and field if it is needed. 
            this.Command.IsExecuting
               .ToProperty(this, x => x.IsLoading, out this.isLoading);

            // set the file name in processor and then execute command. 
            this.DisposeCollection.Add(this.WhenAnyValue(x => x.FileName)
                .Where(x => x != "No File Loaded")
                .Subscribe(x =>
                    {
                        this.Processor.FileName = x;
                        this.Command.Execute(null);
                    }));

            this.DisposeCollection.Add(
                this.WhenAnyValue(x => x.Processor.DebugDisplay)
                .Where(x => x.DisplayList.Count > 0)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(x =>
                {
                    this.DebugDisplay.Dispose();
                    this.DebugDisplay = x;
                }));

            this.DisposeCollection.Add(this.WhenAny(x => x.Processor.Image, x => x.Value)
                .Where(x => x != null)
                .Where(x => x.IsInitialized())
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(_ =>
                {
                    this.SetDisplay();
                    this.MainViewModelRef.AppState = 0;
                }));

            this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ImageHeight)
                .StartWith(0)
                .ToProperty(this, x => x.ImageHeight, out this.imageHeight)); 

            this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ImageWidth)                
                .StartWith(0)
                .ToProperty(this, x => x.ImageWidth, out this.imageWidth));

            this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ErrorMessage)
                .Subscribe(x => this.MainViewModelRef.StatusText = x));
        }

        #endregion

        #region Destructors

        #endregion Destructors

        #region Properties

        /// <summary>
        /// Gets or sets the file name. 
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.fileName, value);
            }
        }

        /// <summary>
        /// Gets the image width. 
        /// </summary>
        public int ImageWidth
        {
            get
            {
                return this.imageWidth.Value;
            }
        }

        /// <summary>
        /// Gets the image height. 
        /// </summary>
        public int ImageHeight
        {
            get
            {
                return this.imageHeight.Value;
            }
        }

        /// <summary>
        /// Gets  the image. 
        /// </summary>
        public HImage Image
        {
            get
            {
                return this.Processor.Image;
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
            tempDC.AddDisplayObject(this.Processor.Image.CopyObj(1, -1));

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
