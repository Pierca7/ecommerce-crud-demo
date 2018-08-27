using PagedList;
using ShopDemo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDemo.Models.ViewModels
{
    public class PlantasIndexViewModel
    {
        public PagedList<Product> ProductList { get; set; }
        public string Type { get; set; }
        public string Order { get; set; }
        public string[] OrderList { get; set; }

        public PlantasIndexViewModel(List<Product> ProductList, int ProductsPerPage, int Page, string Type, string Order)
        {
            List<Product> lista = ProductList;
            this.Type = Type;
            this.Order = Order;
            this.OrderList = new string[] { "Relevancia", "Oferta", "Menor_Precio", "Mayor_Precio", "Stock" };
            switch (Order)
            {
                default:
                    lista = ProductList.OrderByDescending(x => x.DateCreated).ToList();
                    break;
                case "Relevancia":
                    lista = ProductList.OrderByDescending(x => x.DateCreated).ToList();
                    break;
                case "Oferta":
                    lista = ProductList.OrderByDescending(x => x.DiscountPercent).ToList();
                    break;
                case "Menor_Precio":
                    lista = ProductList.OrderBy(x => x.Price).ToList();
                    break;
                case "Mayor_Precio":
                    lista = ProductList.OrderByDescending(x => x.Price).ToList();
                    break;
                case "Stock":
                    lista = ProductList.OrderByDescending(x => x.Stock).ToList();
                    break;
            }
            this.ProductList = new PagedList<Product>(lista, Page, ProductsPerPage);
        }
    }
}