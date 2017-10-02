using PairwisePermutationSort.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairwisePermutationSort.SortingMethods
{
    /// <summary>
    /// Сортировка путем обхода дерева всевозможных комбинаций перестановок методом поиска в глубину
    /// </summary>
    class DFSMethodManager : SortingBase
    {
        public static DFSMethodManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DFSMethodManager();
                return instance;
            }
        }
        private static DFSMethodManager instance;
        private DFSMethodManager() { }
        public override SortingResult Sort(byte[] array)
        {
            var nodes = new List<Node>();
            var stack = new Stack<Node>();

            var currentNode = new Node(array);
            if (currentNode.IsSorting)
                return new SortingResult(string.Format("Заданая последовательность чисел [{0}] является отсортированной", string.Join(",", array)));

            stack.Push(currentNode);

            while (stack.Any())
            {
                currentNode = stack.Pop();
                if (nodes.Contains(currentNode)) continue;

                nodes.Add(currentNode);

                var pairs = GetAvailablePairs(currentNode);
                foreach (var pair in pairs)
                {
                    var newNode = DoPermutation(currentNode, pair);
                    // если потенциальный потомок уже был поражден кем-то ранее, то его пропускаем
                    if (nodes.Any(x => x == newNode)) continue;

                    currentNode.AddChild(newNode);
                    if (newNode.IsSorting)
                        return CreateSortingResult(newNode, string.Format("произведено {0} вариантов перестановок", nodes.Count));

                    if (!nodes.Contains(newNode))
                        stack.Push(newNode);
                }
            }

            return new SortingResult(string.Format("Заданный массив чисел [{0}] невозможно привести к требуемому виду\r\n\r\n{1}",
                string.Join(",", array),
                string.Format("в процессе сортировки было произведено {0} перестановок", nodes.Count)));
        }
    }
}
