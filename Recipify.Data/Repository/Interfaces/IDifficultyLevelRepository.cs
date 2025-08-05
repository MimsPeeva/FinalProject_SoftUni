using Recipify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Data.Repository.Interfaces
{
    public interface IDifficultyLevelRepository:IRepository<DifficultyLevel, Guid>, IAsyncRepository<DifficultyLevel, Guid>
    {
        
    }
}
