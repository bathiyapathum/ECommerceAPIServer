# E-Commerce Platform Web Application

## Overview

This project is part of a comprehensive E-Commerce platform built using a client-server architecture. The mobile application is developed for Android using Kotlin, while the backend is powered by .NET (C#) and integrated with a web application developed using React. The platform enables seamless functionality between the web and mobile applications, delivering a rich user experience for both customers and administrators.

## Features

- **Customer Registration**: Customers can create accounts via the mobile app. Email is a mandatory field and will serve as the primary identifier. Upon registration, the account will be sent for approval to a Customer Service Representative (CSR). Until the account is approved, the user will receive a meaningful message.
- **Product Search and Purchase**: Customers can search for products using filters (name, price, vendor, ratings) and sort items accordingly. Multiple items can be added to the shopping cart and purchased at once. Customers can track their order status directly through the app.

- **Order Management**:

  - Customers can request cancellations, but orders cannot be canceled directly. The CSR will be notified and will process the cancellation.
  - Vendors can manage orders related to their products.
  - Administrators and CSRs have full control over all orders across the platform.

- **Vendor Management**: Vendors can create and manage their own product listings. All vendor products are visible to customers, and vendor-specific ratings and prices can be filtered.

## Key Roles and Responsibilities

- **Administrator**: Manages the overall system, including approving or declining accounts and overseeing all orders.
- **Vendor**: Handles product listings, inventory, and vendor-specific orders.
- **Customer Service Representative (CSR)**: Approves user accounts, handles customer support, manages order cancellations, and monitors orders.

## Technologies Used

- **Backend**: .NET (C#)
- **Mobile Application**: Kotlin (Android)
- **Web Application**: React (Admin, CSR, and Vendor activities)
- **Database**: Firebase Firestore
- **Cloud Integration**: AWS services for scalable infrastructure

## Getting Started

### Prerequisites

Ensure you have the following installed:

- Android Studio
- Kotlin (for Android development)
- .NET 8 (for backend services)
- Firebase account for database integration

### Installation

1. **Clone the repository**:

   ```bash
   git clone <repository-url>
   cd web-application
   ```

2. **Set up Firebase**:

   - Create a Firebase project and configure Firestore.
   - Add the `google-services.json` file to your `app` directory.

3. **Run the app**:
   - Open the project in Android Studio.
   - Sync the project with Gradle and build the app.
   - Connect your Android device or use an emulator to run the application.

### Backend Integration

The mobile app is integrated with a centralized .NET-based web service that handles business logic, user management, and order processing. API endpoints from the backend are used to communicate with the database and handle various functionalities such as user registration, product search, and order management.

Ensure that the backend service is running and accessible from the mobile application.

## Usage

- **Registration**: Customers must provide a valid email during registration. After account creation, the customer will not be able to log in until a CSR approves the account. The user will receive a meaningful message indicating that the account is pending approval.

- **Product Search & Purchase**: Use the search filters to find products. Add items to the cart and proceed to purchase. Order status can be tracked through the "My Orders" section.

- **Order Cancellations**: To request a cancellation, navigate to the order details page. The CSR will process the request, and the customer will receive updates via notifications.

## Contributing

We welcome contributions to enhance the mobile application. Please submit pull requests with clear documentation of any changes. Ensure that the code follows standard practices and is thoroughly tested.

## References

### Backend Server .net c#

- https://github.com/bathiyapathum/ECommerceAPIServer.git

### Frontend Web Application ReactJS

- https://github.com/IT21182914/EAD-Ecommerce-frontend.git

### Mobile Application

- https://github.com/thilinahansana/EAD-Ecommerce-Mobile.git

## Contact

For any questions or support, please reach out to the development team.
