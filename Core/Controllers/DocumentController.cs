using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Controllers
{
    public class DocumentController
    {
        public string salesdata(Models.Order Order)
        {
            string Header = string.Empty;
            string Detail = string.Empty;
            string Footer = string.Empty;
            string BranchName = string.Empty;
            string TerminalName = string.Empty;
            string BranchAddress = string.Empty;
            Core.Models.Company company = null;

            if (Order.company != null)
            {
                company = Order.company;
            }


            if (Order.location != null)
            {
                BranchName = Order.location.name;
                BranchAddress = Order.location.address;
            }

            string ContractName = "";

            if (Order.paymentContract != null)
            {
                ContractName = Order.paymentContract.name;
            }


            string CurrencyName = "";
            if (Order.currency != null)
            {
                CurrencyName = Order.currency;
            }

            string TransNumber = Order.invoiceNumber;
            DateTime TransDate = Order.date;

            Header =
                company.name + "\n"
                + "RUC:" + company.taxId + "\n"
                + company.address + "\n"
                + "***" + company.name + "***" + "\n"
                + "-------------------------------- \n"
                + "Descripcion, Cantiad, Precio" + "\n"
                + "--------------------------------" + "\n"
                + "\n";

            foreach (Core.Models.OrderDetail d in Order.details)
            {
                string ItemName = d.item.name;
                string ItemCode = d.item.barCode;
                decimal? Qty = Math.Round(d.quantity, 2);
                string TaskName = d.itemDescription;

                Detail = Detail + (string.IsNullOrEmpty(Detail) ? "\n" : "")
                    + ItemName + "\n"
                    + Qty.ToString() + "\t" + ItemCode + "\t" + Math.Round((d.subTotalVat), 2) + "\n";
            }

            Footer = "--------------------------------" + "\n";
            Footer += "Total Bruto       : " + Math.Round((Order.total), 2) + "\n";
            Footer += "Fecha & Hora      : " + Order.date + "\n";
            Footer += "Numero de Factura : " + Order.invoiceNumber + "\n";
            Footer += "-------------------------------" + "\n";

            Footer += "------------------------------- \n";
            Footer += "Cliente    : " + Order.customer ?? Order.customer.alias + "\n";
            Footer += "Documento  : " + Order.customer ?? Order.customer.taxId + "\n";
            Footer += "------------------------------- \n";
            Footer += "Sucursal   : " + BranchName + "\n";
            Footer += "Sucursal Address  : " + BranchAddress + "\n";
            Footer += "Terminal   : " + TerminalName;

            Footer += "\n";

            string Text = Header + Detail + Footer;
            return Text;
        }
    }
}
