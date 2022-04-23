using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms.Contracts
{
    /// <summary>
    /// Многоуровневый алгоритм поиска оптимального сечения графа.
    /// </summary>
    public interface IMultiLevelAlgorithm
	{
		/// <summary>
        /// Процесс поиска сечения.
        /// </summary>
        /// <param name="graph"><see cref="IGraph"/></param>
        /// <param name="kSection">Целевое количество подграфов</param>
        /// <param name="levels">Количество этапов редукции</param>
        /// <returns><see cref="Solution"/></returns>
		Solution Run(IGraph graph, int kSection, int levels);
	}
}

