Rishtanata Marriage Registration System

Project Overview

The Rishtanata Marriage Registration System is a web-based application designed to digitize and manage the marriage registration and Nikah approval process.

The system allows applicants to submit marriage applications, provide the required information and documents, track application progress, and receive notifications. Authorized Jama'at and Rishta Nata officials can review, verify, approve, reject, and manage applications throughout the workflow.

Project Objectives

- Digitize the marriage registration process.
- Reduce manual paperwork and processing time.
- Provide a structured application and approval workflow.
- Ensure secure handling of applicants' information and documents.
- Provide role-based access to different users.
- Maintain audit logs of important system activities.
- Generate marriage certificates after final approval.

Main Users

- Bride
- Bridegroom
- Guardian
- Witness
- Jama'at President
- Circuit President
- Rishta Nata Secretary
- Assistant/General Secretary
- Amir
- SuperAdmin
- Admin
- Jama'at Member

Core Features

- User authentication and authorization
- Role-based access control
- Marriage application
- Bride and bridegroom information
- Guardian information
- Witness information
- Representative information
- Document upload and verification
- Application review and approval
- Application rejection and correction workflow
- Appointment and Nikah scheduling
- Marriage certificate generation
- Notifications
- Reports and dashboards
- Audit logging

Application Workflow

Login
  ↓
Dashboard
  ↓
Create Marriage Application
  ↓
Bride/Bridegroom Details
  ↓
Guardian Information
  ↓
Witness Information
  ↓
Applicants' Review
  ↓
Submit Application
  ↓
Jama'at President Review
  ↓
National Rishta Nata Verification
  ↓
Amir Approval
  ↓
Generate Nikkah Form Serial Number
  ↓
Marriage Certificate Issuance

Technology Stack

Frontend

- HTML
- CSS
- Bootstrap
- JavaScript
- ASP.NET Core MVC Views

Backend

- C#
- ASP.NET Core MVC

Database

- MySQL
- Entity Framework Core

Authentication

- ASP.NET Core Identity

Validation

- FluentValidation

Version Control

- Git
- GitHub

Architecture

The project follows a Clean Architecture approach to keep business logic independent from the presentation layer and infrastructure.

Rishtanata
│
├── Rishtanata.Domain
│   ├── Entities
│   ├── Abstractions
│   └── Enums
│
├── Rishtanata.Application
│   ├── Services
│   ├── DTOs
│   ├── Validators
│   ├── Interfaces
│   └── Exception Handlers
│
├── Rishtanata.Infrastructure
│   ├── DbContext
│   ├── Migrations
│   ├── Identity
│   └── FileStorage
│
└── Rishtanata.Presentation
    ├── Controllers
    ├── Views
    ├── ViewModels
    └── wwwroot

Main Entities

The system will include entities such as:

- User
- Role
- Jamaat Member
- Marriage Application
- Bride
- Bridegroom
- Guardian
- Representative
- Witness
- Document
- Review
- Appointment
- Certificate
- Notification
- Audit Log

Git Branching Strategy

The project uses the following branch structure:

main
└── develop
    ├── feature/authentication
    ├── feature/application
    ├── feature/bride
    ├── feature/bridegroom
    ├── feature/documents
    ├── feature/review
    ├── feature/appointment
    ├── feature/reports
    └── feature/notifications

Branch Rules

- Do not develop directly on "main".
- Use feature branches for individual modules.
- Feature branches should be merged into "develop" through Pull Requests.
- Team leads review Pull Requests before merging.
- "main" should contain stable, tested code.

Coding Standards

- Use PascalCase for classes and methods.
- Use camelCase for local variables.
- Keep controllers focused on handling HTTP requests.
- Put business logic inside services.
- Validate input before saving data.
- Use meaningful commit messages.
- Follow the established project architecture.
- Avoid unnecessary duplication of code.

Security

The application should implement:

- Password hashing through ASP.NET Identity.
- Role-based authorization.
- Input validation.
- File type and size restrictions.
- HTTPS in production.
- Protection of sensitive information.
- Audit logging for critical actions.
- Regular database backups.

Development Workflow

Requirements
     ↓
Database Design
     ↓
Architecture
     ↓
Project Setup
     ↓
Authentication
     ↓
Marriage Application
     ↓
Supporting Modules
     ↓
Review & Approval
     ↓
Appointments
     ↓
Certificate
     ↓
Reports & Notifications
     ↓
Testing
     ↓
Deployment

Contribution

Each team member is responsible for their assigned module.

Before submitting changes:

1. Work on the appropriate feature branch.
2. Follow the project's coding standards.
3. Test your changes locally.
4. Commit using a meaningful commit message.
5. Push the feature branch to GitHub.
6. Create a Pull Request into "develop".
7. Wait for code review before merging.

Project Status

Development in progress.

The project is currently being developed as a collaborative team project.

License

This project is developed for the Rishtanata marriage registration and management system.