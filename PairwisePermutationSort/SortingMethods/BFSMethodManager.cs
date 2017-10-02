﻿using PairwisePermutationSort.Models;
using System.Collections.Generic;
using System.Linq;

namespace PairwisePermutationSort.SortingMethods
{
    /// <summary>
    /// Сортировка путем обхода дерева всевозможных комбинаций перестановок методом поиска в ширину
    /// </summary>
    class BFSMethodManager : SortingBase
    {
        public static BFSMethodManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new BFSMethodManager();
                return instance;
            }
        }
        private static BFSMethodManager instance;
        private BFSMethodManager() { }
        public override SortingResult Sort(byte[] array)
        {
            var nodes = new List<Node>();
            var queue = new Queue<Node>();

            var currentNode = new Node(array);
            if (currentNode.IsSorting)
                return new SortingResult(string.Format("Заданая последовательность чисел [{0}] является отсортированной", string.Join(",", array)));

            queue.Enqueue(currentNode);

            while (queue.Any())
            {
                currentNode = queue.Dequeue();

                var pairs = GetAvailablePairs(currentNode);
                foreach (var pair in pairs)
                {
                    var newNode = DoPermutation(currentNode, pair);
                    // если потенциальный потомок уже был поражден кем-то ранее, то его пропускаем
                    if (nodes.Contains(newNode)) continue;

                    currentNode.AddChild(newNode);
                    nodes.Add(newNode);
                    queue.Enqueue(newNode);

                    if (newNode.IsSorting)
                        return CreateSortingResult(newNode, string.Format("произведено {0} вариантов перестановок", nodes.Count));

                }
            }

            return new SortingResult(string.Format("Заданный массив чисел [{0}] невозможно привести к требуемому виду\r\n\r\n{1}",
                string.Join(",", array),
                string.Format("произведено {0} вариантов перестановок", nodes.Count)));
        }
    }
}
