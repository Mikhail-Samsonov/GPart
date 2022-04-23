using GPart.Algorithms.Contracts.Models;

namespace GPart
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    internal class Graph : IGraph
    {
        /// <summary>
        /// Создать граф.
        /// </summary>
        /// <param name="nodes">Совокупность вершин</param>
        /// <param name="totalEdges">Количество всех ребер</param>
        public Graph(Node[] nodes, int totalEdges)
        {
            Nodes = nodes;

            for (var i = 0; i < nodes.Length; i++)
            {
                Weight += nodes[i].Weight;
            }

            TotalNodes = nodes.Length;
            TotalEdges = totalEdges;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Node[] Nodes { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Node this[int n] => Nodes[n];

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Weight { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int TotalNodes { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int TotalEdges { get; }
    }
}

