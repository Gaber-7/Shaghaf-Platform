# Shaghaf Platform - Complete Architecture

## Project Overview

Shaghaf is a complete Digital School EdTech ecosystem designed to serve students from Grade 3 Primary through Grade 12 Secondary.

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Architecture**: Clean Architecture (Domain/Application/Infrastructure/API)
- **Authentication**: JWT + ASP.NET Core Identity
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Documentation**: Swagger/OpenAPI

### Frontend
- **Framework**: Angular 17+
- **Language**: TypeScript
- **Styling**: Bootstrap 5 + SCSS
- **State Management**: RxJS/NgRx (Optional)
- **HTTP Client**: Angular HttpClient
- **Localization**: ngx-translate (AR/EN)
- **RTL Support**: Yes
- **Dark Mode**: Yes

## Solution Structure

```
Shaghaf-Platform/
├── Backend/
│   ├── Shaghaf.Domain/
│   ├── Shaghaf.Application/
│   ├── Shaghaf.Infrastructure/
│   ├── Shaghaf.API/
│   └── Shaghaf.sln
├── Frontend/
│   ├── shaghaf-angular/
│   ├── angular.json
│   └── package.json
└── Documentation/
```

## Clean Architecture Layers

### 1. Domain Layer (Business Logic)
- Entities
- Value Objects
- Enums
- Interfaces/Contracts
- Domain Events

### 2. Application Layer (Use Cases)
- Services
- DTOs
- Validators
- AutoMapper Profiles
- Specifications

### 3. Infrastructure Layer (Data Access)
- DbContext
- Repositories
- Unit of Work Pattern
- External Service Implementations

### 4. API Layer (Presentation)
- Controllers
- Middleware
- Filters
- Extensions

## Key Features

### For Students
- ✅ Video lessons with progress tracking
- ✅ Interactive lessons and quizzes
- ✅ AI Tutor assistance
- ✅ Gamification (XP, Badges, Streaks)
- ✅ Course discovery and enrollment
- ✅ Assignment submission
- ✅ Q&A discussions
- ✅ Live classes

### For Teachers
- ✅ Course management
- ✅ Lesson creation
- ✅ Video analytics (views, watch time, completion)
- ✅ Teacher engagement scoring
- ✅ Student performance tracking
- ✅ Live classroom
- ✅ Smart Board for interactive teaching

### For Parents
- ✅ Student monitoring
- ✅ Progress tracking
- ✅ Performance analytics
- ✅ Weekly reports
- ✅ Notifications

### For Admins
- ✅ Platform analytics
- ✅ User management
- ✅ Course moderation
- ✅ Teacher leaderboard
- ✅ Financial analytics

## Database Design

### Main Entities
- Users (Student, Parent, Teacher, Admin)
- EducationStage, Grade, Subject
- Course, CourseSection, Lesson
- Video, VideoWatchSession, VideoAnalytics
- Enrollment, LessonProgress
- Quiz, Question, StudentQuizAttempt
- Assignment, Submission
- LessonQuestion, LessonReply (Q&A)
- LiveClass, ClassAttendance
- TeacherEngagementScore
- CourseReview, TeacherReview
- Achievement, StudentAchievement
- Subscription, Payment, Certificate

See DATABASE.md for complete ERD and schema.

## Authentication & Authorization

- **JWT Tokens**: Access (15 min) + Refresh (7 days)
- **Role-Based Access Control**: Student, Parent, Teacher, Admin, SuperAdmin
- **Resource-Based Authorization**: Custom policies
- **Password Security**: bcrypt hashing
- **Audit Logging**: All important operations logged

## Video Analytics Architecture

### Components
1. **Client**: Sends heartbeat events every 30 seconds
2. **VideoWatchTrackingService**: Intelligently counts views
3. **Background Jobs**: Aggregates analytics daily
4. **VideoAnalytics**: Pre-computed metrics

### Metrics Tracked
- Total views (minimum 30% watched)
- Unique students
- Total watch time
- Average watch duration
- Average watch percentage
- Completion rate
- Rewatch count

## Teacher Engagement Scoring

### Scoring Components
1. **Video Engagement** (25%)
2. **Student Interaction** (20%)
3. **Q&A Activity** (15%)
4. **Live Class Activity** (15%)
5. **Course Completion** (15%)
6. **Ratings** (10%)

Score calculated daily/weekly/monthly/yearly for leaderboards.

## Development Phases

1. **Phase 1**: Backend project setup
2. **Phase 2**: Database design & EF Core setup
3. **Phase 3**: Authentication & Authorization
4. **Phase 4**: Core educational entities
5. **Phase 5**: Course & lesson management
6. **Phase 6**: Video system & analytics
7. **Phase 7**: Teacher engagement & leaderboard
8. **Phase 8**: Quizzes & assignments
9. **Phase 9**: Q&A system
10. **Phase 10**: Parent portal
11. **Phase 11**: Smart Classroom
12. **Phase 12**: AI Tutor integration
13. **Phase 13**: Angular UI/UX
14. **Phase 14**: Testing & optimization

## Deployment

### Backend
- Docker containerization
- Azure App Service / AWS EC2
- SQL Server on Azure / RDS
- Redis for caching

### Frontend
- Angular production build
- Azure Static Web Apps / CloudFront
- CDN for static assets

## Security Considerations

- ✅ Input validation on all endpoints
- ✅ Authorization checks on resources
- ✅ SQL injection protection (EF Core)
- ✅ XSS protection (Angular built-in)
- ✅ CORS configuration
- ✅ HTTPS only
- ✅ Secure password handling
- ✅ Audit logging
- ✅ Rate limiting
- ✅ API versioning

## Performance Optimization

- ✅ Database indexing strategy
- ✅ Pagination for large datasets
- ✅ AsNoTracking for read-only queries
- ✅ Lazy loading control
- ✅ N+1 query prevention
- ✅ Response caching
- ✅ Background job processing
- ✅ CDN for media files
- ✅ Database connection pooling

## Next Steps

1. Clone the repository
2. Follow Backend setup guide
3. Follow Frontend setup guide
4. Run database migrations
5. Seed initial data
6. Start development

For detailed setup instructions, see README.md in Backend and Frontend folders.
