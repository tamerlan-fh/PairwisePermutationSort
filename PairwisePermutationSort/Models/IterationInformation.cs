using System;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

namespace PairwisePermutationSort.Models
{
    /// <summary>
    /// Информация о итерации сортировки
    /// </summary>
    public class IterationInformation
    {
        /// <summary>
        /// Информация о итерации сортировки
        /// </summary>
        /// <param name="array">Порядок следования чисел в данной итерации</param>
        public IterationInformation(byte[] array) : this(array, null) { }

        /// <summary>
        /// Информация о итерации сортировки
        /// </summary>
        /// <param name="array">Порядок следования чисел в данной итерации</param>
        /// <param name="pair">Сцепка парных элементов, участвующих в перестановке на данной итерации</param>
        public IterationInformation(byte[] array, Couple pair)
        {
            this.NumbersArray = array;
            this.PermutationPair = pair;
        }

        /// <summary>
        /// Порядок следования чисел в данной итерации
        /// </summary>
        public byte[] NumbersArray { get; private set; }

        /// <summary>
        /// Сцепка парных элементов, участвующих в перестановке на данной итерации
        /// </summary>
        public Couple PermutationPair { get; private set; }
        public override string ToString()
        {
            return string.Format("{0}\t{1}", string.Join(",", NumbersArray), PermutationPair);
        }

        public Paragraph GetParagraph()
        {
            string separator = "";

            if (PermutationPair == null)
                return new Paragraph(new Run(string.Join(separator, NumbersArray)));

            var indexOfLeft = Array.IndexOf(NumbersArray, PermutationPair.Left[0]);
            var indexOfRight = Array.IndexOf(NumbersArray, PermutationPair.Right[0]);

            int line1Position = 0;
            int line1Length = indexOfLeft;
            var line1 = new Run(string.Format("{0}", line1Length == 0 ? "" : string.Format("{1}{0}", separator, string.Join(separator, NumbersArray.Skip(line1Position).Take(line1Length)))));
            var line2 = new Bold(new Run(string.Join(separator, PermutationPair.Left)));

            int line3Position = indexOfLeft + PermutationPair.Left.Length;
            int line3Length = indexOfRight - indexOfLeft - PermutationPair.Left.Length;
            var line3 = new Run(string.Format("{0}", line3Length == 0 ? separator : string.Format("{0}{1}{0}", separator, string.Join(separator, NumbersArray.Skip(line3Position).Take(line3Length)))));
            var line4 = new Bold(new Run(string.Join(separator, PermutationPair.Right)));

            int line5Position = indexOfRight + PermutationPair.Right.Length;
            int line5Length = NumbersArray.Length - indexOfRight - PermutationPair.Right.Length;
            var line5 = new Run(string.Format("{0}", line5Length == 0 ? "" : string.Format("{0}{1}", separator, string.Join(separator, NumbersArray.Skip(line5Position).Take(line5Length)))));
            //var line6 = new Run(string.Format("\t"));
            //var line7 = new Bold(new Run(string.Join(" ", ПрименимаяПерестановка.ЛеваяПара)));
            //var line8 = new Run(string.Format(" <-> "));
            //var line9 = new Bold(new Run(string.Join(" ", ПрименимаяПерестановка.ПраваяПара)));
            var line6 = new Run(string.Format("\t{0} ↔ {1}", string.Join(separator, PermutationPair.Left), string.Join(separator, PermutationPair.Right)));

            var paragraph = new Paragraph();
            paragraph.Inlines.Add(line1);
            paragraph.Inlines.Add(line2);
            paragraph.Inlines.Add(line3);
            paragraph.Inlines.Add(line4);
            paragraph.Inlines.Add(line5);
            paragraph.Inlines.Add(line6);


            var rangeLeft = new TextRange(line2.ContentStart, line2.ContentEnd);
            rangeLeft.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);

            var rangeRight = new TextRange(line4.ContentStart, line4.ContentEnd);
            rangeRight.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);

            return paragraph;
        }
    }
}
