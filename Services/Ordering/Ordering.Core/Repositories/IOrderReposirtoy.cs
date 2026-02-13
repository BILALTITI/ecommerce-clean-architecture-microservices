
using Ordering.Core.Entites;
namespace Ordering.Core.Repositories
{
  public interface IOrderReposirtoy: IAsyncRepoistry<Entites.Order>
    {

        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}
