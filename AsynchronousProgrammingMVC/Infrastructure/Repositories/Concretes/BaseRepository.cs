using AsynchronousProgrammingMVC.Infrastructure.Context;
using AsynchronousProgrammingMVC.Infrastructure.Repositories.Interfaces;
using AsynchronousProgrammingMVC.Models.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AsynchronousProgrammingMVC.Infrastructure.Repositories.Concretes
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            //Dependency Injection
            //Eski çalışmalarımızda tam burada ApplicationDbContext.cs sınıfının nesnesini çıkarırdık. Bu örneklem alma işlem yüzünde respository sınıfları ile ApplicationDbContext.cs sınıf arasında sıkı sıkıya bağlı bir ilişki oluşmaktaydı. Ayrıca memory yönetimi açısından sıkı sıkıya bağlı sınıfların maliyet oluşturuduğunu ve RAM'in HEAP alanında yönetilemeyen kaynaklara neden olmaktadır. Sonuç olarak her sınıfın instance'ının çıkardığımızda nesneler yönetiminde projelerimiz büyüdükçe sıkıntılar yaşamaktayız. Bu yüzden projelerimizde bu tarz bağımlılıklara sebep olan sınıflar DIP prensiplerine de uymak için Dependency Injection deseni kullanılarak gevşek bağlı bir hale getirmek istiyoruz. 

            //Inject ederken 3 farklı yol izleyebilrizi
            // 1) Constructor Injection
            // 2) Custom Method Injection
            // 3) Property ile Injection

            //DI bir desendir bir prensib değil. Hatta DIP prenciplerini, uygulamamızda bize yardımcı olan bir araçtır. ASP .NET Core bu prensipleri projelerimizde rahatlıkla kullanmanız için dizayn edilmiştir.
            _context = context;
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            entity.Status = Status.Modified;
            entity.UpdatedDate = DateTime.Now;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            entity.Status = Status.Passive;
            entity.DeletedDate = DateTime.Now;
            _context.Update(entity);
            await _context.SaveChangesAsync();
                
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression) => await _context.Set<T>().AnyAsync(expression);



        public async Task<T> GetByDefault(Expression<Func<T, bool>> expression) => await _context.Set<T>().FirstOrDefaultAsync(expression);


        public async Task<List<T>> GetByDefaults(Expression<Func<T, bool>> expression) => await _context.Set<T>().Where(expression).ToListAsync();


        public async Task<T> GetById(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select, 
                                                            Expression<Func<T, bool>> where = null, 
                                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,                                                                                                           Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (join != null)
            {
                query = join(query);
            }

            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(select).ToListAsync();
            }

            else
            {
                return await query.Select(select).ToListAsync();
            }

        }

        
    }
}
