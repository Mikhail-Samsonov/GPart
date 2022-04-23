using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using GPart.Algorithms.Contracts;
using GPart.Algorithms.Contracts.Exceptions;
using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms
{
	/// <summary>
	/// Алгоритм поиска псевдо-периферийной вершины (ППВ).
	///
	/// ППВ - вершина максимально удаленная от всех остальных.
	/// Идея алгоритма - начиная с рандомной вершины, найти максимально удаленные вершины и затем повторить процесс от найденных вершин на прежнем шаге.
	/// </summary>
	/// <remarks>
	/// Шаги алгорима можно повторять сколь угодно раз.
	/// Обычно 2, 3 шагов достаточно.
	/// </remarks>
	internal class StartNodeDetector : IStartNodeDetector
	{
		private const int _qualityDefault = 2;
		private readonly int _quality;

		/// <summary>
        /// Создать алгоритм поиска псевдо-периферийной вершины (ППВ)
        /// </summary>
        /// <param name="quality">Количество качественных уровней поиска</param>
        /// <remarks>Чем больше <paramref name="quality"/> - тем лучше, но дольше по времени и дороже по пямяти. Обычно значение 2, 3 - оптимально.</remarks>
		public StartNodeDetector(int quality = _qualityDefault)
		{
			_quality = quality;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public int[] Run(IGraph graph)
        {
			if (graph.TotalNodes == 0)
			{
				throw new AlgorithmException();
			}

			var buffer = new Buffer(3);

			// индексы псевдо-периферийных значений
			var nodes = new Buffer(2);
			nodes[0].Add(0); // стартовая вершина
			var q = 0; // индекс качественного уровня

			// q - счетчик качественных итераций
			// k - счетчик радиусов вершин в графе
			for (int k = 0; q < _quality; q++)
			{
				nodes[q + 1].Clear();

				foreach (var node in nodes[q])
				{
					buffer.ClearAll();
					buffer[k].Add(node);

					for (var i = buffer[k].Count; i < graph.TotalNodes; k += 2) // k += 2 т.к. за одну итерацию строим 2 уровня
					{
						buffer[k + 1].Clear();

						foreach (var n in buffer[k])
						{
							for (var j = 0; j < graph[n].Nodes.Length; j++)
							{
								if (!buffer.Contains(graph[n].Nodes[j]))
								{
									buffer[k + 1].Add(graph[n].Nodes[j]);
									i++;
								}
							}
						}

						buffer[k + 2].Clear();

						foreach (var n in buffer[k + 1])
						{
							for (var j = 0; j < graph[n].Nodes.Length; j++)
							{
								if (!buffer.Contains(graph[n].Nodes[j]))
								{
									buffer[k + 2].Add(graph[n].Nodes[j]);
									i++;
								}
							}
						}
					}

					var currentBuffer = buffer[k];

					// за итерацию проходим 2 уровня
					// последний уровень может быть пустым
					if (buffer[k].Count == 0)
					{
						currentBuffer = buffer[k + 2];
						k--;
					}

					// каждый новый качественный уровень не ухудшает предыдущие
					foreach (var n in currentBuffer)
					{
						nodes[q + 1].Add(n);
					}
				}
			}

			return nodes[q + 1].ToArray();
		}

		private class Buffer
		{
			private readonly HashSet<int>[] _data;

			public Buffer(int capacity)
			{
				_data = new HashSet<int>[capacity];

				for (var i = 0; i < capacity; i++)
				{
					_data[i] = new HashSet<int>();
				}
			}

			public HashSet<int> this[int d] => _data[d % _data.Length];

			public void ClearAll()
			{
				for (var i = 0; i < _data.Length; i++)
				{
					_data[i].Clear();
				}
			}

			public bool Contains(int n)
			{
				for (var i = 0; i < _data.Length; i++)
				{
					if (_data[i].Contains(n))
					{
						return true;
					}
				}

				return false;
			}
		}
    }
}

