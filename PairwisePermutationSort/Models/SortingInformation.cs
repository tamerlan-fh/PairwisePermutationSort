using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

namespace PairwisePermutationSort.Models
{
    /// <summary>
    /// Результат сортировки
    /// </summary>
    public class SortingResult
    {
        /// <summary>
        /// Результат сортировки
        /// </summary>
        /// <param name="comment">комментарии по сортировке</param>
        public SortingResult(string comment) : this(null, comment) { }

        /// <summary>
        /// Результат сортировки
        /// </summary>
        /// <param name="iterations">последовательность итераций, приведших к требуемому результату</param>
        public SortingResult(IterationInformation[] iterations) : this(iterations, string.Empty) { }

        /// <summary>
        /// Результат сортировки
        /// </summary>
        /// <param name="iterations">последовательность итераций, приведших к требуемому результату</param>
        /// <param name="comment">комментарии по сортировке</param>
        public SortingResult(IterationInformation[] iterations, string comment)
        {
            Iterations = iterations;
            Comment = comment;
        }

        /// <summary>
        /// true - сортировка прошла успешна
        /// false  - в противном случае
        /// </summary>
        public bool Success
        {
            get { return Iterations != null && Iterations.Any(); }
        }
        public IterationInformation[] Iterations { get; private set; }

        /// <summary>
        /// Комментарий по сортировке
        /// </summary>
        public string Comment { get; set; }
        public override string ToString()
        {
            if (Success)
                return string.Format("{0}\r\n\r\n{1}", string.Join("\r\n", Iterations.ToList()), Comment);
            else
                return Comment;
        }

        /// <summary>
        /// Формирование форматированного текстового представления сортировки
        /// </summary>
        /// <returns></returns>
        public FlowDocument CreateDocument()
        {
            var document = new FlowDocument();

            if (Success)
            {
                foreach (var iteration in Iterations)
                    document.Blocks.Add(iteration.GetParagraph());
                document.Blocks.Add(new Paragraph(new Italic(new Run(string.Format("\r\n\r\n{0}", Comment)))));
            }
            else
                document.Blocks.Add(new Paragraph(new Italic(new Run(Comment))));

            document.FontFamily = new FontFamily("Arial");
            document.LineHeight = 3;
            return document;
        }
    }
}
