using Fluent.Infrastructure.FluentModel;
using ProjectIO.model;


public class WorkerRepository : IRepository<Worker>
{
    private readonly ApplicationDbContext _context;

    public WorkerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Worker> GetByIdAsync(int id)
    {
        return await _context.Workers.FindAsync(id);
    }

    public async Task<IEnumerable<Worker>> GetAllAsync()
    {
        return await _context.Workers.ToListAsync();
    }

    public async Task AddAsync(Worker worker)
    {
        await _context.Workers.AddAsync(worker);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Worker worker)
    {
        _context.Workers.Update(worker);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var worker = await GetByIdAsync(id);
        if (worker != null)
        {
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
        }
    }
}
