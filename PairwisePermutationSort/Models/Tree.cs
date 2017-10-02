using System.Collections.Generic;
using System.Linq;

namespace PairwisePermutationSort.Models
{
    /// <summary>
    /// Структура Дерево
    /// </summary>
    class Tree
    {
        /// <summary>
        /// Структура Дерево
        /// </summary>
        /// <param name="root">Узел, являющийся корнем дерева</param>
        /// <param name="nodes">Коллекция узлов, формирующих дерево</param>
        public Tree(Node root, List<Node> nodes)
        {
            this.Root = root;
            this.Nodes = nodes;
        }
        /// <summary>
        /// Узел, являющийся корнем дерева
        /// </summary>
        public Node Root { get; private set; }

        /// <summary>
        /// Коллекция узлов, формирующих дерево
        /// </summary>
        public List<Node> Nodes { get; private set; }

        public Node FindNode(byte[] array)
        {
            return Nodes.FirstOrDefault(node => node.ContainsArray(array));
        }

        public bool ConteinsNode(byte[] array)
        {
            return FindNode(array) != null;
        }
    }
}
