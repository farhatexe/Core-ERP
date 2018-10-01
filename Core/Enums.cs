using System;
namespace Core
{
    public class Enums
    {
        /// <summary>
        /// Status.
        /// </summary>
        public enum Status {
            /// <summary>
            /// The pending.
            /// </summary>
            Pending = 1,
            /// <summary>
            /// The approved.
            /// </summary>
            Approved = 2,
            /// <summary>
            /// The annulled.
            /// </summary>
            Annulled = 3,
        }

        /// <summary>
        /// Order types.
        /// </summary>
        public enum OrderTypes {
            /// <summary>
            /// The sales orders.
            /// </summary>
            SalesOrders = 1,
            /// <summary>
            /// The sales invoice.
            /// </summary>
            SalesInvoice = 2,
            /// <summary>
            /// The purchase order.
            /// </summary>
            PurchaseOrder = 3,
            /// <summary>
            /// The purchase invoice.
            /// </summary>
            PurchaseInvoice = 4,
        }

        /// <summary>
        /// Item types.
        /// </summary>
        public enum ItemTypes{
            /// <summary>
            /// The stockable.
            /// </summary>
            Stockable = 1, 
            /// <summary>
            /// The service.
            /// </summary>
            Service = 2, 
            /// <summary>
            /// The made to order.
            /// </summary>
            MadeToOrder = 3
        }

        /// <summary>
        /// RowStatus
        /// </summary>
        public enum Action
        {
            CreatedOnCloud = 1,
            UpdatedOnCloud = 2,
            CreateOnLocal = 3,
            UpdatedOnLocal = 4,
            NoChanges = 5


        }
    }
}
