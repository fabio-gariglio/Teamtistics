using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Atlassian
{
    public interface IBacklogProvider
    {
        Task<IEnumerable<Item>> GetItemsAsync(string sprintName);
    }
}