using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Font5.Annotations;

namespace Font5
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        public const string FontSearchText = "Search";

        public const double DefaultFontSize = 32;

        public const string DefaultFontPreviewText = "Abc";

        public event PropertyChangedEventHandler PropertyChanged;

        private string customText = DefaultFontPreviewText;

        private ICollection<FontFamily> allFonts = Fonts.SystemFontFamilies;

        private IEnumerable<FontFamily> systemFonts = Fonts.SystemFontFamilies;

        private double fontSize = DefaultFontSize;

        private double smallFontSize = 14;

        private string searchCriteria;

        private bool isItalic;

        private bool fontNamesOn = true;

        private double windowWidth;

        private bool isUnderlined;

        private bool isStrikethrough;

        private IList<string> textDecorations = new List<string>();

        private Color textColor = Color.FromRgb(15, 15, 0);

        private Color backgroundColor = Color.FromRgb(255, 255, 255);

        private int rows = 5;

        private int columns = 4;

        private List<FontFamily> selectedFonts = new List<FontFamily>();
        private List<FontFamily> previousSelection;

        private bool isTextViewVisible = false;
        private bool isTextViewEnabled = false;

        public List<int> rowsColumnsValue = Enumerable.Range(1, 10).ToList();

        private Color selectedColor;

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }


        public List<int> RowsColumnsValue
        {
            get
            {
                return rowsColumnsValue;
            }
            set
            {
                rowsColumnsValue = value;
                OnPropertyChanged();
            }
        }

        public FontFamily SelectedFont
        {
            get
            {
                return SelectedFonts.DefaultIfEmpty(new FontFamily()).First();
            }
        }


        public List<FontFamily> PreviousSelection
        {
            get
            {
                return previousSelection;
            }
            set
            {
                previousSelection = value;
                OnPropertyChanged();
            }
        }

        public bool IsTextViewVisible
        {
            get { return isTextViewVisible; }
            set
            {
                isTextViewVisible = value;
                OnPropertyChanged();
            }
        }

        public bool ViewTextEnabled
        {
            get { return isTextViewEnabled; }
            set
            {
                isTextViewEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool EnableTextView()
        {
            if (SelectedFonts == null)
                return false;

            return SelectedFonts.Count == 1;
        }

        public bool CompareFontsEnabled
        {
            get
            {
                if (SelectedFonts == null || SelectedFonts.Count == 0)
                    if (PreviousSelection != null)
                        return PreviousSelection.Count >= 2;
                return SelectedFonts.Count >= 2;
            }
        }

        public void SelectedFontsCollectionChanged()
        {
            ViewTextEnabled = EnableTextView();
            OnPropertyChanged("CompareFontsEnabled");
        }

        public FontFamily FirstSelectedFont
        {
            get { return selectedFonts.FirstOrDefault(); }
        }

        public List<FontFamily> SelectedFonts
        {
            get { return selectedFonts; }
            set
            {
                selectedFonts = value;
                OnPropertyChanged();
            }
        }

        public int Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged();
            }
        }

        public int Rows
        {
            get { return rows; }
            set
            {
                rows = value;
                OnPropertyChanged();
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public Brush Background
        {
            get { return new SolidColorBrush(backgroundColor); }
        }


        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                OnPropertyChanged();
            }
        }

        public Brush Foreground
        {
            get { return new SolidColorBrush(textColor); }
        }

        #endregion

        #region Properties

        private bool isLoadingFonts;

        public bool IsLoadingFonts
        {
            get { return isLoadingFonts; }
            set
            {
                isLoadingFonts = value;
                OnPropertyChanged();
            }
        }

        public int FontsNumber
        {
            get { return systemFonts.ToArray().Length; }
        }

        public bool FontNamesOn
        {
            get { return fontNamesOn; }
            set
            {
                fontNamesOn = value;
                OnPropertyChanged();
            }
        }

        public bool IsItalic
        {
            get { return isItalic; }
            set
            {
                isItalic = value;
                OnPropertyChanged();
                OnPropertyChanged("IsFontItalic");
            }
        }

        public FontStyle IsFontItalic
        {
            get
            {
                return isItalic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        public bool IsStrikethrough
        {
            get { return isStrikethrough; }
            set
            {
                isStrikethrough = value;
                if (value)
                {
                    textDecorations.Add("Strikethrough");
                }
                else
                {
                    textDecorations.Remove("Strikethrough");
                }
                OnPropertyChanged("TextDecorations");
            }
        }

        public bool IsUnderlined
        {
            get { return isUnderlined; }
            set
            {
                isUnderlined = value;
                if (value)
                {
                    textDecorations.Add("Underline");
                }
                else
                {
                    textDecorations.Remove("Underline");
                }
                OnPropertyChanged("TextDecorations");
            }
        }

        public string TextDecorations
        {
            get
            {
                return string.Join(",", textDecorations);
            }
        }

        public string SearchCriteria
        {
            get { return searchCriteria; }
            set
            {
                searchCriteria = value;
                SearchFont();
                OnPropertyChanged();
            }
        }

        public double FontSize
        {
            get { return Math.Round(fontSize, 0); }
            set
            {
                fontSize = value;
                OnResizeUpdate(windowWidth);
                OnPropertyChanged();
            }
        }

        public double SmallFontSize
        {
            get
            {
                return smallFontSize;
            }
            set
            {
                smallFontSize = value;
                OnPropertyChanged();
            }
        }

        public string CustomText
        {
            get { return customText; }
            set
            {
                customText = value;
                OnPropertyChanged();
            }
        }

        public ICollection<FontFamily> SystemFonts
        {
            get
            {
                return systemFonts.OrderBy(x => x.ToString()).ToList();
            }
            set
            {
                systemFonts = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Methods

        public Task SearchFont()
        {
            return Task.Run(() =>
            {
                SystemFonts = string.IsNullOrEmpty(SearchCriteria) || searchCriteria.Equals(FontSearchText, StringComparison.InvariantCultureIgnoreCase)
                    ? allFonts.AsParallel().WithDegreeOfParallelism(4).ToList() : allFonts.AsParallel().WithDegreeOfParallelism(4)
                        .Where(f => f.Source.ToLowerInvariant()
                            .Contains(SearchCriteria.ToLowerInvariant())).OrderBy(x => x.ToString()).ToList();
            });
        }

        public void CalculateSmallFontSize()
        {
            var newSize = fontSize / 3;
            if (newSize > 14)
            {
                smallFontSize = newSize;
            }
            else
            {
                smallFontSize = 14;
            }
        }

        #endregion

        public MainViewModel()
        {
            systemFonts = SystemFonts;
        }

        public void OnResizeUpdate(double width)
        {
            windowWidth = width;
            CalculateSmallFontSize();
        }
    }
}