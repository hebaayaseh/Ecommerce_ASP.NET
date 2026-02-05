using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.AddOrderItem;
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Invoice;
using Ecommerce_ASP.NET.DTOs.Order;
using Ecommerce_ASP.NET.DTOs.Payment;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Linq.Expressions;
using System.Net;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Transaction = System.Transactions.Transaction;

namespace Ecommerce_ASP.NET.Manager
{
    public class OrderManager
    {
        private readonly AppDbContext dbContext;
        private readonly CartManager cartManager;
        private readonly PaymentManager processPayment;
        private readonly DiscountManager discountManager;
        public OrderManager(AppDbContext dbContext, CartManager cartManager, PaymentManager processPayment, DiscountManager discountManager)
        {
            this.dbContext = dbContext;
            this.cartManager = cartManager;
            this.processPayment = processPayment;
            this.discountManager = discountManager;
        }
        public void Checkout(CartDto cartDto,int userId , AddressDto addressDto,PaymentDto paymentDto,DiscountUserDto discountDto)
        {
            
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                
                var cartItems = dbContext.CartItems
                .Include(ci => ci.product)
                .Where(ci => ci.UserId == userId)
                .ToList();

                if (!cartItems.Any())
                    throw new Exception("Cart is empty!");

                decimal totalAmount = 0;
                foreach (var item in cartItems)
                {
                    if (cartDto.quantity <= 0)
                        throw new Exception("Invalid quantity!");
                    if (item.product.stock <= 0)
                    {
                        item.product.stock = 0;
                        throw new Exception("You Cant Pay It!");

                    }
                    if (item.product.stock < cartDto.quantity)
                        throw new Exception($"Not enough stock for {item.product.name}");

                    totalAmount += (item.product.price * item.quantity);
                }
                var Address = new Address
                {
                    Street = addressDto.street,
                    City = addressDto.city,
                    PostalCode = addressDto.postalCode,
                    building = addressDto.building,
                    PhoneNumber = addressDto.PhoneNumber,
                    userId = userId,
                };
                dbContext.addresses.Add(Address);
                dbContext.SaveChanges();
                var id=0;
                decimal finalAmount = totalAmount;
                if (discountDto != null && discountManager.checkvalidCode(discountDto))
                {
                    var discountAdmin = dbContext.discounts.FirstOrDefault(d => d.Code == discountDto.code);
                     
                    if (discountAdmin != null)
                    {
                        id = discountAdmin.Id;
                        if (discountDto.discountType == DiscountType.Percentage)
                        {
                            finalAmount -= (totalAmount * (discountAdmin.DiscountValue / 100));
                        }
                        else if (discountDto.discountType == DiscountType.FixedAmount)
                        {
                            totalAmount -= discountAdmin.DiscountValue;
                        }
                        
                    }
                    
                }
                Orders orders = new Orders
                {
                    UserId = userId,
                    totalPrice = finalAmount,
                    status = OrderStatus.Pending,
                    AddressId = Address.id,
                    discountId = id ,
                };
                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItems
                    {
                        Order = orders,
                        ProductId = item.productId,
                        quantity = item.quantity,
                        price = item.product.price

                    };
                    dbContext.OrderItems.Add(orderItem);

                    item.product.stock -= item.quantity;
                    item.product.updated_at = DateTime.Now;
                }
                    dbContext.SaveChanges();
                
                if (processPayment.processPayment(orders)==PaymentStatus.Completed)
                {
                    var payment = new Payment
                    {
                        PaymentMethod = paymentDto.method,
                        Amount = finalAmount,
                        orderId = orders.id,
                    };
                    dbContext.payments.Add(payment);
                    orders.status = OrderStatus.Processing;
                    if(discountDto!=null)
                    {
                        var discounts = dbContext.discounts.First(d => d.Code == discountDto.code);
                        discounts.UsedCount++;
                    }
                    
                    var notification = new Notification
                    {
                        UserId = userId,
                        Message = $"Your order #{orders.id} has been placed successfully!",
                        Type = NotificationType.OrderCreated,
                        IsRead = false
                    };
                    dbContext.notifications.Add(notification);
                    
                    dbContext.SaveChanges();
                }
                else if (processPayment.processPayment(orders) != PaymentStatus.Completed)
                {
                    orders.status = OrderStatus.Cancelled;
                    var notification = new Notification
                    {
                        UserId = userId,
                        Message = $"Your order #{orders.id} has been Cancelled!",
                        Type = NotificationType.Cancelled,
                        IsRead = false
                    };
                    dbContext.notifications.Add(notification);
                }
                cartManager.deleteCart(userId, cartDto.Id);
                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch{
                transaction.Rollback();
                throw;
            };

        }
        public List<OrderItemsDto> GetMyOrder(int userId, int page, int pageSize)
        {
            
            var orders=dbContext.Orders.Include(o=>o.OrderItems).Where(u=>u.UserId==userId).Skip((page - 1) * pageSize)
                .Take(pageSize).
                Select(o => new OrderItemsDto
                {
                    id = o.id,
                    quantity = o.OrderItems.Sum(oi => oi.quantity),
                    PriceAtPurchase = o.totalPrice
                }).ToList();
            if(orders == null) throw new Exception("Not Found Orders");
            return orders;
        }
        public Object? GetOrderDetails(int userId,int orderId)
        {
           
            var orderdetails = dbContext.Orders.Where(u=>u.UserId == userId&&u.id==orderId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.address)
                .Include(o => o.payment)
                .Include(o => o.discount)
                .Select(o => new
                {
                    o.id,
                    o.totalPrice,
                    status = o.status.ToString(),
                    o.created_at,
                    o.updated_at,
                    items = o.OrderItems.Select(oi => new
                    {
                        productId = oi.ProductId,
                        productName = oi.Product.name,
                        productImage = oi.Product.image_url,
                        oi.quantity,
                        oi.price,
                        subtotal = oi.quantity * oi.price
                    }),
                    address = new
                    {
                        o.address.Street,
                        o.address.City,
                        o.address.building,
                        o.address.PostalCode,
                        o.address.PhoneNumber
                    },
                    payment = new
                    {
                        o.payment.PaymentMethod,
                        o.payment.Amount,
                        status = o.payment.Status.ToString(),
                        o.payment.PaymentDate,
                       
                    },
                    discount = o.discount != null ? new
                    {
                        o.discount.Code,
                        type = o.discount.Type.ToString(),
                        o.discount.DiscountValue
                    } : null
                })
                .FirstOrDefault();
            if (orderdetails == null) return null;
            return orderdetails;
        }
        public OrderTrackingDto GetOrderTracking(int userId,int orderId)
        {
            var order = dbContext.Orders.Include(o=>o.OrderTrackings).Where(o => o.id == orderId && o.UserId == userId)
                .Select(o => new OrderTrackingDto
                {
                    OrderId=o.id,
                    CurrentStatus=o.status.ToString(),
                    LastUpdated=o.updated_at,
                    History=o.OrderTrackings.Select(ot=>new TrackingHistoryDto
                    {
                        status=ot.CurrentStatus.ToString(),
                        updateAt=ot.LastUpdated,
                    }).ToList()
                })
                .FirstOrDefault();
            if (order == null) throw new Exception("Not Found Order");
            return order;
        }
        public InvoiceDto DownloadInvoice(int userId,int orderId)
        {
            var order = dbContext.Orders
        .Include(o => o.OrderItems)
        .FirstOrDefault(o => o.id == orderId && o.UserId == userId);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            return new InvoiceDto
            {
                OrderId = order.id,
                OrderDate = order.created_at,
                TotalPrice = order.totalPrice,
                Items = order.OrderItems.Select(oi => new InvoiceItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.quantity,
                    Price = oi.price,
                    subTotal = (double)(oi.quantity * oi.price),
                    ProductName = oi.Product.name
                }).ToList()
            };
        }
        public void CancelledOrder(int userId , int orderId)
        {
            
            var order = dbContext.Orders.Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Include(o => o.payment)
                .FirstOrDefault(o => o.id == orderId && o.UserId == userId);
            if (order == null) throw new Exception("Not Found Order");
            if (order.status != OrderStatus.Pending) throw new Exception("Only Pending Order Can Cancelled!");
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                foreach (var item in order.OrderItems)
                {
                    item.Product.stock += item.quantity;
                    item.Product.updated_at = DateTime.Now;
                }

                order.status = OrderStatus.Cancelled;
                order.updated_at = DateTime.Now;

                if (order.payment != null)
                {
                    order.payment.Status = PaymentStatus.Failed;
                }

                dbContext.notifications.Add(new Notification
                {
                    UserId = userId,
                    Status = NotificationType.Cancelled,
                    CreatedAt = DateTime.Now
                });

                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public List<OrderDto> GetAllOrder()
        {
            return dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderTrackings)
                .Select(o => new OrderDto
                {
                    UserId = o.UserId,
                    id = o.id,
                    totalPrice = o.totalPrice,
                    status = o.status.ToString(),
                    discountId = o.discountId,
                    AddressId = o.AddressId,

                    OrderItems = o.OrderItems.Select(oi => new OrderItemsDto
                    {
                        id = oi.id,
                        quantity = oi.quantity,
                        PriceAtPurchase = oi.price,
                        ProductId = oi.ProductId
                    }).ToList(),

                    TrackingItems = o.OrderTrackings
                        .OrderBy(t => t.LastUpdated)
                        .Select(t => new OrderTrackingDto
                        {
                            CurrentStatus = t.CurrentStatus.ToString(),
                            LastUpdated = t.LastUpdated
                        }).ToList()
                })
                .ToList();
        }

        public List<OrderDto>? GetOrdersByStatus(OrderStatus orderStatus)
        {
            var order = dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(t => t.OrderTrackings)
                .Where(o => o.status == orderStatus)
                .Select(o => new OrderDto
                {
                    UserId = o.UserId,
                    id = o.id,
                    totalPrice = o.totalPrice,
                    status = o.status.ToString(),
                    discountId = o.discountId,
                    AddressId = o.AddressId,

                    OrderItems = o.OrderItems.Select(oi => new OrderItemsDto
                    {
                        id = oi.id,
                        quantity = oi.quantity,
                        PriceAtPurchase = oi.price,
                        ProductId = oi.ProductId
                    }).ToList(),

                    TrackingItems = o.OrderTrackings
                        .OrderBy(t => t.LastUpdated)
                        .Select(t => new OrderTrackingDto
                        {
                            CurrentStatus = t.CurrentStatus.ToString(),
                            LastUpdated = t.LastUpdated
                        }).ToList()
                })
                .ToList();
            if (!order.Any()) return null;
            return order;
        }
    }
}
