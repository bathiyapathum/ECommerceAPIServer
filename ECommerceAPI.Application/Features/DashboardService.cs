using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features
{
    public class DashboardService : IDashboardService
    {
        private readonly OrderRepository _orderRepository;
        private readonly VendorProductRepository _vendorProductRepository;
        private readonly UserRepository _userRepository;

        public DashboardService(OrderRepository orderRepository, VendorProductRepository vendorProductRepository, UserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _vendorProductRepository = vendorProductRepository;
            _userRepository = userRepository;
        }

        public async Task<Dictionary<string, int>> GetAvailableUserCount()
        {
            try
            {
                var result = await _userRepository.GetAvailableUserCounts();

                return result;
            }
            catch (DataException ex)
            {
                throw new DataException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred GetAvailableUserCount: {ex.Message}");
            }
        }

        public async Task<string> GetTotalRevenue()
        {
            try
            {
                var revenue = await _orderRepository.GetTotalRevenue();
                return revenue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Dictionary<string, int>> GetOrderStats()
        {
            try
            {
                var revenue = await _orderRepository.GetOrderStats();
                return revenue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetAvailableProductCounts()
        {
            try
            {
                var revenue = await _vendorProductRepository.GetAvailableProductCounts();
                return revenue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Dictionary<string, List<string>>> GetWeeklyStats()
        {
            List<string> daysOfWeek = [];
            List<string> ordersCount = [];
            List<string> issuesCount = [];
            List<string> resolvedCount = [];

            DateTime today = DateTime.UtcNow.Date;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);

            for (int i = 0; i < 7; i++)
            {
                DateTime date = startOfWeek.AddDays(i);

                string formattedDay = date.ToString("MMM-dd ddd");
                daysOfWeek.Add(formattedDay);

                var ordersQuery = await _orderRepository.FirestoreDatabase
                    .Collection("Orders")
                    .WhereEqualTo("isInCart", false)
                    .WhereGreaterThanOrEqualTo("createdAt", date)
                    .WhereLessThan("createdAt", date.AddDays(1))
                    .GetSnapshotAsync();

                ordersCount.Add(ordersQuery.Count.ToString());

                var issuesQuery = await _orderRepository.FirestoreDatabase
                    .Collection("CancelRequest")
                    .WhereGreaterThanOrEqualTo("createdAt", date)
                    .WhereLessThan("createdAt", date.AddDays(1))
                    .GetSnapshotAsync();

                issuesCount.Add(issuesQuery.Count.ToString());

                var resolvedQuery = await _orderRepository.FirestoreDatabase
                    .Collection("CancelRequest")
                    .WhereGreaterThanOrEqualTo("resolvedAt", date)
                    .WhereLessThan("resolvedAt", date.AddDays(1))
                    .GetSnapshotAsync();
                resolvedCount.Add(resolvedQuery.Count.ToString());

            }

            return new Dictionary<string, List<string>>
            {
                { "days", daysOfWeek },
                { "orders", ordersCount },
                { "issues", issuesCount },
                { "resolved", resolvedCount }
            };
        }
    }
}
