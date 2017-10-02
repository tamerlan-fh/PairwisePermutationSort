namespace PairwisePermutationSort.Models
{
    /// <summary>
    /// Сцепка двух пар чисел, участвующих в перестановке во время сортировки
    /// </summary>
    public class Couple
    {
        public Couple(byte[] left, byte[] right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Пара чисел, расположенных в сцепке слева
        /// </summary>
        public byte[] Left { get; private set; }

        /// <summary>
        /// Пара чисел, расположенных в сцепке справа
        /// </summary>
        public byte[] Right { get; private set; }
        public override string ToString()
        {
            return string.Format("{0} <-> {1}", string.Join(",", Left), string.Join(",", Right));
        }
    }
}
