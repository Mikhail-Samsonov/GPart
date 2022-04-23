namespace GPart.Algorithms.Contracts.Models
{
    /// <summary>
    /// Вершина графа.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Создать вершину графа.
        /// </summary>
        /// <param name="nodes">Совокупность индексов смежных вершин</param>
        /// <param name="weight">Вес</param>
        public Node(int[] nodes, int weight)
        {
            Nodes = nodes;
            Weight = weight;
        }

        /// <summary>
        /// Вес вершины.
        /// </summary>
        public int Weight { get; }

        /// <summary>
        /// Совокупность индексов смежных вершин.
        /// </summary>
        public int[] Nodes { get; }
    }
}