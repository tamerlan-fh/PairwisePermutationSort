using PairwisePermutationSort.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PairwisePermutationSort.SortingMethods
{

    abstract class SortingBase
    {
        /// <summary>
        /// Функция сортировки
        /// </summary>
        /// <param name="array">исходная последовательность, участвующая в сортировке</param>
        /// <returns>Результат сортировки</returns>
        public abstract SortingResult Sort(byte[] array);

        /// <summary>
        /// формирование последовательности итераций, приведших к конечному результату
        /// </summary>
        /// <param name="lastNode"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        protected virtual SortingResult CreateSortingResult(Node lastNode, string comment = "")
        {
            var iterations = new List<IterationInformation>();
            iterations.Add(new IterationInformation(lastNode.NumbersArray));

            if (lastNode.Parent == null) return null;

            Node previousNode = lastNode;
            Node currentNode = previousNode.Parent;

            do
            {
                iterations.Add(new IterationInformation(currentNode.NumbersArray, previousNode.PermutationPair));
                previousNode = currentNode;
                currentNode = currentNode.Parent;

            } while (currentNode != null);

            iterations.Reverse();
            return new SortingResult(iterations.ToArray(), comment);
        }

        /// <summary>
        /// получение нового узла из исходного, путем перестановки указанной связки парных чисел
        /// </summary>
        /// <param name="node">исходный узел</param>
        /// <param name="permutationPair">связка парных чисел</param>
        /// <returns>результат перестановки</returns>
        protected Node DoPermutation(Node node, Couple permutationPair)
        {
            var array = node.NumbersArray.ToArray();
            var indexOfLeft = Array.IndexOf(array, permutationPair.Left[0]);
            var indexOfRight = Array.IndexOf(array, permutationPair.Right[0]);

            array[indexOfLeft] = permutationPair.Right[0];
            array[indexOfLeft + 1] = permutationPair.Right[1];

            array[indexOfRight] = permutationPair.Left[0];
            array[indexOfRight + 1] = permutationPair.Left[1];


            return new Node(array, permutationPair, node);
        }


        /// <summary>
        /// Получение для последовательно чисел текущего узла всевозможных вариантов пар перестановок
        /// </summary>
        /// <param name="node">узел</param>
        /// <param name="needReverse">необходимость получение 'отзеркаленных' пар. Требуется в случае обратного хода</param>
        /// <returns></returns>
        protected List<Couple> GetAvailablePairs(Node node, bool needReverse = false)
        {
            return GetAvailablePairs(node.NumbersArray, needReverse);
        }
        protected List<Couple> GetAvailablePairs(byte[] array, bool needReverse = false)
        {
            var pairs = new List<Couple>();
            for (int i = 0; i < array.Length - 3; i++)
            {
                for (int j = i + 2; j < array.Length - 1; j++)
                {
                    var left = new byte[] { array[i], array[i + 1] };
                    var right = new byte[] { array[j], array[j + 1] };
                    if (needReverse)
                        pairs.Add(new Couple(right, left));
                    else
                        pairs.Add(new Couple(left, right));
                }
            }
            return pairs;
        }
    }
}
