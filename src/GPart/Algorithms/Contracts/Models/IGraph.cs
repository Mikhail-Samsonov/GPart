namespace GPart.Algorithms.Contracts.Models
{
    /// <summary>
    /// Симметричный неориентированный граф.
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Совокупность вершин.
        /// </summary>
        Node[] Nodes { get; }

        /// <summary>
        /// Вершина по индексу.
        /// </summary>
        /// <param name="n">Индекс смежной вершины</param>
        /// <returns><see cref="Node"/></returns>
        Node this[int n] { get; }

        /// <summary>
        /// Вес графа - совокупность весов всех вершин.
        /// </summary>
        int Weight { get; }

        /// <summary>
        /// Количество всех вершин.
        /// </summary>
		int TotalNodes { get; }

        /// <summary>
        /// Количество всех ребер.
        /// </summary>
		int TotalEdges { get; }
    }
}

