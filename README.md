# OnTheRanj - Timesheet Management System
#
# **Architecture, Design Patterns, and Trade-offs**

## Architecture Overview

The OnTheRanj Timesheet Management System is built using a layered, modular architecture that separates concerns across API, domain, infrastructure, and frontend layers. This approach ensures maintainability, testability, and scalability.

### Backend (.NET 8)
- **Clean Architecture**: The solution is split into API, Core (domain), and Infrastructure projects. This enforces separation of concerns and allows for independent testing and evolution of each layer.
- **RESTful API**: All endpoints follow REST conventions, making the API predictable and easy to consume.
- **Entity Framework Core (Code-First)**: Enables rapid development and easy migrations, with strong typing and LINQ support.
- **Repository & Unit of Work Patterns**: Abstracts data access, making it easy to swap out the data layer or mock it for tests.
- **AutoMapper**: Reduces boilerplate for mapping between entities and DTOs.
- **JWT Authentication**: Ensures secure, stateless API access.
- **Design Patterns**: Factory, Strategy, and Decorator patterns are used to encapsulate object creation, business rules, and cross-cutting concerns like logging.
- **Testing**: NUnit and Moq are used for comprehensive unit testing of services, repositories, and controllers.

### Frontend (Angular 20)
- **Feature-based Module Architecture**: Each major feature (e.g., timesheets, assignments) is encapsulated in its own module for clarity and lazy loading.
- **NgRx**: Used for global state management, ensuring predictable state transitions and enabling time-travel debugging.
- **Reactive Forms**: Provides robust, type-safe form handling and validation.
- **RxJS**: Handles asynchronous data streams and side effects.
- **Angular Signals**: Used for local UI state, improving performance and simplifying change detection.
- **Role-based Routing**: Guards and UI logic ensure users only see what they are authorized to access.

### Database
- **SQL Server**: Chosen for reliability and enterprise features. Code-first migrations ensure schema is always in sync with the model.
- **Seed Data**: Provided for rapid onboarding and demo purposes.

## Design Patterns & Rationale

### Repository & Unit of Work
- **Why**: To decouple business logic from data access, making the codebase easier to test and maintain.
- **Trade-off**: Adds some boilerplate, but pays off in testability and flexibility.

### Factory Pattern
- **Why**: To centralize and abstract the creation of service instances, especially when dependencies or configuration may vary.
- **Trade-off**: Slightly more complex registration, but enables future extensibility.

### Strategy Pattern
- **Why**: To encapsulate different business rules (e.g., timesheet validation) and allow them to be swapped or extended without modifying core logic.
- **Trade-off**: More classes, but much easier to extend and test business rules.

### Decorator Pattern
- **Why**: To add cross-cutting concerns (like logging or caching) to services without modifying their code.
- **Trade-off**: Slightly more indirection, but keeps core services clean and focused.

## Key Trade-offs

1. **Simplicity vs. Extensibility**: The architecture favors extensibility and testability, even if it introduces more files and abstractions.
2. **Code-First Migrations**: Chosen for developer productivity, but requires discipline to keep migrations in sync.
3. **No Caching Layer**: For demo purposes, caching (e.g., Redis) is omitted, but the design allows for easy addition via decorators.
4. **JWT without Refresh Tokens**: Simpler for demo, but in production, refresh tokens and token revocation should be added.
5. **No Email/Notification System**: Not implemented to keep the scope focused, but hooks are present for future integration.

## Summary

This project demonstrates a production-quality, maintainable, and extensible full-stack application using modern .NET and Angular best practices. The chosen patterns and trade-offs are explained above, and the codebase is structured for clarity, testability, and future growth.

## Project Overview

OnTheRanj is a production-quality **Full Stack Timesheet Management System** built with **.NET 8** and **Angular 20**. It enables employees to submit weekly timesheets against assigned project codes, while managers can create projects, assign them to employees, approve timesheets, and generate comprehensive reports.

## Architecture

### Backend (.NET 8)
- **Clean Architecture** with separation of concerns
- **RESTful Web API** following REST principles
- **Entity Framework Core** with Code-First approach
- **Repository Pattern** and **Unit of Work** for data access
- **AutoMapper** for object mapping
- **JWT Authentication** for secure API access
- **Design Patterns**: Factory, Strategy, Decorator
- **Unit Testing** with NUnit and Moq

### Frontend (Angular 20)
- **Feature-based Module Architecture**
- **NgRx** for global state management
- **Reactive Forms** for data entry
- **RxJS** for asynchronous operations
- **Angular Signals** for local UI state
- **Role-based routing** and access control
- **Modern responsive UI** with attractive design

### Database
- **SQL Server** with Code-First migrations
- Optimized indexes for query performance
- Referential integrity constraints
- Seed data for quick start

## Project Structure


```
OnTheRanj/
├── OnTheRanj.API/                  # Web API (.NET 8)
│   ├── Controllers/                # API Controllers
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Data/                       # DbSeeder, context helpers
│   ├── Mapping/                    # AutoMapper profiles
│   ├── Migrations/                 # EF Core migrations
│   ├── Properties/                 # Launch settings
│   ├── appsettings.json            # API config
│   └── Program.cs                  # API entry point
│
├── OnTheRanj.Core/                 # Domain Layer
│   ├── DTOs/                       # Core DTOs (for reports, summaries)
│   ├── Entities/                   # Domain Models
│   ├── Enums/                      # Enums (Status, Roles, etc.)
│   └── Interfaces/                 # Repository & Service Interfaces
│       ├── Services/               # Service interfaces
│       └── Strategies/             # Strategy interfaces
│
├── OnTheRanj.Infrastructure/       # Data Access & Business Logic
│   ├── Data/                       # DbContext
│   ├── Migrations/                 # Infrastructure migrations
│   ├── Repositories/               # Repository Implementations
│   ├── Services/                   # Business Logic Services
│   │   ├── Decorators/             # Decorator pattern (logging, etc.)
│   │   ├── Implementations/        # Service implementations
│   │   └── Strategies/             # Strategy pattern
│   └── UnitOfWork/                 # Unit of Work Implementation
│
├── OnTheRanj.Tests/                # Backend Unit Tests
│   ├── Controllers/                # Controller tests
│   ├── DTOs/                       # DTO tests
│   ├── Data/                       # Seeder tests
│   ├── Mapping/                    # AutoMapper tests
│   ├── Repositories/               # Repository tests
│   ├── Services/                   # Service tests
│   └── UnitOfWork/                 # Unit of Work tests
│
├── OnTheRanj.Web/                  # Angular 20 Frontend
│   ├── src/app/
│   │   ├── core/                   # Core services, guards, interceptors, models
│   │   ├── features/               # Feature modules (auth, employee, manager)
│   │   ├── shared/                 # Shared components, layout, constants
│   │   └── store/                  # NgRx store (actions, reducers, selectors, effects)
│   ├── public/                     # Static assets
│   ├── proxy.conf.json             # API proxy config
│   ├── angular.json                # Angular CLI config
│   └── coverage/                   # Frontend coverage reports
│
├── Database/                       # Database Scripts
│   ├── SeedData.sql                # Initial Data Script
│   ├── SeedDataSqlite.sql          # SQLite seed
│   ├── SeedTimesheetsSqlite.sql    # SQLite timesheet seed
│   └── update_manager_hash.sql     # Utility script
│
├── TestResults/                    # Backend test and coverage results
├── .github/                        # GitHub Actions/workflows
├── OnTheRanj.sln                   # Solution file
└── README.md                       # Project documentation
```

## Key Features

### For Employees
- Submit weekly timesheets with date, hours, and description
- View assigned project codes
- Edit draft timesheets
- Submit timesheets for approval
- View timesheet status (Draft/Submitted/Approved/Rejected)
- Maximum 24 hours per day validation
- No duplicate entries for same project and date

### For Managers
- Create and manage project codes
- Activate/Deactivate projects
- Assign project codes to employees
- Define assignment start and end dates
- Approve or reject timesheets
- Mandatory comments for rejections
- Generate comprehensive reports:
  - Employee-wise hours summary
  - Project-wise hours summary
  - Billable vs Non-billable hours
  - Date range filtering

## Design Patterns Implemented

### 1. Repository Pattern
- **Purpose**: Abstracts all data access logic, so business logic never directly interacts with EF Core or SQL.
- **Location**: `OnTheRanj.Infrastructure/Repositories/` (e.g., `ProjectCodeRepository.cs`, `TimesheetRepository.cs`)
- **Usage**: All CRUD operations for entities are funneled through repositories, making it easy to mock data access in tests.
- **Benefits**: Testability, maintainability, and the ability to swap data sources with minimal changes.

### 2. Unit of Work Pattern
- **Purpose**: Coordinates changes across multiple repositories and ensures atomic transactions.
- **Location**: `OnTheRanj.Infrastructure/UnitOfWork/` (e.g., `UnitOfWork.cs`)
- **Usage**: Services use the Unit of Work to commit or rollback changes as a single transaction, especially for multi-entity operations.
- **Benefits**: Data consistency, transactional integrity, and simplified service logic.

### 3. Factory Pattern
- **Purpose**: Centralizes and abstracts the creation of service instances, especially when dependencies or configuration may vary.
- **Location**: Service registration in `Program.cs` and DI setup.
- **Usage**: Used to instantiate services with different strategies or decorators based on runtime configuration.
- **Benefits**: Flexible object creation, easier testing, and future extensibility.

### 4. Strategy Pattern
- **Purpose**: Encapsulates interchangeable business rules, such as timesheet approval or validation logic.
- **Location**: `OnTheRanj.Infrastructure/Services/Strategies/` (e.g., `DefaultApprovalStrategy.cs`)
- **Usage**: The approval process can use different strategies (e.g., default, custom) without changing the service code. Strategies are injected via DI.
- **Benefits**: Runtime algorithm selection, extensibility, and open/closed principle compliance.

### 5. Decorator Pattern
- **Purpose**: Adds cross-cutting concerns (like logging, auditing, or caching) to services without modifying their core logic.
- **Location**: `OnTheRanj.Infrastructure/Services/Decorators/` (e.g., `TimesheetApprovalLoggingDecorator.cs`)
- **Usage**: Decorators wrap core services to add logging or other behaviors transparently. Registered in DI so consumers get the enhanced service.
- **Benefits**: Clean separation of concerns, enhanced functionality, and easy toggling of cross-cutting features.

### Pattern Synergy
- **Combined Usage**: Patterns are often combined. For example, a service may use repositories (Repository), coordinate them with a Unit of Work, apply a Strategy for business rules, and be wrapped with a Decorator for logging—all instantiated via Factory/DI.

**Result:**
These patterns together ensure the codebase is modular, testable, and easy to extend for new business requirements or technical improvements.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server 2019+
- Angular CLI 20
- Visual Studio 2022 or VS Code

### Backend Setup

1. **Clone the repository**
```bash
git clone <repository-url>
cd OnTheRanj
```

2. **Update connection string**
Edit `OnTheRanj.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OnTheRanjDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

3. **Run database migrations**
```bash
cd OnTheRanj.API
dotnet ef database update
```

4. **Run seed data script**
```bash
sqlcmd -S localhost -d OnTheRanjDB -i ../Database/SeedData.sql
```

5. **Run the API**
```bash
dotnet run
```

API will be available at: `https://localhost:5001`

### Frontend Setup

1. **Navigate to Angular project**
```bash
cd OnTheRanj.Web
```

2. **Install dependencies**
```bash
npm install
```

3. **Update API URL**
Edit `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

4. **Run the application**
```bash
ng serve
```

Application will be available at: `http://localhost:4200`

## Test Credentials

### Manager Account
- **Email**: manager@ontheranj.com
- **Password**: Manager@123

### Employee Accounts
- **Alice**: alice@ontheranj.com / Employee@123
- **Bob**: bob@ontheranj.com / Employee@123
- **Charlie**: charlie@ontheranj.com / Employee@123

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

### Project Management (Manager Only)
- `GET /api/projects` - Get all project codes
- `GET /api/projects/active` - Get all active project codes
- `GET /api/projects/{id}` - Get project code by ID
- `POST /api/projects` - Create project code
- `PUT /api/projects/{id}` - Update project code
- `DELETE /api/projects/{id}` - Delete project code
- `PATCH /api/projects/{id}/activate` - Activate project
- `PATCH /api/projects/{id}/deactivate` - Deactivate project

### Project Assignments (Manager Only)
- `GET /api/assignments/employee/{employeeId}` - Get employee assignments
- `POST /api/assignments` - Assign project to employee
- `DELETE /api/assignments/{id}` - Remove assignment

### Timesheets (Employee)
- `GET /api/timesheets/my` - Get employee's timesheets
- `POST /api/timesheets` - Create timesheet
- `PUT /api/timesheets/{id}` - Update timesheet
- `POST /api/timesheets/{id}/submit` - Submit timesheet
- `DELETE /api/timesheets/{id}` - Delete draft timesheet

### Timesheet Approval (Manager Only)
- `GET /api/timesheets/pending` - Get pending timesheets
- `POST /api/timesheets/{id}/approve` - Approve timesheet
- `POST /api/timesheets/{id}/reject` - Reject timesheet

### Reports (Manager Only)
- `GET /api/reports/employee-hours?startDate={start}&endDate={end}` - Employee hours summary
- `GET /api/reports/project-hours?startDate={start}&endDate={end}` - Project hours summary
- `GET /api/reports/billable-hours?startDate={start}&endDate={end}` - Billable vs Non-billable

## Running Tests
dotnet test

## Test Coverage & How to Run

### Backend Tests & Coverage
Run all backend tests and generate a coverage report:
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults
~/.dotnet/tools/reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:TestResults/coverage-report
open TestResults/coverage-report/index.html
```
**Latest backend test coverage:**
- Line coverage: 38%
- Covered lines: 1503 / 3866
- Classes: 54, Files: 58
- All critical business logic, DTOs, and controllers are covered by unit tests.

### Frontend Tests & Coverage
Run all frontend tests and generate a coverage report:
```bash
cd OnTheRanj.Web
ng test --code-coverage
open OnTheRanj.Web/coverage/index.html
```
**Latest frontend test coverage:**
- (Insert % here after running `ng test --code-coverage`)

### Recent Test Additions
- Added/extended tests for all backend DTOs, services, repositories, and controllers.
- Edge cases, error handling, and business rules are covered.
- Frontend: All major components and NgRx store logic have unit tests.

### Where to Find Coverage Reports
- Backend: `TestResults/coverage-report/index.html`
- Frontend: `OnTheRanj.Web/coverage/index.html`

### Example: Full Backend Test & Coverage Workflow
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults
~/.dotnet/tools/reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:TestResults/coverage-report
open TestResults/coverage-report/index.html
```

### Example: Full Frontend Test & Coverage Workflow
```bash
cd OnTheRanj.Web
ng test --code-coverage
open coverage/index.html
```

## Business Rules

1. **Timesheet Submission**
   - Maximum 24 hours per day per employee
   - No duplicate entries for same project and date
   - Can only submit timesheets for assigned and active projects
   - Assignments must be valid for the timesheet date

2. **Timesheet Lifecycle**
   - Draft → Submitted → Approved/Rejected
   - Only draft timesheets can be edited or deleted
   - Rejected timesheets must have manager comments

3. **Project Assignments**
   - Must have a start date
   - End date is optional (null means ongoing)
   - Employees can only log time within assignment date range

4. **Project Codes**
   - Must have unique code
   - Can be marked as billable or non-billable
   - Can be activated or deactivated
   - Inactive projects cannot receive new timesheet entries

## Frontend Features

### State Management (NgRx)
- Centralized application state
- Predictable state updates
- Time-travel debugging support
- Performance optimization with selectors

### Reactive Forms
- Form validation
- Dynamic form controls
- Real-time validation feedback
- Type-safe form handling

### Angular Signals
- Local component state
- Reactive UI updates
- Improved performance
- Simplified change detection

### UI/UX
- Modern Material Design
- Responsive layout (mobile-friendly)
- Loading indicators
- Error handling and notifications
- Intuitive navigation

## 🔧 Configuration

### JWT Settings (appsettings.json)
```json
{
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJWTTokenGenerationWithMinimum256Bits",
    "ExpiryHours": 24
  }
}
```

### CORS Settings
Configured to allow Angular development server (`http://localhost:4200`)

## Performance Optimizations

1. **Database**
   - Indexed foreign keys
   - Composite indexes for frequent queries
   - Optimized LINQ queries with projections

2. **API**
   - Async/await throughout
   - DTOs to minimize data transfer
   - AutoMapper for efficient mapping

3. **Frontend**
   - Lazy loading of modules
   - OnPush change detection strategy
   - NgRx memoized selectors
   - RxJS operators for stream optimization

## Known Issues / Trade-offs

1. **Authentication**: Currently using JWT without refresh tokens. For production, implement refresh token mechanism.
2. **File Uploads**: Not implemented. Could be added for timesheet attachments.
3. **Email Notifications**: Not implemented. Could notify managers of pending approvals.
4. **Audit Trail**: Basic audit fields present. Could be extended for comprehensive tracking.
5. **Caching**: No caching layer. Could add Redis for improved performance.

## Contributing

This is a demonstration project for showcasing full-stack development skills.

## Additional Setup & Usage Notes

### API Documentation (Swagger)

The backend provides interactive API documentation using **Swagger UI**. This allows you to explore, test, and understand all available endpoints directly from your browser.

- **Access Swagger UI:**
   - When running the backend in development mode, open [http://localhost:5197](http://localhost:5197) in your browser.
   - The Swagger UI will be available at the root URL (or as configured in your launch settings).
- **Features:**
   - View all API endpoints, request/response schemas, and authentication requirements.
   - Try out API calls directly from the browser (requires a valid JWT for protected endpoints).
- **Location:**
   - Swagger is enabled by default in development mode (see `Program.cs`).
   - Configuration is in `OnTheRanj.API/Program.cs` and `OnTheRanj.API/Properties/launchSettings.json`.


#### Authorizing with JWT in Swagger

To test protected endpoints in Swagger UI, you need to obtain a JWT token and authorize your session:

1. **Generate a JWT Token:**
   - Use the `/api/auth/login` endpoint in Swagger UI.
   - Enter a valid email and password (see Test Credentials section in this README).
   - Copy the `token` value from the login response.

2. **Authorize in Swagger UI:**
   - Click the `Authorize` button (top right in Swagger UI).
   - In the value field, enter: `Bearer <your-token-here>` (include the word `Bearer` and a space before the token).
   - Click `Authorize` and close the dialog.

You can now call any protected API endpoints as an authenticated user.

### Frontend Proxy & API Routing

For local development, the Angular app uses a proxy configuration to seamlessly route API requests to the backend, avoiding CORS issues and making development easier.

- **Start the backend** (default): http://localhost:5197
- **Start the frontend** with:
   ```bash
   ng serve --proxy-config proxy.conf.json
   ```
- **How it works:**
   - All requests from the frontend to `/api` (e.g., `/api/timesheets`) are automatically proxied to the backend at `http://localhost:5197`.
   - You do not need to hardcode the backend port in your Angular environment files for development.
   - If you change the backend port, just update `proxy.conf.json`.
- **Benefits:**
   - No CORS errors during development
   - Clean separation of frontend and backend
   - Easy to switch backend ports without changing frontend code

**proxy.conf.json example:**
```json
{
   "/api": {
      "target": "http://localhost:5197",
      "secure": false,
      "changeOrigin": true,
      "logLevel": "debug"
   }
}
```

### Useful Commands

#### Backend
- Restore: `dotnet restore`
- Build: `dotnet build`
- Run: `dotnet run`
- Migrate DB: `dotnet ef database update`
- Test: `dotnet test`

#### Frontend
- Install: `npm install`
- Start: `npm start` or `ng serve`
- Test: `ng test`

---

## Contact & Support

For issues, please open an issue in this repository.

---

## Project Structure (Summary)

- `OnTheRanj.API/` - .NET 8 Web API (backend)
- `OnTheRanj.Web/` - Angular 20 app (frontend)
- `OnTheRanj.Core/` - Core business logic, entities, interfaces
- `OnTheRanj.Infrastructure/` - Data access, repositories, services
- `OnTheRanj.Tests/` - Backend unit tests
- `Database/` - SQL seed scripts

---

## License

This project is created for educational and demonstration purposes.

## Developer

Created as a full-stack assignment demonstrating:
- Clean architecture principles
- Design patterns implementation
- Modern development practices
- Production-quality code
- Comprehensive documentation

---

**Note**: This application demonstrates production-quality code with proper error handling, validation, security, and best practices. All code is well-commented for easy understanding and maintenance.
# OnTheRanj
# OnTheRanj
