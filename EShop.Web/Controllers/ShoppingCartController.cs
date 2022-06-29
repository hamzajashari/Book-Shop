﻿using EShop.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Stripe;

namespace EShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [Route("/cart")]
        public IActionResult Index()
        {
            //prevzemanje na site info od najaveniot korisnik

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
               return RedirectToAction("Login", "Account");
            }
            else { 
           
            return View(this._shoppingCartService.getShoppingCartInfo(userId));
            }
        }
        [Route("/payment/pay")]
        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shoppingCartService.getShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "Eshop Application Payment",
                Currency = "usd",
                Customer = customer.Id
            }); 
            if(charge.Status == "succeeded")
            {
                var result = this.OrderNow();

                if (result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }


            return RedirectToAction("Index", "ShoppingCart");
        }
        public IActionResult DeleteProductFromShoppingCart(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result=this._shoppingCartService.deleteProductFromShoppingCart(userId, id);
            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");

            }
        }
        private Boolean OrderNow()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._shoppingCartService.orderNow(userId);

            return result;
        }
        public IActionResult Order()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.orderNow(userId);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
        }
    }
    }
