# Backend Requirements for Booking.com Clone

## Technologies

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core (ORM)
- SQL Server or PostgreSQL (database)
- JWT (authentication)
- Swagger (API documentation)
- Automapper (DTO mapping)
- Serilog (logging)
- FluentValidation (validation)
- Swashbuckle (OpenAPI/Swagger)
- Identity (user management)

## Functional Requirements

### 1. User Management

- User registration (guest / provider / admin)
- Login and logout (JWT token / cookie)
- Password reset and change
- User profile update
- Email verification (optionally SMS for phone)
- User roles (guest, provider, admin)

### 2. Hotel/Service Management

- Add hotel/service by owner
- Edit hotel/service
- Delete hotel/service
- List all hotels/services
- Search hotels/services (by name, location, categories, availability, etc.)
- Filter and sort results (e.g., price, popularity, rating)

### 3. Room/Offer Management

- Add rooms/offers to hotel/service
- Edit room/offer data
- Set prices and availability (availability calendar)
- Manage room/service photos

### 4. Reservations

- Create reservation (by user)
- View own reservations
- Cancel reservation
- Change reservation date (if policy allows)
- Check availability (before creating reservation)
- Payment processing (integration with payment gateway — e.g., Stripe, PayU)

### 5. Payment Management

- Create payment session
- Payment confirmation
- Refunds in case of cancellation
- User transaction history
- Sales reports for hotel/service owners

### 6. Ratings and Reviews System

- Add rating and review after completed reservation
- Edit/delete own reviews
- View average rating of hotel/service
- Filter offers by ratings

### 7. Admin Panel (API for admin)

- Manage users (ban, activate, delete accounts)
- Manage hotels/services (moderate reported offers)
- Manage reviews (moderate reported reviews)
- Access to all transactions

### 8. Notifications

- Email notifications for:
  - Registration confirmation
  - Reservation confirmation
  - Reservation cancellation
  - Date change
- System notifications (e.g., WebSocket or SignalR in the future)

### 9. Reports and Support

- Report problems with reservation/service
- API for reports (send message, admin response)

### 10. Promotions and Discount Codes (optional)

- Create discount codes by providers/admin
- Code verification during reservation
- Code limits and expiration dates

### 11. Audit and Security

- User activity logs (e.g., logins, reservation creation)
- Audit of changes in offers/hotels
- Endpoint protection (authorization, rate limiting)

### 12. Location Support

- Search by city/country
- Map support (assigning geographic location to hotel/service)

## Technical Requirements

- Input data validation (e.g., FluentValidation)
- Standard API responses (e.g., API Response Wrapper)
- Error and exception handling (global Exception Handler)
- API Versioning (optional)
- API documentation (Swagger/OpenAPI)
- Logging (e.g., Serilog)
- DTO mapping (e.g., AutoMapper)
- Authentication and authorization (JWT, Identity)
- Unit and integration tests
