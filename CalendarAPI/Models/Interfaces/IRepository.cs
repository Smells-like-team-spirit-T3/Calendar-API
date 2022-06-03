using System.Collections.Generic;

namespace CalendarAPI.Models.Interfaces
{
    public interface IRepository<T, I> 
        where T : class 
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        IEnumerable<T> GetAll();
        T GetById(I id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}