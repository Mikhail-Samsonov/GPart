using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms.Contracts
{
    /// <summary>
    /// Алгоритм уменьшения графа.
    /// </summary>
    internal interface IReducer
	{
		/// <summary>
        /// Процесс уменьшения графа.
        /// </summary>
        /// <param name="graph"><see cref="IGraph"/></param>
        /// <returns><see cref="Solution"/></returns>
		Solution Run(IGraph graph);
	}
}

