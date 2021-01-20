using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PRODUCT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PRODUCT.Repository
{
    public class ProductConcrete : IProduct
    {
        private readonly IConfiguration _config;   // Perform Connection String From Startup File
       

     //   string ConnectionString = "Server=localhost; user=root; password=Path@123; Database=product_dotnetcore";

        public ProductConcrete(IConfiguration config) // Create Constructor for Repository Class 
        {
            this._config = config;
        }

        public IDbConnection Connection // Define Connection String For Getting DataBase
        {
            get
            {
                return new MySqlConnection(_config.GetConnectionString("MyConnectionString"));
            }
        }

        public void DeleteProduct(int productId) // For Delete Product
        {
            using var con = Connection;
       //   using var con = new MySqlConnection(ConnectionString);
            var param = new DynamicParameters();
            param.Add("p_productId", productId);
            var result = con.Execute("Usp_Delete_Product", param, null, 0, CommandType.StoredProcedure);
        }

        public Product GetProductByProductId(int productId) // Get Single Product
        {
            using var con = Connection;
            //  using var con = new MySqlConnection(ConnectionString);
            var param = new DynamicParameters();
            param.Add("p_productId", productId);
            return con.Query<Product>("Usp_Get_Productby_ProductId", param, null, true, 0, CommandType.StoredProcedure).FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts() // Get List Of Product
        {
            using var con = Connection;
            //  using var con = new MySqlConnection(ConnectionString);
            return con.Query<Product>("Usp_GetAll_Products", null, null, true, 0, CommandType.StoredProcedure).ToList();
        }


        public void InsertProduct(ProductVm product) // For Insert Product 
        {
            try
            {
                using var con = Connection;
                //  using var con = new MySqlConnection(ConnectionString);
                con.Open();
                var transaction = con.BeginTransaction();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("Name", product.Name);
                    param.Add("Quantity", product.Quantity);
                    param.Add("Color", product.Color);
                    param.Add("Price", product.Price);
                    param.Add("ProductCode", product.ProductCode);
                    var result = con.Execute("Usp_Insert_Product", param, transaction, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateProduct(Product product)  // For Update Case
        {
            try
            {
                using var con = Connection;
                //   using var con = new MySqlConnection(ConnectionString);
                con.Open();
                var transaction = con.BeginTransaction();
                try //Usp_Insert_Product
                {
                    var param = new DynamicParameters();
                    param.Add("p_productId", product.ProductId);
                    param.Add("n_name", product.Name);
                    param.Add("q_quantity", product.Quantity);
                    param.Add("c_color", product.Color);
                    param.Add("p_price", product.Price);
                    param.Add("p_productCode", product.ProductCode);
                    var result = con.Execute("Usp_Update_Product", param, transaction, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckProductExists(int productId)
        {
            using var con = Connection;
            //   using var con = new MySqlConnection(ConnectionString);
            var param = new DynamicParameters();
            param.Add("p_productId", productId);
            var result = con.Query<bool>("Usp_CheckProductExists", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
            return result;
        }
    }
}
