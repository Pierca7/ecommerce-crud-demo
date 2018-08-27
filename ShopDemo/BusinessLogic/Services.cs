using ShopDemo.DataAccess;
using ShopDemo.Models;
using ShopDemo.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ShopDemo.Models.ViewModels;
using System.Data.Entity;

namespace ShopDemo.BusinessLogic
{
    public class Services
    {
        private UnitOfWork uwork;

        public Services()
        {
            uwork = new UnitOfWork(new ApplicationDbContext());
        }

        //Devuelve el producto pedido.
        public Product GetProductById(int id)
        {
            var product = uwork.Products.Get(id);
            uwork.Complete();
            return product;
        }

        //Devuelve una lista con todos los productos existentes.
        public List<Product> GetAllProducts()
        {
            var products = uwork.Products.GetAll().ToList();
            uwork.Complete();
            return products;
        }

        //Crea un nuevo producto.
        public void CreateProduct(Product NewProduct, HttpPostedFileBase Image, bool hasImage = false)
        {
            SetImage(NewProduct, Image, hasImage);
            NewProduct.DateCreated = System.DateTime.Now;
            uwork.Products.Add(NewProduct);
            uwork.Complete();
        }

        //Edita el producto dado.
        public void EditProduct(Product EditedProduct, HttpPostedFileBase Image, bool hasImage = false)
        {
            var product = uwork.Products.Get(EditedProduct.ID);
            //Establece la imagen y la fecha de edicion.
            EditedProduct.DateCreated = System.DateTime.Now;
            var backupImg = product.Image;
            SetImage(EditedProduct, Image, hasImage);
            if (EditedProduct.Image == "/Images/not-found.png" && hasImage) EditedProduct.Image = backupImg;
            uwork.Products.UpdateProduct(EditedProduct);
            uwork.Complete();
        }

        //Elimina el producto dado.
        public void RemoveProduct(int id, Cart cart)
        {
            //Elimina el producto del carro guardado en la sesion.
            var TempProduct = cart.CartItems.FirstOrDefault(x => x.Product.ID == id);
            if (TempProduct != null) cart.CartItems.Remove(TempProduct);

            //Elimina el producto de todos los carros existentes.
            var carts = uwork.Carts.GetAll();
            foreach (var item in carts)
            {
                var items = item.CartItems;
                if (items != null)
                {
                    var p = items.FirstOrDefault(x => x.Product.ID == id);
                    if (p != null)
                    {
                        UpdateCartList(item);
                        items.Remove(p);
                    }
                }

            }
            uwork.Carts.RemoveAllCartItemsByProductId(id);

            //Elimina el producto de su tabla.
            var product = uwork.Products.Get(id);
            uwork.Products.Remove(product);
            uwork.Complete();
        }

        //Crea el ViewModel necesario para Plantas/Index
        public PlantasIndexViewModel CreateProductsIndexViewModel(int? ProductsPerPage, int? Page, string Type, string Order, string Text)
        {
            List<Product> ProductList;
            if (!String.IsNullOrEmpty(Type))
            {
                ProductList = new List<Product>(uwork.Products.GetProductsByType(Type));
            }
            else if (!String.IsNullOrEmpty(Text))
            {
                ProductList = new List<Product>(uwork.Products.GetProductsByName(Text));
            }
            else
            {
                ProductList = new List<Product>(uwork.Products.GetAll());
            }

            //Opciones de paginacion.
            int ppp = (ProductsPerPage == null) ? 6 : ProductsPerPage.Value;
            int page = (Page == null) ? 1 :
                ((Page.Value - 1) * ppp > ProductList.Count()) ? 1 : Page.Value;
            var ViewModel = new PlantasIndexViewModel(ProductList, ppp, page, Type, Order);

            return ViewModel;
        }

        //Evalua si el carrito dado no fue creado o es temporal.
        public bool IsTempOrNull(Cart cart)
        {
            if (cart == null || cart.Owner == "Temp") return true;
            else return false;
        }

        //Busca el carrito del usuario en la base de datos.
        public Cart LoadCart(Cart TempCart, string user)
        {
            Cart NewCart;
            NewCart = uwork.Carts.GetByOwner(user);
            if (NewCart == null)
            {
                NewCart = uwork.Carts.CreateCart(user);
            }
            AddItemsToCart(TempCart, NewCart.CartItems);
            return NewCart;
        }

        //Agrega un item al carrito.
        public void AddToCart(int id, int quantity, Cart cart)
        {
            AddItemsToCart(cart, new[] { new CartItem { Product = uwork.Products.Get(id), QuantityAdded = quantity } });
            UpdateCartList(cart);
            uwork.Complete();
        }

        //Elimina un item del carrito.
        public void RemoveFromCart(int id, Cart cart)
        {
            ((HashSet<CartItem>)cart.CartItems).RemoveWhere(x => x.Product.ID == id);
            UpdateCartList(cart);
            uwork.Complete();
        }

        //Evalua si existe suficiente stock al momento de la compra.
        public List<string> ControlStock(Cart cart)
        {
            List<string> OutOfStock = new List<string>();
            foreach (var item in cart.CartItems)
            {
                var id = item.Product.ID;
                var product = uwork.Products.Get(id);
                if (item.QuantityAdded > product.StockQuantity)
                {
                    OutOfStock.Add(item.Product.Name);
                }
            }
            return OutOfStock;
        }

        //Si falta stock de algun producto, crea el mensaje correspondiente.
        public string CreateMessage(List<string> OutOfStock)
        {
            uwork.Complete();
            string Message = "No existe suficiente stock de " + OutOfStock.First();
            for (int i = 1; i < OutOfStock.Count - 1; i++)
            {
                Message += ", " + OutOfStock.ElementAt(i);
            }
            if (OutOfStock.Count > 1) Message += " y " + OutOfStock.Last();
            Message += ", por favor remuevalos del carrito.";
            return Message;
        }

        //Actualiza el stock de los productos comprados y vacia el carrito
        public void CheckOut(Cart cart)
        {
            foreach (var item in cart.CartItems)
            {
                var EditedProduct = uwork.Products.Get(item.Product.ID);
                EditedProduct.StockQuantity -= item.QuantityAdded;
                uwork.Products.UpdateProduct(EditedProduct);
                if (!IsTempOrNull(cart))
                {
                    uwork.Carts.RemoveCartItem(item.CartItemID);
                }
            }
            cart.CartItems = new HashSet<CartItem>();
            if (!IsTempOrNull(cart))
            {
                UpdateCartList(cart);
            }
            uwork.Complete();
        }

        //Calcula el precio final.
        public decimal TotalPrice(IEnumerable<CartItem> items)
        {
            decimal total = 0;
            foreach (var item in items)
            {
                if (item.Product.DiscountPercent > 0)
                {
                    total += (item.Product.Price - (item.Product.Price * (decimal)item.Product.DiscountPercent) / 100) * item.QuantityAdded;
                }
                else
                {
                    total += item.Product.Price * item.QuantityAdded;
                }
            }
            return total;
        }


        /* -- Metodos auxiliares -- */


        //Crea el path de la imagen.
        private void SetImage(Product product, HttpPostedFileBase image, bool hasImage)
        {
            string path = "", fileName = "";
            if (image != null && image.ContentLength > 0 && hasImage)
            {
                fileName = Path.GetFileName(image.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/"), fileName);
                image.SaveAs(path);
                product.Image = "/Images/" + fileName;
            }
            else
            {
                product.Image = "/Images/not-found.png";
            }
        }

        //Evalua si los items ya existen en el carrito, o si son nuevos.
        private void AddItemsToCart(Cart cart, IEnumerable<CartItem> items)
        {
            if (cart != null)
            {
                foreach (var item in items)
                {
                    var product = cart.CartItems.FirstOrDefault(x => x.Product.ID == item.Product.ID);
                    if (product != null)
                    {
                        var StockAvailable = uwork.Products.Get(item.Product.ID).StockQuantity;
                        product.QuantityAdded += item.QuantityAdded;
                        if (product.QuantityAdded > StockAvailable) product.QuantityAdded = StockAvailable;
                    }
                    else
                    {
                        ((HashSet<CartItem>)cart.CartItems).Add(item);
                    }
                }
            }
        }

        //Si el carrito no es temporal, lo actualiza en la base de datos.
        private void UpdateCartList(Cart cart)
        {
            if (cart.Owner != "Temp")
            {
                uwork.Carts.UpdateCartItems(cart);
            }
        }
    }
}