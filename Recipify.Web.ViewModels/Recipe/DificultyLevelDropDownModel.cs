using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class DificultyLevelDropDownModel
    {
        public int Id { get; set; }
        public string levelName { get; set; } = null!;
    }
}
