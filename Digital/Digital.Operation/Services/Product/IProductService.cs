using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Operation.Base;
using Digital.Schema.Dto.Product;
using Digital.Schema.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public interface IProductService : IBaseService<Product, ProductRequest, ProductResponse>
    {
        public ApiResponse StockUpdate(int id, int stock);

        public ApiResponse<List<ProductResponse>> GetAllProduct(bool isActive);


    }
}
