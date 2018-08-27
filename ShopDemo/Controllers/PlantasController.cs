using ShopDemo.BusinessLogic;
using ShopDemo.Models;
using ShopDemo.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace PaginaDemo.Controllers
{

    public class PlantasController : Controller
    {
        private Services Services { get; set; }

        //Inicio services y el carro.
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Services = new Services();

            //Si el carro actual no existe o es temporal y el usuario esta auetinticado, cargo su carro.
            if (User.Identity.IsAuthenticated && Services.IsTempOrNull((Cart)Session["Cart"]))
            {
                Session["Cart"] = Services.LoadCart((Cart)Session["Cart"], User.Identity.GetUserName());
            }
            //Si no se creo el carro y el usuario no esta autenticado, lo creo.
            else if ((Cart)Session["Cart"] == null)
            {
                Session["Cart"] = new Cart("Temp");
            }
            //Si ya se creo el carro y el usuario no existe, no hago nada.

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string type, int? ppp, int? page, string order = "Relevancia", string text = null)
        {
            var ViewModel = Services.CreateProductsIndexViewModel(ppp, page, type, order, text);
            return View(ViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(Services.GetProductById(id));
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Details(int id, int quantity)
        {
            Services.AddToCart(id, quantity, (Cart)Session["Cart"]);
            return RedirectToAction("Index", "Plantas");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Manage()
        {
            return View(Services.GetAllProducts());
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Product NewProduct, HttpPostedFileBase Image, bool hasImage = false)
        {
            if (ModelState.IsValid)
            {
                Services.CreateProduct(NewProduct, Image, hasImage);
                return RedirectToAction("Index");
            }
            return View(NewProduct);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(Services.GetProductById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Product EditedProduct, HttpPostedFileBase Image, bool hasImage = false)
        {
            if (ModelState.IsValid)
            {
                Services.EditProduct(EditedProduct, Image, hasImage);
                return RedirectToAction("Index", "Plantas");
            }
            return View(EditedProduct);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Services.RemoveProduct(id, (Cart)Session["Cart"]);
            return RedirectToAction("Manage", "Plantas");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Cart()
        {
            var cart = ((Cart)Session["Cart"]);
            var list = Services.ControlStock(cart);
            if (list.Count > 0)
            {
                ViewBag.Message = Services.CreateMessage(list);
            }
            ViewBag.TotalPrice = Services.TotalPrice(cart.CartItems);
            return View(cart.CartItems);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Cart(int? id)
        {
            var cart = ((Cart)Session["Cart"]);
            if (id != null) Services.RemoveFromCart(id.Value, cart);
            var list = Services.ControlStock(cart);
            if (list.Count > 0)
            {
                ViewBag.Message = Services.CreateMessage(list);
            }
            ViewBag.TotalPrice = Services.TotalPrice(cart.CartItems);
            return View(cart.CartItems);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult CheckOut()
        {
            var cart = ((Cart)Session["Cart"]);
            var list = Services.ControlStock(cart);
            if (list.Count > 0)
            {
                return RedirectToAction("Cart", "Plantas");
            }
            Services.CheckOut(cart);
            return View(cart.CartItems);
        }

    }
}