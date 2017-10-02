using System.Collections.Generic;

namespace PairwisePermutationSort.Models
{
    public class Node
    {
        /// <summary>
        /// Узел, часть структуры Дерева
        /// </summary>
        /// <param name="array">Порядок следования чисел</param>
        public Node(byte[] array) : this(array, null, null) { }

        /// <summary>
        /// Узел, часть структуры Дерева
        /// </summary>
        /// <param name="array">Порядок следования чисел</param>
        /// <param name="pair">Сцепка парных элементов, при перестановке которых образуется текущий узел из родительского </param>
        /// <param name="parent">Родительский узел</param>
        public Node(byte[] array, Couple pair, Node parent)
        {
            this.NumbersArray = array;
            this.PermutationPair = pair;
            this.Parent = parent;
            this.Children = new List<Node>();
        }

        /// <summary>
        /// Родительский узел
        /// </summary>
        public Node Parent { get; private set; }

        /// <summary>
        /// Коллекция узлов-потомков
        /// </summary>
        public List<Node> Children { get; private set; }

        /// <summary>
        /// Сцепка парных элементов, при перестановке которых
        /// образуется текущий узел из родительского
        /// </summary>
        public Couple PermutationPair { get; private set; }

        /// <summary>
        /// Порядок следования чисел
        /// </summary>
        public byte[] NumbersArray { get; private set; }
        public void AddChild(Node node)
        {
            if (Children.Contains(node)) return;
            Children.Add(node);
        }

        /// <summary>
        /// Информация про порядок следования чисел в данном узле
        /// </summary>
        public bool IsSorting
        {
            get
            {
                for (int i = 1; i < NumbersArray.Length; i++)
                    if (NumbersArray[i - 1] > NumbersArray[i])
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Проверка, является ли входной массив подмассивом последовательности чисел узла
        /// </summary>
        /// <param name="array">проверяемый массив</param>
        /// <returns></returns>
        public bool ContainsArray(byte[] array)
        {
            if (array == null) return false;
            if (NumbersArray.Length != array.Length)
                return false;

            for (int i = 0; i < NumbersArray.Length - array.Length + 1; i++)
            {
                if (NumbersArray[i] == array[0])
                {
                    for (int j = 0; j < array.Length; j++)
                        if (NumbersArray[i + j] != array[j]) return false;
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return string.Join(",", NumbersArray);
        }

        public static bool operator ==(Node x, Node y)
        {
            if (object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null))
                return true;
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            if (x.NumbersArray.Length != y.NumbersArray.Length)
                return false;

            for (int i = 0; i < x.NumbersArray.Length; i++)
                if (x.NumbersArray[i] != y.NumbersArray[i]) return false;

            return true;
        }
        public static bool operator !=(Node x, Node y)
        {
            return !(x == y);
        }


        public override bool Equals(object obj)
        {
            return this == (obj as Node);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
