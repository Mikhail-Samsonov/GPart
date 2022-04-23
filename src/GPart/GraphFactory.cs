using System.Collections.Generic;
using System.IO;
using System.Linq;
using GPart.Algorithms.Contracts.Exceptions;
using GPart.Algorithms.Contracts.Models;

namespace GPart
{
    /// <summary>
    /// Фабрика создания графа
    /// </summary>
	public static class GraphFactory
    {
        public const char DelimiterDefault = ' ';

        /// <summary>
        /// Загрузить граф из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="delimiter">Разделитель смежных вершин в файле</param>
        /// <returns><see cref="IGraph"/></returns>
        /// <exception cref="FileNotFoundException">Не найден файл</exception>
        /// <exception cref="InvalidFileException">Содержимое файла некорректно</exception>
        public static IGraph LoadFrom(string path, char delimiter = DelimiterDefault)
        {
            using var reader = File.OpenText(path);

            if (reader is null)
            {
                throw new FileNotFoundException();
            }

            if (reader.EndOfStream)
            {
                throw new InvalidFileException();
            }

            var tokens = (reader.ReadLine() ?? throw new InvalidFileException()).Split(delimiter);

            if (tokens.Length < 2)
            {
                throw new InvalidFileException();
            }

            if (!int.TryParse(tokens[0], out int totalNodes))
            {
                throw new InvalidFileException();
            }

            if (!int.TryParse(tokens[1], out int totalEdges))
            {
                throw new InvalidFileException();
            }

            var nodes = new Node[totalNodes];

            for (var i = 0; i < totalNodes; i++)
            {
                if (reader.EndOfStream)
                {
                    throw new InvalidFileException();
                }

                tokens = (reader.ReadLine() ?? throw new InvalidFileException()).Trim().Split(delimiter);
                nodes[i] = new Node(new int[tokens.Length], 1);

                for (var j = 0; j < tokens.Length; j++)
                {
                    if (!int.TryParse(tokens[j], out int node))
                    {
                        throw new InvalidFileException();
                    }

                    nodes[i].Nodes[j] = node - 1;
                }
            }

            return new Graph(nodes, totalEdges);
        }

        /// <summary>
        /// Создать граф по <see cref="Solution"/>
        /// </summary>
        /// <param name="sln"><see cref="Solution"/></param>
        /// <returns><see cref="IGraph"/></returns>
        public static IGraph CreateFrom(Solution sln)
        {
            var nodes = new Node[sln.KSection];
            var buffer = new HashSet<int>();

            for (int n = 0, w; n < sln.Graph.TotalNodes; n++)
            {
                if (nodes[sln[n]] is not null)
                {
                    continue;
                }

                buffer.Clear();
                w = sln.Graph[n].Weight;

                for (var a = 0; a < sln.Graph[n].Nodes.Length; a++)
                {
                    if (sln[n] != sln[sln.Graph[n].Nodes[a]])
                    {
                        buffer.Add(sln[sln.Graph[n].Nodes[a]]);
                    }
                    else
                    {
                        w += sln.Graph[sln.Graph[n].Nodes[a]].Weight;

                        for (var k = 0; k < sln.Graph[sln.Graph[n].Nodes[a]].Nodes.Length; k++)
                        {
                            if (sln[n] != sln[sln.Graph[sln.Graph[n].Nodes[a]].Nodes[k]])
                            {
                                buffer.Add(sln[sln.Graph[sln.Graph[n].Nodes[a]].Nodes[k]]);
                            }
                        }
                    }
                }

                // todo можно обойтись без буфера - считаем колчичество уникальных, выделяем память и инициализируем. Как вариант можно использовать ArrayPool.Shared
                nodes[sln[n]] = new Node(buffer.ToArray(), w);
            }

            var totalEdges = CalculateTotalEdges(nodes);

            return new Graph(nodes, totalEdges);
        }

        /// <summary>
        /// Подсчитать количество ребер в графе
        /// </summary>
        /// <param name="nodes"><see cref="T:Node[]"/></param>
        /// <returns>Количество вершин</returns>
        private static int CalculateTotalEdges(Node[] nodes)
        {
            int q = 0;

            for (int n = 0; n < nodes.Length; n++)
            {
                q += nodes[n].Nodes.Length;
            }

            return q / 2;
        }
    }
}

