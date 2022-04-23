using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms.Contracts
{
    /// <summary>
    /// Алгоритм поиска псевдо-периферийной вершины (ППВ).
    /// </summary>
    internal interface IStartNodeDetector
	{
        /// <summary>
        /// Процесс поиска псевдо-периферийной вершины (ППВ).
        /// </summary>
        /// <param name="graph"><see cref="IGraph"/></param>
        /// <returns>Совокупность псевдо-периферийных вершин</returns>
        int[] Run(IGraph graph);
	}
}

