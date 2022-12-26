using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Font5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel dt = null;

        public MainWindow()
        {
            InitializeComponent();
            dt = (MainViewModel)DataContext;

            Left = 0;
            Top = 0;
            Width = SystemParameters.WorkArea.Width / 2;
            Height = SystemParameters.WorkArea.Height;

            SizeChanged += OnWindowSizeChanged;
        }

        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            dt.OnResizeUpdate(e.NewSize.Width);
        }

        private void fontSize_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var slider = sender as Slider;
            slider.Focus();
            if (slider != null)
            {
                if (e.Delta > 0)
                {
                    slider.Value += 1;
                }
                else if (e.Delta < 0)
                {
                    slider.Value -= 1;
                }
            }
        }

        private void GridRC_Checked(object sender, RoutedEventArgs e)
        {
            if (dt != null)
            {
                dt.Columns = 4;
                dt.Rows = 5;
            }
        }

        private void ListRC_Checked(object sender, RoutedEventArgs e)
        {
            if (dt != null)
            {
                dt.Columns = 1;
                dt.Rows = 5;
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            dt.SelectedFonts = listBox.SelectedItems.Cast<FontFamily>().ToList();
            dt.SelectedFontsCollectionChanged();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            dt.IsTextViewVisible = false;
            TextRadioButton.IsChecked = false;
            GridFontsPreview.Focus();
        }

        private bool TextViewChecked;
        private void TextView_Checked(object sender, RoutedEventArgs e)
        {
            dt.IsTextViewVisible = true;
            TextViewChecked = true;
        }

        private void TextView_Click(object sender, RoutedEventArgs e)
        {
            if (TextViewChecked)
            {
                TextViewChecked = false;
                e.Handled = true;
                return;
            }
            RadioButton s = sender as RadioButton;
            if (s.IsChecked.Value)
            {
                dt.IsTextViewVisible = false;
                TextRadioButton.IsChecked = false;
                GridFontsPreview.Focus();
            }
        }

        private bool SelectionCompareChecked;
        private void SelectionCompare_Checked(object sender, RoutedEventArgs e)
        {
            dt.SystemFonts = dt.SelectedFonts;
            VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(GridFontsPreview)?.ScrollToTop();
            SelectionCompareChecked = true;
        }

        private void SelectionCompare_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionCompareChecked)
            {
                dt.PreviousSelection = GridFontsPreview.SelectedItems.Cast<FontFamily>().ToList();
                GridFontsPreview.SelectedItems.Clear();

                SelectionCompareChecked = false;
                e.Handled = true;
                return;
            }
            RadioButton s = sender as RadioButton;
            if (s.IsChecked.Value)
            {
                foreach (var item in dt.PreviousSelection)
                {
                    GridFontsPreview.SelectedItems.Add(item);
                }
                GridFontsPreview.Focus();
                dt.PreviousSelection = null;

                s.IsChecked = false;
                dt.SearchFont();
            }
        }

        private void ClearSelectionBtn_Click(object sender, RoutedEventArgs e)
        {
            GridFontsPreview.SelectedItems.Clear();
        }
    }
}
