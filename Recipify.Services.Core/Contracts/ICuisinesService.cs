using Recipify.Web.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Services.Core.Contracts
{
    public interface ICuisinesService
    {
        Task<IEnumerable<CuisinesDropDownModel>> GetAllCuisinesDropDownAsync();

    }
}
