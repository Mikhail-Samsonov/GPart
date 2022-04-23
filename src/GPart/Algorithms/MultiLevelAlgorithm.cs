using GPart.Algorithms.Contracts;
using GPart.Algorithms.Contracts.Exceptions;
using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms
{
    /// <summary>
    /// Многоуровневый алгоритм поиска оптимального сечения графа.
    ///
    /// Идея алгоритма в уменьшении графа до оптимальных размеров для применения к поиску более точного алгоритма.
    /// При уменьшении графа также и нивелируется проблема наличия избыточных малосвязных участков.
    /// Эти участки редуцируются и объединяются в одну вершину.
    /// На уже редуцированном графе выполняется поиск и далее на каждом этапе восстановления найденного решения запускается алгоритм локальной оптимизации.
    /// </summary>
    internal class MultiLevelAlgorithm : IMultiLevelAlgorithm
	{
        private readonly IDecomposer _decomposer;
        private readonly IOptimizer _optimizer;
        private readonly IReducer _reducer;

        /// <summary>
        /// Создать многоуровневый алгоритм поиска оптимального сечения графа.
        /// </summary>
        /// <param name="decomposer">Алгоритм разбиения графа</param>
        /// <param name="optimizer">Алгоритм локальной оптимизации</param>
        /// <param name="reducer">Алгоритм уменьшения графа</param>
        public MultiLevelAlgorithm(IDecomposer decomposer, IOptimizer optimizer, IReducer reducer)
        {
            _decomposer = decomposer;
            _optimizer = optimizer;
            _reducer = reducer;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Solution Run(IGraph graph, int kSection, int levels)
        {
            if (levels < 0)
            {
                throw new AlgorithmException();
            }

            if (levels == 0)
            {
                var sln = _decomposer.Run(graph, kSection);

                _optimizer.Process(sln);

                return sln;
            }

            return Process(graph, levels, kSection);
        }

        /// <summary>
        /// точка входа в рекурсивный процесс редукции графа.
        /// </summary>
        /// <param name="graph">Граф</param>
        /// <param name="maxLevel">Максимальный уровень редукции</param>
        /// <param name="kSection">Целевое количество подграфов</param>
        /// <returns>Решение</returns>
        private Solution Process(IGraph graph, int maxLevel, int kSection)
        {
            var sln = _reducer.Run(graph);

            Process(sln, 1, maxLevel, kSection);

            return sln;
        }

        // рекурсивно уменьшаем граф, далее ищем решение и в обратном порядке восстанавливаем.
        // и на каждом этапе восстановления улучшаем решение
        /// <summary>
        /// Рекурсивное уменьшение графа с последующим поиском решения и восстановлением
        /// </summary>
        /// <param name="sln"><see cref="Solution"/></param>
        /// <param name="level">Текущий уровень редукции</param>
        /// <param name="maxLevel">Максимальный уровень редукции</param>
        /// <param name="kSection">Целевое количество подграфов</param>
        private void Process(Solution sln, int level, int maxLevel, int kSection)
        {
            var local = GraphFactory.CreateFrom(sln);

            if (level < maxLevel)
            {
                var next = _reducer.Run(local);

                Process(next, level + 1, maxLevel, kSection);

                sln.Merge(next);

                _optimizer.Process(sln);
            }
            else
            {
                var next = _decomposer.Run(local, kSection);

                _optimizer.Process(next);

                sln.Merge(next);

                _optimizer.Process(sln);
            }
        }
    }
}

