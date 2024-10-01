using ECommerceAPI.Core.Entities.ProductEntity;
using ECommerceAPI.Infrastructure.Persistance;
using Google.Cloud.Firestore;
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
    }
}
