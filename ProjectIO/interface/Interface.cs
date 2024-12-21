using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id); // Pobierz element po ID
    Task<IEnumerable<T>> GetAllAsync(); // Pobierz wszystkie elementy
    Task AddAsync(T entity); // Dodaj nowy element
    Task UpdateAsync(T entity); // Zaktualizuj istniejący element
    Task DeleteAsync(int id); // Usuń element po ID
}
