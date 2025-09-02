// ProjectManagerApi.Core/Enums/Priority.cs
namespace ProjectManagerApi.Core.Enums
{
    public enum Priority
    {
        /// <summary>
        /// Tarefa de baixa importância.
        /// </summary>
        Low = 1,

        /// <summary>
        /// Tarefa de importância média.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// Tarefa de alta importância.
        /// </summary>
        High = 3,

        /// <summary>
        /// Tarefa crítica, que exige atenção imediata.
        /// </summary>
        Critical = 4
    }
}