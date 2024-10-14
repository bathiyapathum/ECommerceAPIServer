// --------------------------------------------------------------------------------------------------------------------
// VendorProductRepository: Manages database operations related to vendor products.
// Provides methods for adding, updating, deleting, and retrieving vendor products from Firestore.
// Author: Arachchi D.S.U - IT21182914
// Date: 06/10/2024
// --------------------------------------------------------------------------------------------------------------------

using ECommerceAPI.Core.Entities.ProductEntity;
using ECommerceAPI.Core.Entities.UserEntity;
using ECommerceAPI.Infrastructure.Persistance;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class VendorProductRepository
    {
        private readonly ApplicationDbContext _context;

        public VendorProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new vendor product
        public async Task AddVendorProductAsync(VendorProduct product)
        {
            // Add the document to Firestore and get the generated document reference
            var documentRef = await _context.FirestoreDatabase.Collection("VendorProducts").AddAsync(product);

            // Retrieve the generated ID from Firebase
            product.ProductId = documentRef.Id;

            // Set the ProductId field in the Firestore document after creating the document
            await documentRef.UpdateAsync(new Dictionary<string, object>
            {
                { "productId", product.ProductId }
            });
        }

        // Update an existing vendor product
        public async Task UpdateVendorProductAsync(VendorProduct product)
        {
            await _context.FirestoreDatabase.Collection("VendorProducts").Document(product.ProductId).SetAsync(product);
        }

        // Toggle the activation status of a product
        public async Task<bool> ToggleProductActivationAsync(string productId)
        {
            var productRef = _context.FirestoreDatabase.Collection("VendorProducts").Document(productId);
            var snapshot = await productRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                var currentStatus = snapshot.GetValue<bool>("isActive");
                var updates = new Dictionary<string, object>
                {
                    { "isActive", !currentStatus } // Toggle the current status
                };
                await productRef.UpdateAsync(updates);
                return true;
            }
            return false;
        }

        // Update Vendor Product stock (e.g. for managing inventory)
        public async Task<bool> UpdateVendorProductStockAsync(string productId, Dictionary<string, object> updatedField)
        {
            try
            {
                var documentRef = _context.FirestoreDatabase.Collection("VendorProducts").Document(productId);
                var snapshot = await documentRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    await documentRef.UpdateAsync(updatedField);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during execution of UpdateVendorProductStockAsync: {ex.Message}");
            }
        }

        // Update Vendor Product with feedback (e.g. customer feedback)
        public void UpdateVendorProduct(FeedbackInfo feedback, string productID, Transaction transaction)
        {
            var productRef = _context.FirestoreDatabase.Collection("VendorProducts").Document(productID);

            var updateData = new Dictionary<string, object>
            {
                { "feedbackInfo", FieldValue.ArrayUnion(feedback) }
            };

            transaction.Update(productRef, updateData);
        }

        // Fetch all products across all vendors
        public async Task<List<VendorProduct>> GetAllProductsAsync()
        {
            var productsQuery = await _context.FirestoreDatabase
                .Collection("VendorProducts")
                .GetSnapshotAsync();

            List<VendorProduct> allProducts = [];

            foreach (var doc in productsQuery.Documents)
            {
                allProducts.Add(doc.ConvertTo<VendorProduct>());
            }

            // Return the list of all products
            return allProducts;
        }


        public async Task<string> GetAvailableProductCounts()
        {
            try 
            { 
                var productsQuery = await _context.FirestoreDatabase
                    .Collection("VendorProducts")
                    .WhereEqualTo("isActive", true)
                    .GetSnapshotAsync();
                int productCount = productsQuery.Count;
                return productCount.ToString();
            }
            catch(Exception ex)
            {
                throw new Exception($"Error while execution of GetAvailableProductCounts:  {ex.Message}");
            }
        }



        // Get a vendor product by its ID
        public async Task<VendorProduct> GetVendorProductByIdAsync(string productId)
        {
            var productDoc = await _context.FirestoreDatabase.Collection("VendorProducts").Document(productId).GetSnapshotAsync();
            return productDoc.Exists ? productDoc.ConvertTo<VendorProduct>() : null;
        }

        // Delete a vendor product by its ID
        public async Task DeleteVendorProductAsync(string productId)
        {
            await _context.FirestoreDatabase.Collection("VendorProducts").Document(productId).DeleteAsync();
        }

        // Get all vendor products for a specific vendor by vendorId
        public async Task<List<VendorProduct>> GetVendorProductsByVendorAsync(string vendorId)
        {
            var productsQuery = await _context.FirestoreDatabase.Collection("VendorProducts")
                .WhereEqualTo("vendorId", vendorId)
                .GetSnapshotAsync();

            var vendorProducts = new List<VendorProduct>();
            foreach (var doc in productsQuery.Documents)
            {
                vendorProducts.Add(doc.ConvertTo<VendorProduct>());
            }

            return vendorProducts;
        }

        // Update vendor product rating
        public async Task<bool> UpdateVendorProductRating(string productId, int rating, Transaction transaction)
        {
            // Fetch the product details
            var productDetails = await _context.FirestoreDatabase.Collection("VendorProducts").Document(productId).GetSnapshotAsync();
            if (!productDetails.Exists) return false;

            // Convert snapshot to VendorProduct object
            VendorProduct productSnapshot = productDetails.ConvertTo<VendorProduct>();

            // Fetch vendor profile details
            var vendorProfileSnapShot = await _context.FirestoreDatabase.Collection("Users").Document(productSnapshot.VendorId).GetSnapshotAsync();
            if (!vendorProfileSnapShot.Exists) return false;

            // Convert snapshot to User object
            User vendorProfile = vendorProfileSnapShot.ConvertTo<User>();

            if (vendorProfile.FeedbackInfo == null)
            {
                vendorProfile.FeedbackInfo = new VendorFeedbackInfo
                {
                    SumOfRating = 0,
                    FeedbackCount = 0
                };
            }

            var feedbackUpdateData = new Dictionary<string, object>
            {
                { "feedbackInfo.sumOfRating", vendorProfile.FeedbackInfo.SumOfRating + rating },
                { "feedbackInfo.feedbackCount", vendorProfile.FeedbackInfo.FeedbackCount + 1 }
            };

            // Update the vendor profile in Firestore transaction
            transaction.Update(vendorProfileSnapShot.Reference, feedbackUpdateData);

            return true;
        }
    }
}
