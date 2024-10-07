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

        public async Task<bool> UpdateVendorProductStockAsync(string productId, Dictionary<string, object> updatedField )
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
                throw new Exception($"Error while execution of UpdateVendorProductStockAsync:  {ex.Message}");

            }

        }

        public void UpdateVendorProduct(FeedbackInfo feedback, string productID, Transaction transaction)
        {
            var productRef = _context.FirestoreDatabase.Collection("VendorProducts").Document(productID);

            var updateData = new Dictionary<string, object>
            {
                {"feedbackInfo", FieldValue.ArrayUnion(feedback) }
            };

            //await productRef.UpdateAsync(updateData);

            transaction.Update(productRef, updateData);
            //await _context.FirestoreDatabase.Collection("VendorProducts").Document(productID).SetAsync(feedback);
        }

        // Fetching all products across all vendors
        public async Task<List<VendorProduct>> GetAllProductsAsync()
        {
            var productsQuery = await _context.FirestoreDatabase
                .Collection("VendorProducts")
                .GetSnapshotAsync();

            var allProducts = new List<VendorProduct>();
            foreach (var doc in productsQuery.Documents)
            {
                allProducts.Add(doc.ConvertTo<VendorProduct>());
            }

            return allProducts;
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

        // Get all vendor products for a specific vendor
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

        //public async Task<bool> UpdateVendorProductRating(string ProductId, int Rating, Transaction transaction)
        //{
        //    var productDetails = await _context.FirestoreDatabase.Collection("VendorProducts").Document(ProductId).GetSnapshotAsync();

        //    VendorProduct productSnapshot = productDetails.ConvertTo<VendorProduct>();

        //    if(productSnapshot != null)
        //    {
        //        var venderProfileSnapShot = await _context.FirestoreDatabase.Collection("User").Document(productSnapshot.VendorId).GetSnapshotAsync();

        //        User venderProfile = venderProfileSnapShot.ConvertTo<User>();

        //        if (venderProfile != null)
        //        {
        //            var feedBackUpdateData = new Dictionary<string, object>
        //            {
        //                { "feedbackInfo.sumOfRating", venderProfile.FeedbackInfo.SumOfRating + Rating },
        //                { "feedbackInfo.feedbackCount", venderProfile.FeedbackInfo.FeedbackCount + 1 }
        //            };
        //        }

        //        transaction.Update(venderProfileSnapShot.Reference, feedBackUpdateData);
        //    }

        //}

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

            // Prepare the update data
            //var feedbackUpdateData = new Dictionary<string, object>
            //{
            //    { "feedbackInfo.sumOfRating", vendorProfile.FeedbackInfo.SumOfRating + rating },
            //    { "feedbackInfo.feedbackCount", vendorProfile.FeedbackInfo.FeedbackCount + 1 }
            //};

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
