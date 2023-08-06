using Digital.Data.Model;
using Digital.Operation.Base;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public interface ICategoryService : IBaseService<Category, CategoryRequest, CategoryResponse>
    {
    }
}
