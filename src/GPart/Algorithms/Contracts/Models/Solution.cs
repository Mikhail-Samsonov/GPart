namespace GPart.Algorithms.Contracts.Models
{
    /// <summary>
    /// Решение.
    /// </summary>
    public class Solution
    {
        public const int EmptyMarker = 0;
        private readonly int[] _sln;

        /// <summary>
        /// Создать решение.
        /// </summary>
        /// <param name="graph">Граф решения</param>
        /// <param name="kSection">Количество подграфов</param>
        public Solution(IGraph graph, int kSection)
        {
            _sln = new int[graph.TotalNodes];
            Graph = graph;
            KSection = kSection;
        }

        /// <summary>
        /// Создать решение.
        /// </summary>
        /// <param name="graph">Граф решения</param>
        /// <param name="kSection">Количество подграфов</param>
        /// <param name="sln"><see cref="Solution"/></param>
        public Solution(IGraph graph, int kSection, int[] sln)
        {
            _sln = sln;
            Graph = graph;
            KSection = kSection;
        }

        /// <summary>
        /// Объединить текущее решение с новым.
        /// </summary>
        /// <param name="src">Новое решение.</param>
        public void Merge(Solution src)
        {
            for (int i = 0; i < _sln.Length; i++)
            {
                _sln[i] = src[_sln[i]];
            }

            KSection = src.KSection;
        }

        /// <summary>
        /// Идентификатор подграфа.
        /// </summary>
        /// <param name="i">Индекс вершины</param>
        /// <returns>Идентификатор подграфа</returns>
        public int this[int i]
        {
            get => _sln[i];
            set => _sln[i] = value;
        }

        /// <summary>
        /// <see cref="IGraph"/>
        /// </summary>
        public IGraph Graph { get; }

        /// <summary>
        /// Количество подграфов
        /// </summary>
        public int KSection { get; private set; }

        /// <summary>
        /// Сечение графа.
        /// </summary>
        public int CrossPower
        {
            get
            {
                int q = 0;

                for (int n = 0; n < Graph.TotalNodes; n++)
                {
                    for (var a = 0; a < Graph[n].Nodes.Length; a++)
                    {
                        if (_sln[n] != _sln[Graph[n].Nodes[a]])
                        {
                            q++;
                        }
                    }
                }

                return q / 2;
            }
        }
    }
}

