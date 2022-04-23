using System;
using System.Collections.Generic;
using GPart.Algorithms.Contracts;
using GPart.Algorithms.Contracts.Exceptions;
using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    internal class RegionGrowing : IDecomposer
	{
		private static readonly Random _rnd = new Random(DateTime.Now.Millisecond);
		private readonly IStartNodeDetector _startNodeDetector;

		/// <summary>
		/// Создать алгоритм разбиения графа.
		/// </summary>
		/// <param name="startNodeDetector">Алгоритм поиска псевдо-периферийной вершины (ППВ)</param>
		public RegionGrowing(IStartNodeDetector startNodeDetector)
        {
            _startNodeDetector = startNodeDetector;
        }

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public Solution Run(IGraph graph, int kSection)
		{
			if (graph.TotalNodes == 0 || kSection <= 0)
			{
				throw new AlgorithmException();
			}

			var startNodes = _startNodeDetector.Run(graph);

			if (startNodes.Length == 0)
			{
				throw new AlgorithmException();
			}

			var runNode = startNodes[_rnd.Next(startNodes.Length)];
			var sln = new Solution(graph, kSection);
			var buffer = new List<int>() { runNode };

			for (int g = 1, w, total = 0; g <= kSection; g++, total += w)
			{
				if (buffer.Count == 0)
				{
					throw new AlgorithmException();
				}

				var node = DetectStartNode(sln, buffer);

				sln[node] = g;
				buffer.Remove(node); // todo swap ?

				for (var a = 0; a < graph[node].Nodes.Length; a++)
				{
					if (sln[graph[node].Nodes[a]] == Solution.EmptyMarker && !buffer.Contains(graph[node].Nodes[a]))
					{
						buffer.Add(graph[node].Nodes[a]);
					}
				}

				for (w = graph[node].Weight; w < (graph.Weight - total) / (kSection - g + 1); w += graph[node].Weight)
				{
					if (buffer.Count == 0)
					{
						throw new AlgorithmException();
					}

					node = DetectNextNode(sln, buffer, g);
					sln[node] = g;
					buffer.Remove(node); // todo swap ?

					for (var a = 0; a < graph[node].Nodes.Length; a++)
					{
						if (sln[graph[node].Nodes[a]] == Solution.EmptyMarker && !buffer.Contains(graph[node].Nodes[a]))
						{
							buffer.Add(graph[node].Nodes[a]);
						}
					}
				}
			}

			if (buffer.Count > 0)
			{
				throw new AlgorithmException();
			}

			return sln;
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

		/// <summary>
		/// Выбор стартовой вершины построения графа. Стартовая вершина - наименее связная вершина относительно сформированного графа.
		/// </summary>
		/// <param name="sln"><see cref="Solution"/></param>
		/// <param name="buffer">Буфер фронтовых вершин</param>
		/// <returns>Идентификатор стартовой вершины</returns>
		/// <exception cref="AlgorithmException">Буфер фронтовых вершин пуст</exception>
		private static int DetectStartNode(Solution sln, List<int> buffer)
		{
			if (buffer.Count == 0)
			{
				throw new AlgorithmException();
			}

			var node = buffer[0];

			if (buffer.Count > 1)
			{
				var power = sln.Graph[node].Nodes.Length - CalculatePower(sln, node, Solution.EmptyMarker);

				for (int i = 1, p; i < buffer.Count; i++)
				{
					p = sln.Graph[buffer[i]].Nodes.Length - CalculatePower(sln, buffer[i], Solution.EmptyMarker);

					if (p < power)
					{
						node = buffer[i];
						power = p;
					}
				}
			}

			return node;
		}

		/// <summary>
		/// Выбор следующей вершины графа. Следующая вершина - максимально связная с текущим формируемым графом.
		/// </summary>
		/// <param name="sln"><see cref="Solution"/></param>
		/// <param name="buffer">Буфер фронтовых вершин</param>
		/// <param name="g">Формируемый граф</param>
		/// <returns>Идентификатор следующей вершины</returns>
		/// <exception cref="AlgorithmException">Буфер фронтовых вершин пуст</exception>
		private static int DetectNextNode(Solution sln, List<int> buffer, int g)
		{
			if (buffer.Count == 0)
			{
				throw new AlgorithmException();
			}

			var node = buffer[0];

			if (buffer.Count > 1)
			{
				var power = CalculatePower(sln, node, g);

				for (int i = 1, p; i < buffer.Count; i++)
				{
					p = CalculatePower(sln, buffer[i], g);

					if (power < p)
					{
						node = buffer[i];
						power = p;
					}
				}
			}

			return node;
		}
	}	
}