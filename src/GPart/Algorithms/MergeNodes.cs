using System.Diagnostics.CodeAnalysis;
using GPart.Algorithms.Contracts;
using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms
{
    /// <summary>
    /// Алгоритм уменьшения графа путем отоджествления смежных вершин.
    ///
    /// </summary>
    internal class MergeNodes : IReducer
	{
        private const int _defaultEmptyMarker = -1;
        private readonly int _emptyMarker;

        /// <summary>
        /// Создать алгоритм уменьшения графа.
        /// </summary>
        /// <param name="emptyMarker">Идентификатор пустой вершины</param>
        /// <remarks>Алгоритм использует <paramref name="emptyMarker"/> для работы и временно записывает в решение это значение</remarks>
        public MergeNodes(int emptyMarker = _defaultEmptyMarker)
        {
            _emptyMarker = emptyMarker;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Solution Run(IGraph graph)
        {
            // подстановка для обращения к решению
            var mask = new int[graph.TotalNodes];

            for (var i = 0; i < graph.TotalNodes; i++)
            {
                mask[i] = i;
            }

            QuickSort(graph, mask);

            var sln = new int[graph.TotalNodes];
            var kSection = 0;

            for (var i = 0; i < sln.Length; i++)
            {
                sln[i] = _emptyMarker;
            }

            for (var i = 0; i < mask.Length; i++, kSection++)
            {
                // пропускаем уже занятые вершины
                while (i < mask.Length && sln[mask[i]] != _emptyMarker)
                {
                    i++;
                }

                if (i >= mask.Length)
                {
                    break;
                }

                sln[mask[i]] = kSection;

                if (TryDetectMergeNode(graph, sln, mask[i], out var n))
                {
                    sln[n.Value] = kSection;
                }
            }

            return new Solution(graph, kSection, sln);
        }

        private static void QuickSort(IGraph graph, int[] mask)
        {
            QuickSort(graph, mask, 0, mask.Length - 1);
        }

        private static void QuickSort(IGraph graph, int[] mask, int low, int high)
        {
            int l = low, h = high;
            var middle = graph[mask[(low + high) / 2]].Weight;  // middle - опорный элемент посредине между low и high
            do
            {
                while (graph[mask[l]].Weight < middle) ++l;  // поиск элемента для переноса в старшую часть
                while (middle < graph[mask[h]].Weight) --h;  // поиск элемента для переноса в младшую часть

                if (l <= h)
                {
                    (mask[l], mask[h]) = (mask[h], mask[l]);
                    l++; h--;
                }
            }
            while (l < h);

            if (low < h)
            {
                QuickSort(graph, mask, low, h);
            }

            if (l < high)
            {
                QuickSort(graph, mask, l, high);
            }
        }

        /// <summary>
        /// Определяет вершину для объединения. Выбирается свободная вершина с наименьшим весом.
        /// </summary>
        /// <param name="graph"><see cref="IGraph"/></param>
        /// <param name="sln"><see cref="Solution"/></param>
        /// <param name="n">Идентификатор вершины</param>
        /// <param name="node">Идентификатор найденной вершины</param>
        /// <returns><see langword="true"/> вершина найдена, <see langword="false"/> вершина не найдена</returns>
        private bool TryDetectMergeNode(IGraph graph, int[] sln, int n, [NotNullWhen(true)] out int? node)
        {
            if (graph[n].Nodes.Length == 0)
            {
                node = default;
                return false;
            }

            node = graph[n].Nodes[0];

            if (graph[n].Nodes.Length > 1)
            {
                for (int a = 1; a < graph[n].Nodes.Length; a++)
                {
                    if (sln[graph[n].Nodes[a]] == _emptyMarker && graph[graph[n].Nodes[a]].Weight < graph[node.Value].Weight)
                    {
                        node = graph[n].Nodes[a];
                    }
                }
            }

            return sln[node.Value] == _emptyMarker;
        }
    }
}

