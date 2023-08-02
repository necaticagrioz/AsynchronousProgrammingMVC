using AsynchronousProgrammingMVC.Models.Entities.Abstract;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AsynchronousProgrammingMVC.Infrastructure.Repositories.Interfaces
{
    /*
        Asenkron Programlama 
        Bu güne kadar yaptığımız çalışmalarda senkron programlama(eş zamanlı programlama) yapıyorduk. Bu yüzden bir iş(business) yapıldığında kullanıcı arayüzü (UI - UserInterface) sadece yapılan bu işe bütün eforunu sarf etmekteydi. Örneğin bir web servisten data çekmek istiyorsunuz ve request attınız. Response olarak genel data'nın listelenemesi işlemine UI thread'i kitledi. Böylelikle kullanıcı uygulamanın ona yan tarafta verdiği nopt tutma bölümünü kullanamaz hale geldi. Senkron programnlam burada yetersiz kaldı. Bizim problemimizi yani data listeleneirken arayüz üzerinde not tutma işini asenkron programalama ile yapabiliriz. Asenkron programlama aynı anda bir birinden bağımsız olarak işlemler yapmamızı temin edecektir. 
     */

    public interface IBaseRepository<T> where T : BaseEntity
    {
        //Bu projede asenkron programlamaya alışmanız için bütün methodları asenkron kullanacağız. Lakin Create, Update ve Delete işlemleri çok aksi bir business olmadığı müddetçe asenkron programlanmaz. Bu gerek yoktur. Bunun yanında bizim asıl odaklanmamız gereken nokta READ operasyonlarıdır. 

        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

        //READ Operations
        //_categoyRepo.GetByDefaults(x => x.Status != Status.Passive)
        Task<List<T>> GetByDefaults(Expression<Func<T, bool>> expression);
        Task<T> GetByDefault(Expression<Func<T, bool>> expression);

        //Repository Design Pattern GetFilteredList
        Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select,
                                                     Expression<Func<T, bool>> where = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                     Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null
                                                     );
        Task<T> GetById(int id);
        Task<bool> Any(Expression<Func<T, bool>> expression);
    }
}
