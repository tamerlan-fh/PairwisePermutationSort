using PairwisePermutationSort.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PairwisePermutationSort.SortingMethods
{
    /// <summary>
    /// основной смысл метода - сформировать дерево достижимости
    /// </summary>
    class BackStrokeMethodManager : SortingBase
    {
        public static BackStrokeMethodManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new BackStrokeMethodManager();
                return instance;
            }
        }
        private static BackStrokeMethodManager instance;
        private BackStrokeMethodManager() { }

        private Tree tree = null;
        public override SortingResult Sort(byte[] array)
        {
            if (array == null || !array.Any())
            {
                MessageBox.Show("Исходная последовательность значений не обьявлена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
          
            if (tree == null)
                tree = BuildTree();

            if (tree.Root.ContainsArray(array))
                return new SortingResult(string.Format("Заданая последовательность чисел [{0}] является отсортированной", string.Join(",", array)));

            if (!tree.ConteinsNode(array))
                return new SortingResult(string.Format("Заданный массив чисел [{0}] невозможно привести к требуемому виду\r\n\r\n{1}",
                        string.Join(",", array),
                        string.Format("произведено {0} вариантов перестановок", tree.Nodes.Count)));

            return CreateSortingResult(tree.FindNode(array), string.Format("произведено {0} вариантов перестановок", tree.Nodes.Count));
        }

        protected override SortingResult CreateSortingResult(Node node, string comment = "")
        {
            var iterations = new List<IterationInformation>();

            Node currentNode = node;

            while (currentNode.Parent != null)
            {
                iterations.Add(new IterationInformation(currentNode.NumbersArray, currentNode.PermutationPair));
                currentNode = currentNode.Parent;
            }
            iterations.Add(new IterationInformation(currentNode.NumbersArray, currentNode.PermutationPair));
            return new SortingResult(iterations.ToArray(), comment);
        }

        /// <summary>
        /// дерево формируется от отсортированной коллекции чисел, путем всевозможных перестановок. Выявляются все возможные комбинации последовательностей, которые могут быть сортированны
        /// </summary>
        /// <returns>дерево</returns>
        private Tree BuildTree()
        {
            var rootArray = new byte[] { 1, 2, 3, 4, 5, 6 };

            var root = new Node(rootArray);
            var nodes = new List<Node>();

            var queue = new Queue<Node>();
            var currentNode = root;
            queue.Enqueue(currentNode);

            while (queue.Any())
            {
                currentNode = queue.Dequeue();
                var pairs = GetAvailablePairs(currentNode, true);
                foreach (var pair in pairs)
                {
                    var newNode = DoPermutation(currentNode, pair);

                    // если потенциальный потомок уже был поражден кем-то ранее, то его пропускаем
                    if (root == newNode || nodes.Any(x => x == newNode)) continue;

                    currentNode.AddChild(newNode);
                    nodes.Add(newNode);
                    queue.Enqueue(newNode);
                }
            }

            return new Tree(root, nodes);
        }
    }
}
