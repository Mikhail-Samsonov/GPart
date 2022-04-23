using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms.Contracts
{
    /// <summary>
    /// Алгоритм локальной оптимизации.
    /// </summary>
    internal interface IOptimizer
	{
		/// <summary>
        /// Процесс локальной оптимизации.
        /// </summary>
        /// <param name="sln"><see cref="Solution"/></param>
		void Process(Solution sln);
	}
}

