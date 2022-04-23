using System.Collections.Generic;
using System.Linq;
using GPart.Algorithms.Contracts;
using GPart.Algorithms.Contracts.Exceptions;
using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms
{
    /// <summary>
    /// Алгоритм локального улучшения.
    ///
    /// Идея алгоритма во взаимно поочередном обмене смежными вершинами сначала в прямом направлении, затем - в обратном.
    /// Этот обмен позволяет достичь сразу 2 целей:
    /// 1. преодолеть локальные минимумы
    /// 2. улучшить текущее решение
    /// </summary>
    internal class GraphBooster : IOptimizer
    {
        private const double _qualityDefault = 0.1;
        private readonly double _quality;

        /// <summary>
        /// Создать алгоритм локальной оптимизации.
        /// </summary>
        /// <param name="quality">Количество вершин для перемещения из одного графа в другой</param>
        /// <remarks>Величина параметра <paramref name="quality"/> не должна быть слишком большой, т.к. это негативно скажется как на качестве, так и на скорости работы и на величине потребляемой памяти</remarks>
        public GraphBooster(double quality = _qualityDefault)
        {
            _quality = quality;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Process(Solution sln)
        {
            var buffer = new HashSet<int>();

            for (var from = 1; from <= sln.KSection; from++)
            {
                for (var to = 1; to <= sln.KSection; to++)
                {
                    if (from == to)
                    {
                        continue;
                    }

                    buffer.Clear();

                    for (var n = 0; n < sln.Graph.TotalNodes; n++)
                    {
                        if (sln[n] != from)
                        {
                            continue;
                        }

                        for (var a = 0; a < sln.Graph[n].Nodes.Length; a++)
                        {
                            if (sln[sln.Graph[n].Nodes[a]] == to)
                            {
                                buffer.Add(n);
                                break;
                            }
                        }
                    }

                    if (buffer.Count == 0)
                    {
                        continue;
                    }

                    for (int i = 0, n; i < (sln.Graph.TotalNodes / sln.KSection) * _quality; i++)
                    {
                        // todo заменить бы Linq на что-то. Может предрассчитывать profit и хранить его, т.к. изменяется лишь малая дельта из всего списка.
                        n = buffer.MaxBy(x => CalculateProfit(sln, x, to));
                        sln[n] = to;
                        buffer.Remove(n);

                        for (var a = 0; a < sln.Graph[n].Nodes.Length; a++)
                        {
                            if (sln[sln.Graph[n].Nodes[a]] == from)
                            {
                                buffer.Add(sln.Graph[n].Nodes[a]);
                            }
                        }
                    }

                    buffer.Clear();

                    for (var n = 0; n < sln.Graph.TotalNodes; n++)
                    {
                        if (sln[n] != to)
                        {
                            continue;
                        }

                        for (var a = 0; a < sln.Graph[n].Nodes.Length; a++)
                        {
                            if (sln[sln.Graph[n].Nodes[a]] == from)
                            {
                                buffer.Add(n);
                                break;
                            }
                        }
                    }

                    if (buffer.Count == 0)
                    {
                        throw new AlgorithmException();
                    }

                    for (int i = 0, n; i < (sln.Graph.TotalNodes / sln.KSection) * _quality; i++)
                    {
                        n = buffer.MaxBy(x => CalculateProfit(sln, x, from));
                        sln[n] = from;
                        buffer.Remove(n);

                        for (var a = 0; a < sln.Graph[n].Nodes.Length; a++)
                        {
                            if (sln[sln.Graph[n].Nodes[a]] == to)
                            {
                                buffer.Add(sln.Graph[n].Nodes[a]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Выгода от перемещения вершины <paramref name="n"/> в граф <paramref name="g"/>
        /// </summary>
        /// <param name="sln"><see cref="Solution"/></param>
        /// <param name="n">Идентификатор вершины</param>
        /// <param name="g">Идентификатор графа</param>
        /// <returns>Величина улучшения</returns>
        /// <remarks>Чем больше величина улучшения, тем лучше</remarks>
        private static int CalculateProfit(Solution sln, int n, int g)
        {
            return CalculatePower(sln, n, g) - CalculatePower(sln, n, sln[n]);
        }

        /// <summary>
        /// Количество связей от вершины <paramref name="n"/> до графа <paramref name="g"/>
        /// </summary>
        /// <param name="sln"><see cref="Solution"/></param>
        /// <param name="n">Идентификатор вершины</param>
        /// <param name="g">Идентификатор графа</param>
        /// <returns>Количество связей</returns>
        private static int CalculatePower(Solution sln, int n, int g)
        {
            var power = 0;

            for (var a = 0; a < sln.Graph[n].Nodes.Length; a++)
            {
                if (sln[sln.Graph[n].Nodes[a]] == g)
                {
                    power++;
                }
            }

            return power;
        }
    }
}

