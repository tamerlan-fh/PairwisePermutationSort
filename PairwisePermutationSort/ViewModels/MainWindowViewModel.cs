using PairwisePermutationSort.SortingMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace PairwisePermutationSort.ViewModels
{
    class MainWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        private const int dimension = 6;
        private byte[] array;
        public MainWindowViewModel()
        {
            Methods = new List<SortMethodViewModel>();
            Methods.Add(new SortMethodViewModel("Метод обратного хода", BackStrokeMethodManager.Instance.Sort));
            Methods.Add(new SortMethodViewModel("Метод обхода в ширину", BFSMethodManager.Instance.Sort));
            Methods.Add(new SortMethodViewModel("Метод обхода в глубину", DFSMethodManager.Instance.Sort));

            SelectedMethod = Methods.FirstOrDefault();
            SortingCommand = new RelayCommand(param => Sort(), param => CanSort());
            CloseCommand = new RelayCommand(param => App.Current.Shutdown());
            AboutCommand = new RelayCommand(param => About());
        }

        /// <summary>
        /// Коллекция методов сортировки
        /// </summary>
        public List<SortMethodViewModel> Methods { get; private set; }

        /// <summary>
        /// выбранный метод сортировки
        /// </summary>
        public SortMethodViewModel SelectedMethod
        {
            get { return selectedMethod; }
            set { selectedMethod = value; OnPropertyChanged("SelectedMethod"); }
        }
        private SortMethodViewModel selectedMethod;
        public ICommand SortingCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }

        /// <summary>
        /// Текстовая строка, вводимая пользователем
        /// </summary>
        public string InputString
        {
            get { return inputString; }
            set { inputString = value; OnPropertyChanged("InputString"); }
        }
        private string inputString;
      
        /// <summary>
        /// форматированный текст с результатом сортировки
        /// </summary>
        public FlowDocument Document
        {
            get { return документРезультатов; }
            set { документРезультатов = value; OnPropertyChanged("Document"); }
        }
        private FlowDocument документРезультатов;
        private void Sort()
        {
            var result = SelectedMethod.Command(array);
            Document = result.CreateDocument();
        }
     
        private bool CanSort()
        {
            return (array != null && array.Length == dimension);
        }

        private void About()
        {
            MessageBox.Show("Все предложения, замечания и ругательства просьба не высылать на адрес tamerlan-fh@ya.ru\r\n\r\nУфа, 2017г ", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #region Validation

        public bool IsValid
        {
            get { return GetValidationError("") == null; }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        public string this[string columnName]
        {
            get { return GetValidationError(columnName); }
        }
        private string GetValidationError(string propertyName)
        {
            switch (propertyName)
            {
                case "InputString":
                    {
                        array = null;

                        if (string.IsNullOrEmpty(InputString))
                            return string.Empty;

                        var bytes = ConvertToArray(InputString);

                        if (bytes.Length != bytes.Distinct().Count())
                            return "В последовательности встречаются повторяющиеся значения";

                        if (bytes.Length < dimension)
                            return string.Empty;

                        if (bytes.Length > dimension)
                            return "Число элементов последовательности превышает необходимого значения";
                        if (bytes.Length == dimension)
                            array = bytes;

                        break;
                    }
            }
            return string.Empty;
        }
        private byte[] ConvertToArray(string context)
        {
            var pattern = new Regex(@"([^1-6]*(?<number>[1-6]{1})[^1-6]*)?");
            var matches = pattern.Matches(context);

            var bytes = new List<byte>();
            foreach (Match match in matches)
            {
                if (!match.Success) continue;
                byte number = 0;
                if (!byte.TryParse(match.Groups["number"].Value, out number)) continue;
                bytes.Add(number);
            }

            return bytes.ToArray();
        }

        #endregion 
    }
}
