﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveCommerce.Training.CartPersistence.Pipelines.PersistCart
{
    public class PersistToCustomer : IPersistCartProcessor
    {
        public const string CouponCodeKey = "AC Cart Coupon";
        public const string CartItemsKey = "AC Cart Items";

        public void Process(PersistCartArgs args)
        {
            var user = args.CustomerManager.CurrentUser;
            if (user == null || string.IsNullOrEmpty(user.NickName) || Sitecore.Context.Domain.IsAnonymousUser(user.NickName))
            {
                return;
            }
            user.CustomProperties[CouponCodeKey] = args.CouponCode;
            string cartItems = string.Empty;
            if (args.CartItems != null && args.CartItems.Any())
            {
                cartItems = string.Join(",", args.CartItems.Select(x => string.Format("{0}|{1}", x.Key, x.Value)));
            }
            user.CustomProperties[CartItemsKey] = cartItems;
            args.CustomerManager.UpdateCustomerProfile(user);
        }
    }
}