using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core.Controllers
{
    public class DocumentController
    {
        private Context _db;

        public DocumentController(Context db)
        {
            _db = db;
        }

        public ObservableCollection<Document> List()
        {
            _db.Documents.Load();
            return _db.Documents.Local.ToObservableCollection();
        }

        public void Add(Document document)
        {
            _db.Documents.Add(document);
        }

        public void Delete(Document document)
        {
            _db.Documents.Remove(document);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public string Sales(Models.Order Order)
        {
            Models.Document document = Order.range.document;
            string design = document.designUrl;

            if (document.designUrl != "" && document.designUrl != null)
            {
                if (Order.company != null)
                {
                    design = Replace(design, "@companyName", Order.company.name);
                    design = Replace(design, "@companyTaxId", Order.company.taxId);
                    design = Replace(design, "@companyAddress", Order.company.address);
                }

                if (Order.customer != null)
                {
                    design = Replace(design, "@customerName", Order.customer.alias);
                    design = Replace(design, "@customerTaxId", Order.customer.taxId);
                    design = Replace(design, "@customerAddress", Order.customer.address);
                }

                if (Order.location != null)
                {
                    design = Replace(design, "@locationName", Order.location.name);
                    design = Replace(design, "@locationAddress", Order.location.address);
                }

                design = Replace(design, "@paymentContract", (Order.paymentContract != null ? Order.paymentContract.name : design = "Cash"));
                design = Replace(design, "@currencyName", Order.currency);
                design = Replace(design, "@currencyRate", Order.currencyRate.ToString());

                design = Replace(design, "@invoiceNumber", Order.invoiceNumber);
                design = Replace(design, "@invoiceDate", Order.date.ToShortDateString());
                design = Replace(design, "@invoiceDTime", Order.date.ToShortDateString() + " " + Order.date.ToShortTimeString());

                if (design.Contains("@invoiceDetails"))
                {
                    string details = "";
                    foreach (Models.OrderDetail detail in Order.details)
                    {
                        string itemDescription = detail.item != null ? detail.item.name : detail.itemDescription;
                        string itemCode = detail.item != null ? "/t" + detail.item.sku : "";

                        details = details +
                        "/n" + itemCode + itemDescription +
                        "/n" + Math.Round(detail.quantity, 2).ToString() + "/t" + Math.Round(detail.subTotalVat, 2).ToString();
                    }

                    design = Replace(design, "@invoiceDetails", details);
                }

                design = Replace(design, "@invoiceTotal", Math.Round(Order.details.Sum(x => x.subTotal), 2).ToString());
                design = Replace(design, "@invoiceVatTotal", Math.Round(Order.details.Sum(x => x.subTotalVat), 2).ToString());

                if (design.Contains("@invoiceVatDetails"))
                {
                    //loop for vat name and value
                }

                if (design.Contains("@includePromo"))
                {
                    //include code for discount on next purchase. this case you must create a new coupon or reference an existing coupon.
                }

            }

            return design;
        }

        private string Replace(string body, string key, string value)
        {
            return body.Contains(key) ? body.Replace(key, value) : body;
        }
    }
}
