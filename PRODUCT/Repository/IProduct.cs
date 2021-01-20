using PRODUCT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRODUCT.Repository
{
    public interface IProduct
    {
        void InsertProduct(ProductVm product); // Create Product 

        IEnumerable<Product> GetProducts();

        Product GetProductByProductId(int productId);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);

        bool CheckProductExists(int productId);
    }
}
