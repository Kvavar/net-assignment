namespace Work.Interfaces
{
    public interface IRepository<T, in TK>
    {
        void Create(T obj);
        T Read(TK key);        
        void Update(T obj);
        void Remove(TK key);
    }
}
