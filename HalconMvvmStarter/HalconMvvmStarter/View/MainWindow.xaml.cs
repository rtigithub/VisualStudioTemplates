//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace HalconMVVMStarter
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Xml;
    using Microsoft.Win32;
    using ReactiveUI;
    using Rti.ViewROIManager;

    using ViewModel = ViewModels.MainViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public sealed partial class MainWindow : Window, ReactiveUI.IViewFor<ViewModel>, IDisposable
    {
        #region Public static Fields

        /// <summary>
        /// The DependencyProperty for the ViewModel.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("MainViewModel", typeof(ViewModel), typeof(MainWindow), new PropertyMetadata(null));

        #endregion Public static Fields

        #region private fields

        /// <summary>
        /// Dialog for opening image files. 
        /// </summary>
        private OpenFileDialog openfileDlg = new OpenFileDialog();

        /// <summary>
        /// A boolean value indicating whether the class is disposed. 
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Stores the CompositeDisposable that holds all subscription disposables. 
        /// </summary>                
        private System.Reactive.Disposables.CompositeDisposable disposeCollection =
            new System.Reactive.Disposables.CompositeDisposable();

        /// <summary>
        /// Stores the instance of the ViewROIManager.
        /// </summary>
        private ViewROIManager viewROIManager = new ViewROIManager();

        /// <summary>
        /// Stores a value indicating whether ViewROIManager can react to zoom changes. 
        /// </summary>
        private bool canReactToZoom = true; 

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.MainViewModel = new ViewModel();
            this.DataContext = this.MainViewModel;

            // Uncomment this line to bind the Process button to the command of a 
            // new ProcessViewModel created in MainViewModel.cs.
            // Change "ProcessVM" to the name of the new ProcessViewModel.
            ////this.BindCommand(this.MainViewModel, vm => vm.ProcessVM.Command, x => x.ProcessButton);

            //// Note: To use radio buttons bound to an enumeration it is necessary to create the radio button group, 
            //// add new values to the RadioButtonSelection enumeration (in Model.Enums.cs), bind the property 
            //// of the Enum type in a view model, and to bind each radio button with a call to this.SetRadioButtonBinding.
            //// Template: this.SetRadioButtonBinding(this.Option1Button, this.MainViewModel.SelectChannelVM, "SelectedOptionProperty", "Option1");
                        
            this.dataGrid1.ItemsSource = this.MainViewModel.ProcessingResultsDataSet.Tables[0].DefaultView;

            this.disposeCollection.Add(this.Events().ContentRendered.Subscribe(_ =>
            {
                this.viewROIManager.LocHWindowContol = this.hWindowControlWPF1;

                this.viewROIManager.LocHWindowContol.ContextMenu.ItemsSource = this.MainViewModel.MenuItems;

                // Create and set the data template. 
                DataTemplate dataTemplate = this.CreateDataTemplate();
                this.viewROIManager.LocHWindowContol.ContextMenu.ItemTemplate = dataTemplate;

                // Create and set the ItemsPanelTemplate.
                ItemsPanelTemplate itemsPanelTemplate = this.CreateItemsPanelTemplate();
                this.viewROIManager.LocHWindowContol.ContextMenu.ItemsPanel = itemsPanelTemplate;
            }));

            this.disposeCollection.Add(this.Events().LayoutUpdated.InvokeCommand(this.viewROIManager.AdjustAspectCommand));

            this.disposeCollection.Add(Observable.FromEventPattern<SelectionChangedEventArgs>(this.comboboxZoom, "SelectionChanged")
                .Where(_ => this.canReactToZoom)
                .Select(ev => ev.EventArgs.AddedItems.Cast<object>().FirstOrDefault())
                .Select(s => (string)((System.Windows.Controls.ComboBoxItem)s).Content)
                .Select(v => GetZoomScaleFromString(v))
                .ObserveOn(RxApp.MainThreadScheduler)
                .BindTo(this.viewROIManager, vm => vm.ZoomScale));
                        
            // Using Subscribe here because BindTo sets up a two-way binding which is not wanted. 
            this.disposeCollection.Add(this.WhenAnyValue(x => x.ImageBorder.ActualHeight)
                .Subscribe(x => this.viewROIManager.ContainerHeight = x));
            this.disposeCollection.Add(this.WhenAnyValue(x => x.ImageBorder.ActualWidth)
                .Subscribe(x => this.viewROIManager.ContainerWidth = x));

            this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.LoadImageVM.ImageHeight)
               .Subscribe(x => this.viewROIManager.ImageHeight = x));
            this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.LoadImageVM.ImageWidth)
                .Subscribe(x => this.viewROIManager.ImageWidth = x));

            // Reset the zoom combo box to "Fit" for a new image, but prevent redisplay of the image. 
            this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.LoadImageVM.Display)
               .Where(x => x.DisplayList.Count > 0)
               .ObserveOn(RxApp.MainThreadScheduler)
               .Subscribe(_ =>
               {
                   this.canReactToZoom = false;
                   this.comboboxZoom.SelectedIndex = 10;
                   this.canReactToZoom = true;
               }));
            
            // Displays 
            this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.LoadImageVM.Display)
                .Where(x => x.DisplayList.Count > 0)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => this.viewROIManager.ShowDisplayCollection(x)));

            //// Add Subscriptions to display collections here by duplicating the above code and changing the ViewModel.

            // Debug Displays
            this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.LoadImageVM.DebugDisplay)
                .Where(x => x.DisplayList.Count > 0)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => this.viewROIManager.ShowDisplayCollection(x)));

            //// Add Subscriptions to debug display collections here by duplicating the above code and changing the ViewModel.

            this.disposeCollection.Add(this.MainViewModel.MenuItems.ItemsAdded
                .Where(_ => this.viewROIManager.LocHWindowContol != null)
                .Subscribe(
                 _ =>
                 {
                     this.viewROIManager.LocHWindowContol.ContextMenu.ItemsSource = this.MainViewModel.MenuItems;

                     // Create and set the data template. 
                     DataTemplate dataTemplate = this.CreateDataTemplate();
                     this.viewROIManager.LocHWindowContol.ContextMenu.ItemTemplate = dataTemplate;

                     // Create and set the ItemsPanelTemplate.
                     ItemsPanelTemplate itemsPanelTemplate = this.CreateItemsPanelTemplate();
                     this.viewROIManager.LocHWindowContol.ContextMenu.ItemsPanel = itemsPanelTemplate;
                 }));
        }

        #endregion Public Constructors

        #region Private Destructors

        /// <summary>
        /// Finalizes an instance of the MainWindow class.
        /// </summary>
        ~MainWindow()
        {
            this.Dispose(false);
        }

        #endregion Private Destructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the ViewModel through the ViewModelProperty.
        /// </summary>
        public ViewModel MainViewModel
        {
            get
            {
                return (ViewModel)GetValue(ViewModelProperty);
            }

            set
            {
                this.SetValue(ViewModelProperty, value);
            }
        }

        #endregion Public Properties

        #region Interface Member Properties

        /// <summary>
        /// Gets or sets the ViewModel as an object. Needed for RxUI binding. 
        /// </summary>
        object ReactiveUI.IViewFor.ViewModel
        {
            get
            {
                return this.MainViewModel;
            }

            set
            {
                this.MainViewModel = (ViewModel)value;
            }
        }

        /// <summary>
        /// Gets or sets the ViewModel. 
        /// </summary>
        ViewModel ReactiveUI.IViewFor<ViewModel>.ViewModel
        {
            get
            {
                return this.MainViewModel;
            }

            set
            {
                this.MainViewModel = value;
            }
        }

        #endregion Interface Member Properties

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

        #endregion

        #endregion Public Methods

        #region Protected Methods        

        #endregion Protected Methods

        #region Private Methods
                
        /// <summary>
        /// The internal method to convert a zoom string to a zoom scale.
        /// </summary>
        /// <param name="item">The zoom string.</param>
        /// <returns>The zoom scale.</returns>
        private static double GetZoomScaleFromString(string item)
        {
            double scale = 1.0;

            switch (item)
            {
                case "6400%":
                    scale = 100.0 / 3200.0;
                    break;
                case "3200%":
                    scale = 100.0 / 3200.0;
                    break;
                case "1600%":
                    scale = 100.0 / 1600.0;
                    break;
                case "800%":
                    scale = 100.0 / 800.0;
                    break;
                case "400%":
                    scale = 100.0 / 400.0;
                    break;
                case "200%":
                    scale = 100.0 / 200.0;
                    break;
                case "100%":
                    scale = 1.0;
                    break;
                case "75%":
                    scale = 100.0 / 75.0;
                    break;
                case "50%":
                    scale = 100.0 / 50.0;
                    break;
                case "33%":
                    scale = 100.0 / 33.3;
                    break;
                case "Fit":
                    scale = 0;
                    break;
                default:
                    scale = 1.0;
                    break;
            }

            return scale;
        }

        /// <summary>
        /// Implements the Dispose method of IDisposable that actually disposes of managed resources. 
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the class is being disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                //// Dispose of unmanaged resourses.                

                if (disposing)
                {
                    // Code to dispose the managed resources
                    // held by the class 
                    if (this.disposeCollection != null)
                    {
                        this.disposeCollection.Dispose();
                    }
                }
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Handler for the buttonLoadImage_Click event. 
        /// </summary>
        /// <param name="sender">The calling object.</param>
        /// <param name="e">The RoutedEventArgs.</param>
        private void ButtonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            bool? result = this.openfileDlg.ShowDialog();

            if (result == true)
            {
                this.MainViewModel.LoadImageVM.FileName = this.openfileDlg.FileName;
            }
        }

        /// <summary>
        /// Create a DataTemplate for parsing the menu objects.
        /// </summary>
        /// <returns>the DataTemplate.</returns>
        private DataTemplate CreateDataTemplate()
        {
            StringReader stringReader = new StringReader(
                @"<DataTemplate 
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">        
                    <MenuItem Header=""{Binding DisplayName }"" Command=""{Binding MenuCommand}""/> 
                </DataTemplate>");
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate;
        }

        /// <summary>
        /// Create an ItemsPanelTemplate for parsing the menu objects.
        /// </summary>
        /// <returns>the ItemsPanelTemplate.</returns>
        private ItemsPanelTemplate CreateItemsPanelTemplate()
        {
            StringReader stringReader = new StringReader(
                @"<ItemsPanelTemplate
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""> 
                        <StackPanel Margin=""-38,0,-55,0"" Background=""White""/>            
                   </ItemsPanelTemplate>");
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as ItemsPanelTemplate;
        }

        /// <summary>
        /// Sets binding for a RadioButton that uses a converter to an enumeration of values.
        /// </summary>
        /// <param name="control">The UI control to bind.</param>
        /// <param name="viewModel">The view model to set as the data source.</param>
        /// <param name="pathString">The name of the property of the view model to which to bind data.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <remarks>Insure that the enumeration value is present in RadioButtonSelection, 
        /// otherwise a red outline will appear when the control is selected and conversion will not function.</remarks>
        private void SetRadioButtonBinding(Control control, object viewModel, string pathString, string parameter)
        {
            Binding bind = new System.Windows.Data.Binding();
            bind.Source = viewModel;
            bind.Path = new PropertyPath(pathString);
            bind.Mode = System.Windows.Data.BindingMode.OneWayToSource;
            bind.Converter = new View.RadioButtonCheckedToEnumConverter();
            if (Enum.IsDefined(typeof(Model.RadioButtonSelection), parameter))
            {
                bind.ConverterParameter = (Model.RadioButtonSelection)Enum.Parse(typeof(Model.RadioButtonSelection), parameter);
            }

            ((RadioButton)control).SetBinding(RadioButton.IsCheckedProperty, bind);
        }

        #endregion Private Methods
    }
}
