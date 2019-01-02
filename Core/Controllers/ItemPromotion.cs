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
                else if (promotion.inputType == Models.ItemPromotion.InputTypes.OnCustomer)
                {
                    if (order.customer != null && promotion.inputReference == order.customer.cloudId)
                    {
                        OnHeader(order, promotion);
                    }
                }
                else if (promotion.inputType == Models.ItemPromotion.InputTypes.OnPaymentType)
                {
                    if (order.paymentContract != null && promotion.inputReference == order.paymentContract.cloudId)
                    {
                        OnHeader(order, promotion);
                    }
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
            else if (promotion.outputType == Models.ItemPromotion.OutputTypes.DiscountOnQuantity
                && promotion.outputValue <= order.details.Sum(x => x.quantity))
            {
                foreach (OrderDetail detail in order.details)
                {
                    Discount(detail, promotion);
                }
            }
            else if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnQuantity
                && promotion.outputValue <= order.details.Sum(x => x.quantity))
            {
                foreach (OrderDetail detail in order.details)
                {
                    Gift(detail, promotion);
                }
            }
            else if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnTotal)
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
            else if (promotion.outputType == Models.ItemPromotion.OutputTypes.DiscountOnQuantity
                && promotion.outputValue <= detail.quantity)
            {
                Discount(detail, promotion);
            }
            else if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnQuantity
            && promotion.outputValue <= detail.quantity)
            {
                Gift(detail, promotion);
            }
            else if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnTotal)
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
            if (promotion.outputType == Models.ItemPromotion.OutputTypes.GiftOnQuantity)
            {

            }

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


        public void Upload(string slug)
        {
            Core.API.CognitivoAPI CognitivoAPI = new Core.API.CognitivoAPI();
            List<object> syncList = new List<object>();

            foreach (Core.Models.ItemPromotion itempromotion in _db.ItemPromotions.ToList())
            {
                Cognitivo.API.Models.ItemPromotion itempromotionModel = new Cognitivo.API.Models.ItemPromotion();

                itempromotionModel = UpdateData(itempromotionModel, itempromotion);
                syncList.Add(itempromotionModel);
            }

            List<object> ReturnItem = CognitivoAPI.UploadData(slug, "", syncList, Core.API.CognitivoAPI.Modules.Promotion);

            foreach (dynamic data in ReturnItem)
            {
                if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnLocal)
                {
                    int localId = (int)data.localId;
                    Models.ItemPromotion itempromotion = _db.ItemPromotions.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        itempromotion.updatedAt = Convert.ToDateTime(data.updatedAt);
                        itempromotion.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {
                        itempromotion.cloudId = data.cloudId;
                        itempromotion.name = data.name;
                        itempromotion.startDate= Convert.ToDateTime(data.startDate);
                        itempromotion.endDate = Convert.ToDateTime(data.endDate);
                        itempromotion.inputType = (Core.Models.ItemPromotion.InputTypes)data.inputType;
                        itempromotion.inputReference = data.inputReference;
                        itempromotion.inputValue = data.inputValue;
                        itempromotion.outputType = (Core.Models.ItemPromotion.OutputTypes)data.outputType;
                        itempromotion.outputReference = data.outputReference;
                        itempromotion.outputValue = data.outputValue;
                        itempromotion.updatedAt = Convert.ToDateTime(data.updatedAt);
                        itempromotion.updatedAt = itempromotion.updatedAt.Value.ToLocalTime();
                        itempromotion.createdAt = Convert.ToDateTime(data.createdAt);
                        itempromotion.createdAt = itempromotion.createdAt.Value.ToLocalTime();
                    }
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.CreateOnLocal)
                {
                    Models.ItemPromotion itempromotion = new Models.ItemPromotion();
                    itempromotion.cloudId = data.cloudId;
                    itempromotion.name = data.name;
                    itempromotion.startDate = Convert.ToDateTime(data.startDate);
                    itempromotion.endDate = Convert.ToDateTime(data.endDate);
                    itempromotion.inputType =(Core.Models.ItemPromotion.InputTypes)data.inputType;
                    itempromotion.inputReference = data.inputReference;
                    itempromotion.inputValue = data.inputValue;
                    itempromotion.outputType = (Core.Models.ItemPromotion.OutputTypes)data.outputType;
                    itempromotion.outputReference = data.outputReference;
                    itempromotion.outputValue = data.outputValue;
                    itempromotion.updatedAt = Convert.ToDateTime(data.updatedAt);
                    itempromotion.updatedAt = itempromotion.updatedAt.Value.ToLocalTime();
                    itempromotion.createdAt = Convert.ToDateTime(data.createdAt);
                    itempromotion.createdAt = itempromotion.createdAt.Value.ToLocalTime();

                    _db.ItemPromotions.Add(itempromotion);
                }
                else if ((Cognitivo.API.Enums.Action)data.action == Cognitivo.API.Enums.Action.UpdateOnCloud)
                {
                    int localId = (int)data.localId;
                    Models.ItemPromotion itempromotion = _db.ItemPromotions.Where(x => x.localId == localId).FirstOrDefault();

                    if (data.deletedAt != null)
                    {
                        itempromotion.updatedAt = Convert.ToDateTime(data.updatedAt);
                        itempromotion.updatedAt = itempromotion.updatedAt.Value.ToLocalTime();
                        itempromotion.deletedAt = data.deletedAt != null ? Convert.ToDateTime(data.deletedAt) : null;
                    }
                    else
                    {

                        itempromotion.cloudId = data.cloudId;
                        itempromotion.updatedAt = Convert.ToDateTime(data.updatedAt);
                        itempromotion.updatedAt = itempromotion.updatedAt.Value.ToLocalTime();
                        itempromotion.createdAt = Convert.ToDateTime(data.createdAt);
                        itempromotion.createdAt = itempromotion.createdAt.Value.ToLocalTime();
                    }
                }
            }

            _db.SaveChanges();
        }
        public dynamic UpdateData(Cognitivo.API.Models.ItemPromotion ItemPromotion, Core.Models.ItemPromotion itempromotion)
        {
            ItemPromotion.updatedAt = itempromotion.updatedAt != null ? itempromotion.updatedAt.Value.ToUniversalTime() : itempromotion.createdAt.Value.ToUniversalTime();
            ItemPromotion.action = (Cognitivo.API.Enums.Action)itempromotion.action;
            ItemPromotion.cloudId = itempromotion.cloudId;
            ItemPromotion.createdAt = itempromotion.createdAt != null ? itempromotion.createdAt.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime();
            ItemPromotion.deletedAt = itempromotion.deletedAt != null ? itempromotion.deletedAt.Value.ToUniversalTime() : itempromotion.deletedAt;
            ItemPromotion.localId = itempromotion.localId;
            ItemPromotion.name = itempromotion.name;
            itempromotion.startDate = itempromotion.startDate != null? itempromotion.startDate.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime();
            itempromotion.endDate = itempromotion.endDate != null? itempromotion.endDate.Value.ToUniversalTime() : DateTime.Now.AddMonths(1).ToUniversalTime();
            itempromotion.inputType = itempromotion.inputType;
            itempromotion.inputReference = itempromotion.inputReference;
            itempromotion.inputValue = itempromotion.inputValue;
            itempromotion.outputType = itempromotion.outputType;
            itempromotion.outputReference = itempromotion.outputReference;
            itempromotion.outputValue = itempromotion.outputValue;
            return ItemPromotion;
        }
    }
}
