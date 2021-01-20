using Microsoft.AspNetCore.Mvc;
using PRODUCT.Models;
using PRODUCT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRODUCT.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _product;

        //Constructor injection get an instance of ProductConcrete class.
        public ProductController(IProduct product) 
        {
            this._product = product;
        }

        [HttpGet] // Product/Index
        public IActionResult Index() 
        {
            return View(_product.GetProducts());
        }

        [HttpGet] // Product/Details/3
        public IActionResult Details(int? id)
        {
          if(id == null)
            {
                return RedirectToAction("Index");
            }
            var product = _product.GetProductByProductId(Convert.ToInt32(id));
            if(product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] // Bind Attribute to Protect from OverPosting Attacks.
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Quantity,Color,Price,ProductCode")] ProductVm product)
        {
            if(ModelState.IsValid)
            {
                _product.InsertProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var product = _product.GetProductByProductId(Convert.ToInt32(id));
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,Name,Quantity,Color,Price,ProductCode")] Product product)
        {
            if(id != product.ProductId)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                try
                {
                  if(_product.CheckProductExists(id))
                    {
                        _product.UpdateProduct(product);
                    }
                  else
                    {
                        ModelState.AddModelError("","Product Does Not Exit");
                        return View(product);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var product = _product.GetProductByProductId(Convert.ToInt32(id));
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost , ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _product.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
