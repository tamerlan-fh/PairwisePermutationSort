using PairwisePermutationSort.Models;
using System;

namespace PairwisePermutationSort.ViewModels
{
    class SortMethodViewModel : ViewModelBase
    {
        public SortMethodViewModel(string displayName, Func<byte[], SortingResult> command)
        {
            this.DisplayName = displayName;
            this.Command = command;
        }

        /// <summary>
        /// Название Метода сортировки
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// делегат функции сортировки
        /// </summary>
        public Func<byte[], SortingResult> Command { get; private set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
