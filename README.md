# Photography Booking Platform

A full-stack application for booking photography services, built with React and .NET.

## Tech Stack

### Frontend
- React 19
- Vite
- React Router DOM 7
- Bootstrap 5
- Radix UI (Components & Icons)
- JavaScript/JSX

### Backend
- .NET 8
- Entity Framework Core
- PostgreSQL
- ASP.NET Core Identity

## Dependencies

### Frontend Dependencies 
json

"dependencies": {
"@radix-ui/react-icons": "^latest",
"@radix-ui/themes": "^latest",
"bootstrap": "^5.3.3",
"react": "^19.0.0",
"react-dom": "^19.0.0",
"react-router-dom": "^7.2.0"
}

### Key API Endpoints

#### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `GET /api/auth/me` - Get current user
- `GET /api/auth/logout` - Logout user

#### Photographers
- `GET /api/photographers` - List all photographers
- `GET /api/photographers/{id}` - Get photographer details
- `GET /api/photographers/{id}/availability` - Get photographer's availability
- `GET /api/photographers/{id}/packages` - Get photographer's packages

#### Bookings
- `GET /api/bookings` - List user's bookings
- `POST /api/bookings` - Create new booking
- `GET /api/bookings/{id}` - Get booking details
- `PUT /api/bookings/{id}` - Update booking
- `DELETE /api/bookings/{id}` - Cancel booking

#### Packages
- `GET /api/packages` - List all packages
- `GET /api/packages/{id}` - Get package details

#### Galleries
- `GET /api/galleries` - List galleries
- `GET /api/galleries/{id}` - Get gallery details
- `POST /api/galleries` - Create new gallery
- `PUT /api/galleries/{id}` - Update gallery
- `DELETE /api/galleries/{id}` - Delete gallery

## Setup Instructions

### Frontend Setup
1. Clone the repository
2. Navigate to the client directory: `cd Capstone-client`
3. Install dependencies: `npm install`
4. Start the development server: `npm run dev`

### Backend Setup
1. Navigate to the API directory: `cd Capstone-api`
2. Update database connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Start the API: `dotnet run`

## Features
- User authentication and authorization
- Photographer profiles and portfolios
- Booking management
- Package selection
- Gallery management
- Review system
- Availability scheduling