using System;
using System.Collections.Generic;
using GPart.Algorithms;
using GPart.Algorithms.Contracts;

namespace GPart
{
    /// <summary>
    /// Фабрика конфигурации и создания многоуровневого алгоритма поиска оптимального сечения графа.
    /// </summary>
    public class AlgorithmBuilder
    {
        private readonly IList<Action<BuilderOptions>> _actions = new List<Action<BuilderOptions>>();

        private AlgorithmBuilder()
        { }

        /// <summary>
        /// Использовать модифицированный алгоритм декомпозиции.
        /// </summary>
        /// <returns><see cref="AlgorithmBuilder"/></returns>
        public AlgorithmBuilder UseRadiusMod()
        {
            _actions.Add(opt =>
            {
                opt.UseRadiusMod = true;
            });

            return this;
        }

        /// <summary>
        /// Использовать модификацию алгоритма локальной оптимизации.
        /// </summary>
        /// <returns><see cref="AlgorithmBuilder"/></returns>
        public AlgorithmBuilder UseGraphBooster()
        {
            _actions.Add(opt =>
            {
                opt.UseGraphBooster = true;
            });

            return this;
        }

        /// <summary>
        /// Использовать модификацию алгоритма локальной оптимизации.
        /// </summary>
        /// <param name="quality">Количество вершин для перемещения из одного графа в другой</param>
        /// <returns><see cref="AlgorithmBuilder"/></returns>
        public AlgorithmBuilder UseGraphBooster(double quality)
        {
            _actions.Add(opt =>
            {
                opt.UseGraphBooster = true;
                opt.BoosterQuality = quality;
            });

            return this;
        }

        /// <summary>
        /// Указать количество качественных уровней.
        /// </summary>
        /// <param name="quality">Количество качественных уровней</param>
        /// <returns><see cref="AlgorithmBuilder"/></returns>
        public AlgorithmBuilder ConfigureStartNode(int quality)
        {
            _actions.Add(opt =>
            {
                opt.StartNodeQuality = quality;
            });

            return this;
        }

        /// <summary>
        /// Построить алгоритм.
        /// </summary>
        /// <returns><see cref="IMultiLevelAlgorithm"/></returns>
        public IMultiLevelAlgorithm Build()
        {
            var options = new BuilderOptions();

            foreach (var action in _actions)
            {
                action.Invoke(options);
            }

            var startNodeDetector = options.StartNodeQuality.HasValue
                ? new StartNodeDetector(options.StartNodeQuality.Value)
                : new StartNodeDetector();
            IDecomposer decomposer = options.UseRadiusMod
                ? new RegionGrowingRadiusMod(startNodeDetector)
                : new RegionGrowing(startNodeDetector);
            var reducer = new MergeNodes();
            IOptimizer optimizer = options.UseGraphBooster
                ? options.BoosterQuality.HasValue
                    ? new GraphBooster(options.BoosterQuality.Value)
                    : new GraphBooster()
                : new FeducciaMateusis();

            return new MultiLevelAlgorithm(decomposer, optimizer, reducer);
        }

        /// <summary>
        /// Создать фабрику конфигурации.
        /// </summary>
        /// <returns><see cref="AlgorithmBuilder"/></returns>
        public static AlgorithmBuilder Create()
        {
            return new AlgorithmBuilder();
        }

        private class BuilderOptions
        {
            public bool UseRadiusMod { get; set; }

            public bool UseGraphBooster { get; set; }

            public double? BoosterQuality { get; set; }

            public int? StartNodeQuality { get; set; }
        }
    }
}

