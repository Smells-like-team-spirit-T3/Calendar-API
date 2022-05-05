using CalendarAPI.Models.Interfaces;

namespace CalendarAPI.Models
{
    public interface IUnitOfWork
    {
        IEventRepository Events { get; }
        ITripRepository Trips { get; }

        void Save();
    }
}