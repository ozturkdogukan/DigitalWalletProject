using AutoMapper;
using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Data.UnitOfWork;
using Digital.Operation.Base;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Order;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public class OrderService : BaseService<Category, OrderRequest, OrderResponse>, IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // Sipariş Oluşturmak için kullanılır.
        public ApiResponse Add(OrderRequest request, int userId)
        {
            if (request is null)
            {
                return new ApiResponse("Bad Request");
            }
            decimal pointSum = 0;
            decimal couponPercentAmount = 0;
            string couponCod = "";
            var products = unitOfWork.GetRepository<Product>().GetAll(x => request.ProductIds.Contains(x.Id));
            var productPrices = products.Select(x => x.Price).Sum();
            var productCount = products.Count();
            if (StockControl(products))
            {
                return new ApiResponse("No Stock");
            }
            var user = unitOfWork.GetRepository<User>().Get(x => x.Id.Equals(userId));
            var userPointRatio = user.DigitalWallet / productCount;
            decimal productsPrice = 0;
            if (string.IsNullOrWhiteSpace(request.CouponCode))
            {
                foreach (var item in products)
                {
                    pointSum += PointCalculator(item, userPointRatio, item.Price);
                    productsPrice += item.Price;
                }
            }
            else
            {
                var coupon = unitOfWork.GetRepository<Coupon>().Get(x => x.Code.Equals(request.CouponCode));
                if (CouponControl(coupon))
                {
                    return new ApiResponse("Coupon Wrong");
                }
                couponPercentAmount = coupon.PercentAmount;
                couponCod = coupon.Code;
                foreach (var item in products)
                {
                    var price = item.Price - ((coupon.PercentAmount * item.Price) / 100);
                    pointSum += PointCalculator(item, userPointRatio, price);
                    productsPrice += price;
                }
            }
            var sumPrice = productsPrice - user.DigitalWallet;
            var couponAmount = (productPrices * couponPercentAmount) / 100;
            if (sumPrice > 0)
            {
                if (Payment(sumPrice))
                {
                    var orderNumber = GenerateUniqueOrderNumber();
                    OrderAdd(userId, sumPrice, user.DigitalWallet, couponAmount, couponCod, orderNumber);
                    PointAdd(user, pointSum);
                    StockReduction(products);
                    OrderDetailAdd(products, orderNumber);
                    if (unitOfWork.SaveChanges() > 0)
                    {
                        return new ApiResponse();
                    }
                    else
                    {
                        return new ApiResponse("Internal Server Error");
                    }
                }
                return new ApiResponse("Payment Failed.");
            }
            else
            {
                return new ApiResponse();
            }
        }
        // Benzersiz Sipariş Numarası Üretir.
        private string GenerateUniqueOrderNumber()
        {
            while (true)
            {
                var orderNumber = Digital.Base.Extensions.Extension.GenerateOrderNumber();
                if (!unitOfWork.GetRepository<Order>().Any(x => x.OrderNumber.Equals(orderNumber)))
                {
                    return orderNumber;
                }
            }
        }
        // Sipariş Detaylarını OrderDetail tablosuna ekler.
        private void OrderDetailAdd(IQueryable<Product> products, string orderNumber)
        {
            foreach (var item in products)
            {
                unitOfWork.GetRepository<OrderDetail>().Add(new OrderDetail
                {
                    OrderNumber = orderNumber,
                    Price = item.Price,
                    ProductId = item.Id,
                    ProductName = item.Name
                });

            }
        }
        // Stok azaltır.
        private void StockReduction(IQueryable<Product> products)
        {
            foreach (var item in products)
            {
                item.Stock--;
                unitOfWork.GetRepository<Product>().Update(item);
            }
        }
        // Sipariş Oluşturur.
        private void OrderAdd(int userId, decimal sumPrice, decimal pointBalance, decimal couponAmount, string couponCode, string orderNumber)
        {
            unitOfWork.GetRepository<Order>().Add(new Order
            {
                UserID = userId,
                OrderNumber = orderNumber,
                CartAmount = sumPrice,
                PointAmount = pointBalance,
                CouponAmount = couponAmount,
                CouponCode = couponCode
            });
        }
        // Verilen kuponun geçerli olup olmadığını kontrol eder.
        private bool CouponControl(Coupon coupon)
        {
            if (coupon is null)
            {
                return true;
            }
            if (coupon.IsActive.Equals(false) || DateTime.Now > coupon.ExpirationDate)
            {
                return true;
            }
            return false;

        }
        // Yeterli sayıda stok olup olmadığını kontrol eder.
        private bool StockControl(IQueryable<Product> products)
        {
            foreach (var item in products)
            {
                if (item.Stock < 1)
                {
                    return true;
                }
            }
            return false;
        }
        // Puan bakiyesinin kaydedilmesini sağlar.
        private void PointAdd(User user, decimal pointSum)
        {
            user.DigitalWallet = pointSum;
            unitOfWork.GetRepository<User>().Update(user);
        }
        // Ödeme entegrasyonu için metod.
        private bool Payment(decimal sumPrice)
        {
            // Ödeme sistemi entegresi..
            return true;

        }
        // Puan bakiyesinin hesaplama işlemi yapılır.
        private decimal PointCalculator(Product item, decimal userPointRatio, decimal price)
        {
            var newPrice = price - userPointRatio;
            var percentage = (newPrice * item.PointEarningPercentage) / 100;
            var point = percentage > item.MaxPointAmount ? item.MaxPointAmount : percentage;
            return point;
        }

        // Request header kısmında bulunan JWT keyindeki user bilgisine göre siparişleri getirir. Kullanıcının kendisine ait siparişleri çekmesini sağlar.
        public ApiResponse<List<OrderResponse>> GetAllMyOrder(int userId)
        {
            var orders = unitOfWork.GetRepository<Order>().GetAll(x => x.UserID.Equals(userId));
            var orderResponses = mapper.Map<List<OrderResponse>>(orders);
            return new ApiResponse<List<OrderResponse>>(orderResponses);

        }
    }
}
