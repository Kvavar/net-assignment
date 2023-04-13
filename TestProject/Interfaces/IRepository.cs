namespace Work.Interfaces
{
    public interface IRepository<T,K>
    {
        void Create(T obj);
        T Read(K key);        
        void Update(T obj);
        void Remove(K key);
    }
}
