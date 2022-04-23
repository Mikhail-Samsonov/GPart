using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms.Contracts
{
    /// <summary>
    /// Алгоритм разбиения графа.
    /// </summary>
    internal interface IDecomposer
    {
        /// <summary>
        /// Процесс разбиения графа.
        /// </summary>
        /// <param name="graph"><see cref="IGraph"/></param>
        /// <param name="kSection">Целевое количество подграфов</param>
        /// <returns><see cref="Solution"/></returns>
        Solution Run(IGraph graph, int kSection);
    }
}

