using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KBCC.Models
{
    public class CustomerViewModels
    {
    }
    public class Order
    {
        [Key]
        public long Id { get; set; }
        public string CustomId { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 0: Pending  | 1: Registed Barcodes  | 2: Disposed | -1: Deleted
        /// </summary>
        public int Status { get; set; }
        //Customer code
        public string Customer { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerLocation { get; set; }
        // Tag code
        public string Tags { get; set; }
        public string InternalNote { get; set; }
        // Product code
        public string Product { get; set; }
        public int TotalPrice { get; set; }
        public int CODPrice { get; set; }
        public string TransportCompany { get; set; }
        public string CreatedIP { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public string CustomId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long Weight { get; set; }
        public string Image { get; set; }

        //Mua vào
        public long BidPrice { get; set; }
        //Bán ra
        public long AskPrice { get; set; }
        //Phí vận chuyển
        public long Shipping { get; set; }

        public bool IsActive { get; set; }

        //public string Properties { get; set; }

        public string CreatedIP { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
    public class Depot
    {
        [Key]
        public long Id { get; set; }
        public string CustomId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }

        public string CreatedIP { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
    public class Stock
    {
        [Key]
        public long Id { get; set; }
        public string CustomId { get; set; }
        public string Depot { get; set; }
        public string Product { get; set; }
        public long NewBidPrice { get; set; }
        public long Quantity { get; set; }
        public string Location { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }

        public string CreatedIP { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        public string CustomId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public string Location { get; set; }

        public bool IsActive { get; set; }

        public string CreatedIP { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
    public class Tag
    {
        [Key]
        public long Id { get; set; }
        public string CustomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Icon { get; set; }
        public string BackgroundColor { get; set; }
        public string Color { get; set; }

        public bool IsActive { get; set; }

        public string CreatedIP { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}