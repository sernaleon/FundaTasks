using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Core
{
    public interface IUser
    {
        Task AddUserAsync(User user, CancellationToken token);
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken token);
    }
}
