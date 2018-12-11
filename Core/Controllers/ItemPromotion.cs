using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Core.Controllers
{

    public class ItemPromotion
    {
        private Context _db;
        public ItemPromotion(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Models.ItemPromotion> List()
        {
            _db.ItemPromotions.Load();
            return _db.ItemPromotions.Local.ToObservableCollection();
        }

        public void Add(Models.ItemPromotion Entity)
        {
            _db.ItemPromotions.Add(Entity);
        }

        public void Delete(Models.ItemPromotion Entity)
        {
            _db.ItemPromotions.Remove(Entity);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public Models.Order CalculatePromotionsOnSales(Models.Order order)
        {
            List<Models.ItemPromotion> promotions = _db.ItemPromotions
            .Where(x => (x.startDate <= DateTime.Now && x.endDate >= DateTime.Now)
                ||
                (x.endDate == null)
                )
                .Where(x => x.onLocationCloudId == order.location.cloudId || x.onLocationCloudId == null)
                .ToList();


            foreach (var promotion in promotions)
            {
                if (promotion.inputType == Models.ItemPromotion.InputTypes.General)
                {
                    OnHeader(order, promotion);
                }

                foreach (OrderDetail detail in order.details)
                {
                    if (promotion.inputType == Models.ItemPromotion.InputTypes.OnProduct
                        &&
                        promotion.inputReference == detail.item.cloudId)
                    {
                        OnDetail(detail, promotion);
                    }
                    else if (promotion.inputType == Models.ItemPromotion.InputTypes.OnCategory
                            &&
                            promotion.inputReference == detail.item.categoryCloudId)
                    {
                        OnDetail(detail, promotion);
                    }
                    else if (promotion.inputType == Models.ItemPromotion.InputTypes.OnTag)
                    {
                        //Implementation Missing
                    }
                }
            }

            return order;
        }

        private void OnHeader(Models.Order order, Models.ItemPromotion promotion)
        {
            if (promotion.outputType == Models.ItemPromotion.OutputTypes.Discount)
            {
                foreach (OrderDetail detail in order.details)
                {
                    Discount(detail, promotion);
                }
            }

            if (promotion.outputType == Models.ItemPromotion.OutputTypes.DiscountOnQuantity
                && promotion.outputValue <= order.details.Sum(x => x.quantity))
            {
                foreach (OrderDetail detail in order.details)
                {
                    Discount(detail, promotion);
                }
            }

            if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnQuantity
                && promotion.outputValue <= order.details.Sum(x => x.quantity))
            {
                foreach (OrderDetail detail in order.details)
                {
                    Gift(detail, promotion);
                }
            }

            if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnTotal)
            {
                foreach (OrderDetail detail in order.details)
                {
                    Gift(detail, promotion);
                }
            }
        }

        private void OnDetail(Models.OrderDetail detail, Models.ItemPromotion promotion)
        {
            if (promotion.outputType == Models.ItemPromotion.OutputTypes.Discount)
            {
                Discount(detail, promotion);
            }

            if (promotion.outputType == Models.ItemPromotion.OutputTypes.DiscountOnQuantity
                && promotion.outputValue <= detail.quantity)
            {
                Discount(detail, promotion);
            }

            if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnQuantity)
            {
                Gift(detail, promotion);
            }

            if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnTotal)
            {
                Gift(detail, promotion);
            }
        }

        private void Discount(OrderDetail detail, Models.ItemPromotion promotion)
        {
            //Discount
            decimal originalPrice = detail.price + detail.discount;
            decimal newPrice = (originalPrice * (1 - promotion.outputValue));

            if (newPrice < originalPrice)
            {
                detail.price = newPrice;
                detail.promotion = promotion;
            }
        }

        private void Gift(OrderDetail detail, Models.ItemPromotion promotion)
        {
            Models.Item item = _db.Items.First(x => x.cloudId == promotion.outputReference);

            if (item != null)
            {
                detail.promotion = promotion;

                OrderDetail giftDetail = new OrderDetail()
                {
                    item = item,
                    quantity = promotion.outputValue,
                    price = 0,
                    promotion = detail.promotion,
                    order = detail.order
                };

                detail.order.details.Add(giftDetail);
            }
        }
    }
}
