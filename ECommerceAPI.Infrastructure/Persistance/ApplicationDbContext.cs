using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using System;


namespace ECommerceAPI.Infrastructure.Persistance
{
    public class ApplicationDbContext 
    {
        public ApplicationDbContext()
        {
            InitializeFirestore();
        }

        public FirestoreDb _firestoreDb;

        public void InitializeFirestore()
        {
            if (_firestoreDb == null)
            {
                string pathToServiceAccountKey = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
                if (string.IsNullOrEmpty(pathToServiceAccountKey))
                {
                    throw new InvalidOperationException("Firestore credentials not found. Set GOOGLE_APPLICATION_CREDENTIALS environment variable.");
                }

                GoogleCredential credential = GoogleCredential.FromFile(pathToServiceAccountKey);
                FirestoreClient client = new FirestoreClientBuilder
                {
                    ChannelCredentials = credential.ToChannelCredentials()
                }.Build();

                // Initialize FirestoreDb with the FirestoreClient
                _firestoreDb = FirestoreDb.Create("ecommerceead-c19d4", client);
            }
        }

        public FirestoreDb FirestoreDatabase => _firestoreDb;

    }
}
