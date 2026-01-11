# 1. Introduction

In contemporary fitness and wellness industries, the management of gym facilities, member enrollment, class scheduling, and financial transactions remains one of the most complex and resource-intensive operational challenges. While digital transformation has revolutionized many aspects of business management, fitness centers continue to rely heavily on fragmented software solutions, manual record-keeping, and disconnected systems for different operational functions. These conventional practices often lead to inefficiencies in member management, scheduling conflicts, payment tracking errors, data inconsistencies, and ultimately, suboptimal customer experiences and administrative overhead.

The Maxfit+ Fitness Management System has been meticulously designed as an intelligent, web-based application tailored specifically to address this persistent operational challenge in the fitness industry. It seeks to empower fitness center administrators, staff, and members with an integrated digital platform that supports the full lifecycle of gym operations — from member registration and class scheduling to payment processing and attendance tracking. The system's goal is not only to reduce administrative workload for gym personnel but also to establish a standardized, comprehensive solution for organizing and managing all aspects of fitness center operations efficiently while enhancing the member experience through personalized dashboards and self-service capabilities.

Through a robust and secure backend infrastructure built on ASP.NET Core 9.0 with Entity Framework Core, combined with a modern, user-friendly interface utilizing role-based authentication, Maxfit+ ensures that gym administrators and staff can focus more on service quality and member satisfaction, rather than the tedious logistics of manual data management. The system is poised to become a critical asset in fitness center digital transformation initiatives, supporting the modernization of gym operations and establishing a scalable foundation for growth in health clubs, fitness centers, and wellness facilities of all sizes.

## 1.1. Purpose of this Document

The primary purpose of this document is to articulate, in a clear and comprehensive manner, the architectural and component-level design of the Maxfit+ Fitness Management System. It serves as a technical bridge between the system's functional requirements and its actual implementation. This document translates the previously defined requirement specifications into a coherent and detailed blueprint that can be directly utilized by developers, system engineers, and QA analysts during the system's development and deployment phases.

This System Design Document (SDD) provides an in-depth description of the system's internal structure, including software architecture, module breakdown, data flows, user interface behavior, and integration points. It defines the interaction between various subsystems such as member management, staff coordination, class scheduling, payment processing, and attendance tracking. The document establishes the logic of component communication, specifies database schema relationships, and outlines both functional and non-functional design considerations including security, scalability, and performance optimization.

The intended audience for this document includes:

- **Software developers** responsible for coding the backend (ASP.NET Core MVC controllers and services), frontend (Razor views and UI components), and database modules (Entity Framework Core models and migrations).
- **System architects and designers** overseeing the overall structural integrity, scalability, and maintainability of the fitness management platform.
- **Test engineers and QA teams** validating the functional correctness, security compliance, and performance of the system across different user roles (Admin, Staff, Member).
- **Fitness center stakeholders** including gym owners, facility managers, and IT personnel seeking to understand the system's capabilities, deployment requirements, and operational benefits.
- **Database administrators** responsible for SQL Server configuration, backup strategies, and performance tuning.

By establishing a shared understanding of the design, this document ensures that development efforts remain aligned with fitness industry best practices, technical standards, security requirements, and operational excellence objectives. It serves as the authoritative reference for all technical decisions and implementation strategies throughout the system's lifecycle.

## 1.2. System Information

| **Attribute** | **Description** |
|---------------|-----------------|
| **System Name** | Maxfit+ Fitness Management System |
| **Abbreviation** | Maxfit+ |
| **Version** | 1.0 |
| **Release ID** | Maxfit-2026-TR |
| **Primary Technologies** | ASP.NET Core 9.0 (MVC), Microsoft SQL Server, Entity Framework Core 9.0, Razor Views, ASP.NET Core Identity, Bootstrap/CSS |
| **Repository** | GitHub – Maxfit+ |
| **Author** | Kerem |
| **Development Model** | MVC (Model-View-Controller) Architecture with Repository Pattern |
| **Team Members** | Kerem (Full-Stack Developer & System Architect) |

## 1.3. Standards and Compliance

This System Design Document adheres to the following internationally recognized standards to ensure quality, consistency, and technical validity throughout the system's development:

- **IEEE 1016** – Standard for Software Design Descriptions
- **ISO/IEC 25010** – System and Software Quality Models
- **ANSI SQL** – Standard for Structured Query Language used in relational databases
- **ISO/IEC 27001** – Information Security Management System standards for protecting sensitive member data
- **GDPR Compliance** – Data protection and privacy regulations for handling personal information

These standards provide a robust foundation for the system's architecture, design documentation, security practices, and future scalability within fitness centers, health clubs, and wellness facilities.

## 1.4. Scope Inclusions

### **Includes**

**Secure role-based authentication with three user roles:** Administrator, Staff (Trainers), and Members using ASP.NET Core Identity

**Fitness center management structure:** Members, staff, classes, rooms, equipment, and membership types with comprehensive registration and scheduling

**Web-based interface:** Razor Views with modern responsive UI, role-specific dashboards, and self-service portals

**Core operational features:**
- Automated class scheduling with conflict detection for trainers and rooms
- Payment processing system (Cash, Credit Card, Bank Transfer, Online) with transaction tracking
- Microsoft SQL Server database with Entity Framework Core ORM
- Check-in/check-out attendance tracking
- Course registration with capacity control
- Profile photo uploads
- Statistical dashboards with revenue and demographic analytics
- Modular MVC architecture for scalability

## 1.5. Scope Exclusions

### **Excludes**

- Development of mobile or native applications (iOS/Android) supporting Maxfit+ functionalities
- Multi-language localization beyond Turkish interface support
- Integration with third-party fitness platforms (MyFitnessPal, Strava) or external health tracking systems
- AI-assisted workout recommendations, automated training programs, or intelligent nutrition planning
- Real-time video streaming for online classes or live virtual training sessions
- Wearable device integration (Fitbit, Apple Watch, Garmin) or IoT fitness equipment connectivity

## 1.6. Relationship to Other Plans

This System Design Document (SDD) serves as a foundational reference within the broader project documentation ecosystem for the Maxfit+ Fitness Management System. It establishes a clear architectural and functional framework that aligns with and supports the objectives outlined in related project plans. The interrelationships are as follows:

**Functional Requirements Document [Version 1.0]:**
The SDD is directly derived from the Functional Requirements Document, translating high-level requirements into detailed design specifications. It ensures that the system architecture and components are aligned with the functional expectations and constraints defined in the requirements phase, including member management, class scheduling, payment processing, and attendance tracking functionalities.

**Configuration Management Plan [Version 1.0]:**
This document references the Configuration Management Plan to establish guidelines for version control, change management, and traceability of design artifacts. It ensures that all design elements are consistently managed through Git version control and that modifications are systematically tracked throughout the development lifecycle.

**Software Quality Assurance Plan [Version 1.0]:**
The SDD incorporates quality attributes and compliance criteria as specified in the Software Quality Assurance Plan. It outlines the design considerations necessary to meet quality standards, including performance benchmarks for concurrent user access, security requirements for member data protection, and maintainability goals for long-term system evolution.

**Project Repository:**
The design artifacts and implementation code are stored and managed within the project's repository structure. This repository serves as the central hub for version-controlled source code, database migrations, Razor views, and collaborative development efforts.

By maintaining these interconnections, the SDD ensures that the system's design is coherent, traceable, and aligned with the project's overarching objectives and fitness industry operational standards.

## 1.7. Methodology, Tools, and Techniques

The design and implementation of the Maxfit+ Fitness Management System leverage an iterative, incremental software development methodology aligned with Agile principles, specifically Scrum practices. This methodology supports adaptive planning, continuous stakeholder feedback, and rapid delivery of working increments to accommodate evolving fitness center operational requirements.

### 1.7.1. Methodology

**Agile Scrum Framework:**
Iterations (Sprints) of 2 weeks are employed to enable frequent releases, allowing stakeholders (gym owners, facility managers, and trainers) to validate design decisions and suggest refinements promptly. This enhances responsiveness to requirement changes and promotes collaborative development.

**Test-Driven Development (TDD):**
Unit and integration tests are defined before coding modules, ensuring high code coverage and reducing defects. Automated testing suites are integrated into the development pipeline for continuous verification.

### 1.7.2. Tools

**Version Control and Collaboration:**
Git supports branching strategies such as Git Flow for parallel feature development, release preparation, and hotfix management. Code reviews are conducted to maintain code quality and facilitate knowledge sharing.

**IDE and Build Tools:**
Visual Studio 2022 / Visual Studio Code serves as the primary Integrated Development Environment (IDE) for .NET development. NuGet handles dependency management and MSBuild manages build lifecycle automation, streamlining project builds, test executions, and packaging.

**Database Management:**
Microsoft SQL Server is utilized for its ACID compliance, advanced indexing capabilities, and support for complex queries. Database schema design emphasizes normalization to minimize redundancy, with Entity Framework Core migrations managing schema evolution.

**Frontend Development:**
Razor Views serve as the server-side rendering engine integrated within ASP.NET Core MVC, ensuring seamless HTML templating, dynamic content generation, and form processing with enhanced security features like CSRF protection and model validation.

**Design and Modeling:**
UML diagrams (class diagrams, sequence diagrams, component diagrams) document system structure, data flows, and interactions, facilitating clear communication among developers and stakeholders.

### 1.7.3. Techniques

**MVC Architecture:**
The Model-View-Controller pattern segregates responsibilities into presentation (Views), business logic (Controllers), and data access (Models). This separation improves maintainability, testability, and scalability.

**Design Patterns:**
Core design patterns such as Repository, Service, Dependency Injection, and Factory are applied to promote reusable, loosely coupled components.

**Security Practices:**
Role-Based Access Control (RBAC) with ASP.NET Core Identity enforces user authentication and authorization. Input validation, data annotations, and HTTPS are used to mitigate common web vulnerabilities like SQL injection, XSS, and CSRF.

**Change Request Management:**
Feature enhancements and defect fixes are tracked systematically to maintain traceability and audit trails throughout the development lifecycle.

This comprehensive toolset and methodological approach ensure robust, maintainable, and high-quality system design and implementation for fitness center operations.

## 1.8. Constraints

Several critical constraints shape the design, implementation, and deployment of Maxfit+:

### 1.8.1. Technical Constraints

**Technology Stack Commitment:**
The choice of ASP.NET Core 9.0 with Microsoft SQL Server, Entity Framework Core, and Razor Views is mandated by institutional technology standards and existing expertise within the development team. This limits the adoption of alternative frameworks or databases.

**Browser Compatibility:**
The system must support contemporary browsers (Chrome, Firefox, Edge) with no guarantee for legacy browser compatibility, due to resource prioritization.

**Infrastructure Limitations:**
Deployment must operate within the fitness center's existing IT infrastructure, which imposes constraints on server specifications, network bandwidth, and storage capacity.

## 1.9. Design Trade-offs

Design decisions for Maxfit+ involved careful evaluation of competing system attributes. The following key trade-offs were considered and justified:

### 1.9.1. Flexibility vs. Simplicity

A modular MVC architecture provides extensibility for future features (e.g., AI-based workout recommendations, wearable device integration, online payment gateways). However, to reduce initial system complexity and accelerate delivery, the first release limits configurability and advanced customization options.

Simplifying workflows enhances usability but reduces flexibility for power users; the design strikes a balance by allowing basic customization with a clean interface for member self-service and staff management.

### 1.9.2. Interoperability vs. Development Overhead

Full integration with external fitness platforms (e.g., MyFitnessPal, Strava) or third-party payment processors would streamline workflows but require significant development and maintenance resources.

Initial focus is on well-defined data models and standard-compliant database schemas to enable future integrations without overburdening the current release.

### 1.9.3. Performance vs. Security

Strict security controls, including password hashing, role-based access control, and CSRF protection, add processing overhead that may slightly impact system responsiveness.

Performance optimization is prioritized for critical paths such as class scheduling queries, payment transaction processing, and dashboard statistics through database indexing, eager loading with Entity Framework, and efficient query design to mitigate this impact.

### 1.9.4. Reliability vs. Rapid Feature Delivery

Emphasis on robust error handling, transactional integrity, and automated testing ensures system reliability and data consistency, even if it extends development time.

Features undergo staged rollout with validation phases to minimize risk while enabling continuous improvement based on user feedback from gym staff and members.

### 1.9.5. Usability vs. Security Controls

Implementing comprehensive role-based access control enforces strict permissions, which may increase user onboarding complexity.

To offset this, the UI incorporates intuitive dashboards, clear navigation menus, role-specific interfaces, and contextual guidance, improving user experience without compromising security.

These trade-offs represent deliberate prioritizations driven by project goals, fitness center operational needs, resource availability, and deployment environment. The architecture accommodates incremental enhancements to rebalance these attributes as the system evolves.

## 1.10. User Characteristics

The primary user community of the Maxfit+ Fitness Management System consists of fitness center personnel and members, including gym administrators, trainers, staff coordinators, and members engaged in class participation and facility usage. Users typically have varying levels of technical proficiency:

- **Gym Administrators:** Generally proficient with standard office software (MS Office, email clients) but may have limited experience with specialized gym management systems.
- **Staff and Trainers:** Moderate technical skills focused on scheduling, attendance tracking, and member interaction.
- **Members:** Varying technical proficiency, from tech-savvy users to those with basic computer literacy; primarily use the system for class registration and profile management.
- **IT Support Team:** Advanced technical expertise, responsible for system maintenance and troubleshooting.

Most users expect an intuitive and streamlined interface that minimizes learning curve and reduces manual workload.

### 1.10.1. User Problem Statement

Users currently face significant challenges in managing fitness center operations, including:

- **Manual Member Management:** Use of disconnected tools (spreadsheets, paper forms), resulting in data inconsistencies and errors.
- **Time-consuming Scheduling:** Difficulty in coordinating class schedules, trainer assignments, and room allocations, leading to conflicts and inefficiencies.
- **Lack of Centralized Data:** No unified system for tracking memberships, payments, check-ins, and class registrations, causing confusion and data duplication.
- **Limited Member Self-Service:** Members must visit the facility or call to register for classes, check schedules, or update profiles, increasing staff workload.
- **Manual Payment Tracking:** Paper-based or spreadsheet payment records increase risk of errors and make financial reporting difficult.

These problems result in wasted time, reduced operational efficiency, poor member experience, and increased administrative burden.

### 1.10.2. User Objectives

The users require a system that:

- Automates member registration and membership package management
- Supports multi-user access with role-based permissions (Admin, Staff, Member)
- Provides centralized class scheduling with conflict detection
- Maintains comprehensive payment tracking and financial reporting
- Allows members to self-register for classes and manage profiles
- Offers an intuitive web interface accessible across devices
- Ensures secure access, protecting member personal and financial data
- Enables real-time check-in/check-out tracking for attendance monitoring

**Wish List:**

- AI-assisted workout recommendations based on member goals and fitness level
- Integration with wearable devices (Fitbit, Apple Watch) for activity tracking
- Automated email/SMS notifications for class reminders and membership renewals
- Advanced analytics on member attendance patterns and revenue trends
- Mobile application for iOS and Android platforms

# 2. Background

The Maxfit+ Fitness Management System addresses the critical need for a centralized, automated solution for gym operations management. Traditionally, fitness center management has been manual and fragmented, utilizing disconnected spreadsheets, paper forms, and standalone software, leading to inefficiencies, data inconsistencies, and operational challenges. Maxfit+ consolidates member management, class scheduling, payment processing, attendance tracking, and staff coordination into a single integrated web-based platform.

By streamlining the entire operational workflow, Maxfit+ aims to improve productivity, accuracy, and member satisfaction, while ensuring standardized processes and data-driven decision-making. The system also enhances security and member data protection, which are essential for maintaining trust and regulatory compliance.

This project represents a digital transformation initiative aimed at modernizing fitness center operations through smart, automated solutions that support both administrative efficiency and enhanced member experience.

## 2.1. Overview of the System

Maxfit+ is a comprehensive web-based platform that enables gym administrators, staff, and members to:

**For Administrators and Staff:**
- Manage member registrations with comprehensive profiles and membership packages
- Create and coordinate class schedules with trainer assignments and room allocations
- Process payments through multiple methods with automated transaction tracking
- Monitor attendance through real-time check-in/check-out systems
- Generate financial reports and operational statistics
- Maintain equipment inventory and maintenance records
- Access role-based dashboards with real-time analytics

**For Members:**
- Register for available classes with capacity tracking
- View personalized dashboards with upcoming sessions and membership status
- Update personal profiles and upload photos
- Track check-in history and class participation
- Access class schedules and trainer information

**System Features:**
- Role-based access control with three distinct user types (Admin, Staff, Member)
- Automated conflict detection preventing double-booking of trainers and rooms
- Comprehensive audit trails for data changes and system activities
- Responsive web interface accessible across desktop and mobile devices
- Secure authentication and data encryption protecting sensitive member information

The major users are gym administrators who oversee all operations, trainers who manage classes and member interactions, and members who utilize self-service features for class registration and profile management. The system serves as the central hub for all fitness center operations, replacing fragmented manual processes with an integrated digital solution.

## 2.2. Overview of the Business Process

The Maxfit+ system supports the following core business processes:

1. **Member Registration and Management:** Administrators and staff register new members, assign membership packages, and maintain comprehensive member profiles with personal information and photos.

2. **Class Scheduling and Coordination:** Administrators create class schedules, assign trainers and rooms, with automated conflict detection preventing double-booking of resources.

3. **Member Self-Service Registration:** Members browse available classes, register for sessions with capacity tracking, and manage their own profiles through personalized dashboards.

4. **Payment Processing and Tracking:** Staff process payments through multiple methods (Cash, Credit Card, Bank Transfer, Online), with automated transaction tracking and receipt generation.

5. **Attendance Monitoring:** Real-time check-in/check-out system tracks member attendance, providing data for usage analytics and membership verification.

6. **Financial Reporting and Analytics:** Administrators generate revenue reports, track payment statuses, and analyze operational statistics including member demographics, class popularity, and attendance patterns.

Below is a high-level data flow describing the interaction among system components and users:

**Member Registration Flow:** Member Data → Registration Controller → Database → Membership Assignment → User Account Creation

**Class Scheduling Flow:** Class Information → Course Session Controller → Conflict Detection → Trainer/Room Assignment → Database Storage

**Payment Processing Flow:** Payment Details → Payment Controller → Transaction Validation → Database Recording → Financial Reporting

**Member Self-Service Flow:** Member Login → Dashboard → Class Browser → Registration Request → Capacity Check → Confirmation

### Table 4. Business Process

| **Process ID** | **Process Name** | **Description** | **Actors** |
|----------------|------------------|-----------------|------------|
| BP001 | Member Registration and Management | Register new members, assign membership packages, maintain profiles with personal information and photos | Administrators, Staff |
| BP002 | Class Scheduling and Coordination | Create class schedules, assign trainers and rooms with automated conflict detection | Administrators |
| BP003 | Member Self-Service Registration | Browse available classes, register for sessions with capacity tracking, update profiles | Members |
| BP004 | Payment Processing | Process payments through multiple methods (Cash, Credit Card, Bank Transfer, Online) with transaction tracking | Staff, Administrators |
| BP005 | Attendance Monitoring | Real-time check-in/check-out tracking for member attendance and usage analytics | Staff, Members |
| BP006 | Financial Reporting and Analytics | Generate revenue reports, track payment statuses, analyze operational statistics and demographics | Administrators |

# 3. Conceptual Design

## 3.1. Conceptual Application Design

### 3.1.1. Application Context

The system is designed as a centralized fitness management platform that interfaces with multiple external systems and services to provide seamless functionality. The core application handles user authentication, member management, class scheduling, payment processing, and attendance tracking.

**Application Context Diagram Overview:**

**Core System:** Centralized Fitness Management Application (Maxfit+ - subject of this design)

**External Systems:**
- **ASP.NET Core Identity Provider:** Handles user authentication, password hashing, and session management
- **Email Service Provider:** SMTP gateway for sending class reminders, membership notifications, and system alerts
- **SMS Gateway (Future):** SMS notifications for class reminders and urgent alerts
- **Payment Gateway (Future):** Online payment processing for credit card and digital wallet transactions

**Subsystems:**
- **Member Management Module:** Handles member registration, profile management, and membership package assignment
- **Class Scheduling Module:** Manages class schedules, trainer assignments, room allocations, and conflict detection
- **Payment Processing Module:** Processes payments through multiple methods and tracks financial transactions
- **Attendance Tracking Module:** Real-time check-in/check-out system with usage analytics
- **User Access Control Module:** Enforces RBAC policies (Admin, Staff, Member) and logs access events

**Data Stores:**
- **Microsoft SQL Server Database:** Stores core operational data including members, staff, classes, payments, and attendance records
- **File Storage System:** Holds member profile photos and system documents in the wwwroot/uploads directory
- **Session Storage:** Manages user sessions and authentication tokens

This architecture ensures modularity, scalability, and security by clearly defining boundaries between components and utilizing secure communication channels (HTTPS, encrypted connections) for all integrations. The MVC pattern separates presentation, business logic, and data access layers, promoting maintainability and testability.

## 3.2. Conceptual Data Design

### 3.2.1. Project Conceptual Data Model

The Maxfit+ Conceptual Data Model organizes the system's data into six distinct subject areas, each representing a core functional domain of the fitness management system. The model emphasizes entity relationships, data integrity, and normalization principles to ensure efficient data management and scalability.

**Subject Area 1: User Management & Security**
- **IdentityUser:** Core user authentication entity with email and password hash
- **IdentityRole:** Defines user roles (Admin, Staff, Member)
- **UserRole:** Junction table linking users to roles (many-to-many relationship)
- Relationships: Links to Member and Staff entities for extended profile information

**Subject Area 2: Membership Management**
- **Member:** Complete member profile including personal information and registration details
- **MembershipType:** Defines membership packages with pricing and duration
- **Membership:** Tracks active and historical memberships for each member
- Relationships: Members have multiple memberships over time; each linked to a membership type

**Subject Area 3: Class & Session Management**
- **Class:** Class types (Yoga, CrossFit, Pilates, etc.)
- **CourseSession:** Scheduled class instances with time, trainer, and room assignments
- **Room:** Facility rooms with capacity information
- **Staff:** Trainers and staff members who conduct classes
- **CourseRegistration:** Member enrollments in specific class sessions
- Relationships: Complex web connecting members, classes, trainers, rooms, and registrations

**Subject Area 4: Payment & Financial**
- **Payment:** Financial transactions with payment method, status, and amount tracking
- Relationships: Links to members and membership types for revenue tracking

**Subject Area 5: Attendance & Analytics**
- **CheckIn:** Real-time attendance tracking with check-in/check-out timestamps
- **MemberNotification:** System notifications and alerts for members
- Relationships: Both linked to members for usage analytics

**Subject Area 6: Equipment Management**
- **Equipment:** Gym equipment inventory
- **EquipmentMaintenance:** Maintenance records and service history
- Relationships: Tracks equipment lifecycle and maintenance costs

The conceptual data model diagram (see `conceptual_data_model.puml`) illustrates the entity relationships and cardinalities, forming the foundation for the physical database schema implementation in Microsoft SQL Server.

The detailed entity-relationship diagram (see `entity_relationship_diagram.puml`) provides a comprehensive view of all database entities with their complete attribute lists, primary keys, foreign keys, and relationship cardinalities, serving as the technical specification for database implementation and ORM mapping with Entity Framework Core.

### 3.2.2. Database Information

**Table 5. Database Inventory**

| **Database Name** | **Description** | **Type** | **Steward** |
|-------------------|-----------------|----------|-------------|
| MaxFit | Main application database storing all fitness management system data including users, members, staff, classes, payments, and attendance records | Create | Application Development Team |
| AspNetIdentity_DB | Existing ASP.NET Core Identity database containing user authentication, roles, and security claims | Interface | Application Security Team |
| file_storage_system | File storage system for managing member profile photos and system document metadata in wwwroot/uploads directory | Create | Application Development Team |
| backup_maxfit_db | Backup database for disaster recovery and data archival of fitness management system | Create | Database Administration Team |
| reporting_analytics_db | Data warehouse for analytics and reporting purposes, aggregating member statistics, revenue, and attendance patterns | Create | Business Intelligence Team |
| session_cache_db | Session storage database for managing user authentication tokens and temporary session data | Create | Application Development Team |

## 3.3. Conceptual Infrastructure Design

The Maxfit+ Fitness Management System will operate as a web-based application on the fitness center's existing IT infrastructure. The system is designed to serve gym administrators, staff, trainers, and members securely and reliably.

The system will operate in three main environments: The production environment will be available 24/7 at the fitness center for daily use by administrators, staff, and members accessing the system through web browsers. The test environment will be located in a separate area for system updates, new feature validation, and user acceptance testing before deployment. The development environment will be located in the IT department as a workspace for the software development team to implement new features and bug fixes.

Modern .NET technologies will be used as the technology infrastructure. The main application will be developed with the ASP.NET Core 9.0 MVC framework, and Microsoft SQL Server relational database system will be preferred as the database. For security, ASP.NET Core Identity will provide user authentication and authorization with role-based access control (Admin, Staff, Member). As special features of the system, automated class scheduling with conflict detection will prevent double-booking of trainers and rooms, comprehensive payment processing will support multiple payment methods with transaction tracking, and real-time check-in/check-out functionality will enable attendance monitoring and usage analytics.

The infrastructure will support concurrent access by multiple users with performance optimization through database indexing, eager loading with Entity Framework Core, and efficient query design. All communications will be secured using HTTPS encryption, and sensitive member data will be protected through secure password hashing and data encryption. The system architecture follows the MVC pattern, separating presentation (Razor Views), business logic (Controllers), and data access (Entity Framework Core Models) to ensure maintainability, testability, and scalability for future growth.

### 3.3.1. Special Technology

**Table 6. Special Technology Requirements**

| **Special Technology** | **Description** | **Notional Location** | **TRM Status** |
|------------------------|-----------------|----------------------|----------------|
| ASP.NET Core Identity Framework | Comprehensive security framework providing user authentication, password hashing, role-based authorization (Admin, Staff, Member), and protection against common web attacks | Application Servers in Fitness Center IT Infrastructure | Yes |
| Entity Framework Core 9.0 | Modern Object-Relational Mapping (ORM) framework for .NET, providing database abstraction, migrations, and LINQ query support for managing fitness center operational data | Application Servers in Fitness Center IT Infrastructure | Yes |
| Razor View Engine | Server-side rendering engine integrated within ASP.NET Core MVC, enabling dynamic HTML generation for member dashboards, class scheduling interfaces, and administrative panels | Application Servers in Fitness Center IT Infrastructure | Yes |
| Microsoft SQL Server | Enterprise-grade relational database management system providing ACID compliance, transactional integrity, and advanced indexing for maintaining data consistency in member, class, and payment relationships | Database Servers in Fitness Center IT Infrastructure | Yes |
| Kestrel Web Server | High-performance cross-platform web server for ASP.NET Core applications, providing HTTP request handling, SSL/TLS support, and efficient resource utilization | Web Tier Servers in Fitness Center IT Infrastructure | Yes |
| Bootstrap CSS Framework | Responsive front-end framework for building modern, mobile-first user interfaces with pre-built components and grid system for member and staff dashboards | Application Servers (Static Content) | Yes |

# 4. System Architecture

The Maxfit+ Fitness Management System is designed as a web-based application using modern software architecture principles. The system follows a layered architecture approach that separates different functional responsibilities into distinct layers for better maintainability and scalability.

The overall system architecture is based on the Model-View-Controller (MVC) pattern consisting of presentation, business logic, and data access layers. The presentation layer provides the user interface through web browsers using Razor Views, the business logic layer processes fitness center operations and business rules through Controllers, and the data layer manages all member, class, payment, and attendance information through Entity Framework Core Models. This separation allows each layer to be developed, maintained, and scaled independently.

The system uses ASP.NET Core 9.0 MVC framework as the core technology platform, which provides enterprise-level capabilities for web application development. The architecture supports role-based access control where different types of users (administrators, staff/trainers, members) have appropriate permissions to access specific system functions. Database integration is achieved through Microsoft SQL Server relational database system that stores all operational data including members, classes, payments, attendance records, and equipment inventory.

Key architectural components include the web interface for user interactions, business service modules for processing fitness center workflows (member registration, class scheduling, payment processing, attendance tracking), database access components managed through Entity Framework Core for data persistence, and specialized services for conflict detection in class scheduling and real-time check-in/check-out functionality. The system integrates with existing fitness center infrastructure including ASP.NET Core Identity for authentication and email service providers for notifications.

The deployment architecture is designed as a single web application that can be installed on fitness center servers, making it easy to deploy, maintain, and backup. The system provides security through HTTPS encrypted data transmission, secure user authentication with password hashing, role-based authorization, and comprehensive audit logging capabilities required for operational environments.

Integration capabilities allow the system to connect with external services such as email providers for member notifications and future integration with payment gateways for online transactions. The architecture supports future expansion through modular design patterns that allow new features (such as mobile applications, wearable device integration, or AI-based workout recommendations) to be added without disrupting existing functionality.

This architectural approach ensures the system is reliable, secure, and maintainable while meeting the specific needs of fitness centers for comprehensive operational management including member management, class scheduling, payment processing, and attendance tracking workflows.

## 4.1. Layered Architecture Model

The Maxfit+ system implements a five-layer architecture pattern that clearly separates concerns and promotes modularity:

**Presentation Layer:** User-facing interfaces built with Razor Views including Member Dashboard, Class Scheduling Interface, Payment Interface, and Reports Dashboard Interface. This layer handles user interactions and displays data.

**Controller Layer:** ASP.NET Core MVC Controllers (MembersController, CourseSessionsController, PaymentsController, ReportsController) that process HTTP requests, validate input, and coordinate between presentation and service layers.

**Service Layer:** Business logic components (Member Service, Class Scheduling Service, Payment Service, Check-in Service) that implement fitness center workflows, business rules, and orchestrate data operations.

**Data Access Layer:** Repository pattern implementations (Member Repository, Class Repository, Payment Repository, Attendance Repository) that abstract database operations and provide clean data access interfaces.

**Data Layer:** Physical data storage including Microsoft SQL Server database for operational data and File Storage system for member profile photos.

The layered architecture diagram (see `layered_architecture.puml`) illustrates the component interactions and data flows between layers, ensuring clear separation of responsibilities and supporting independent testing and maintenance of each layer.

## 4.2. Hardware Architecture

The Maxfit+ Fitness Management System operates on a centralized architecture, with core components hosted in the fitness center's IT infrastructure and user access provided via workstations, tablets, and mobile devices throughout the facility. The system infrastructure includes:

- Application servers running ASP.NET Core 9.0
- Microsoft SQL Server database servers for storing member, class, payment, and attendance data
- Kestrel web servers for handling user requests and session management
- Network storage systems for profile photos and system backups

The production environment consists of reliable and scalable hardware:

- Two application servers for high availability and load balancing
- Primary and backup database servers with automatic failover
- Web servers for traffic distribution and concurrent user support
- Centralized file storage systems for member photos and documents

Supporting environments include:

- A test system in a separate server for system updates and UAT
- A development environment in the IT department for feature development
- A facility-wide network of switches, wireless access points, and secure connectivity tools

User access is provided via:

- Administrative workstations in the management office
- Staff/trainer workstations at the front desk and training areas
- Member access through personal devices (laptops, tablets, smartphones) via web browsers
- Reception kiosks for member check-in/check-out

Data protection features include:

- Redundant network-attached storage (NAS) for file backups
- Automated daily database backups to secure storage
- UPS (Uninterruptible Power Supply) systems against power failures
- Firewall and network security for protecting sensitive member data
- HTTPS/SSL encryption for all data transmission

The network design follows a hierarchical model with the server room as the central hub, connected via Ethernet and wireless networks throughout the facility. The infrastructure includes:

- Gigabit Ethernet backbone for high-speed data transfer
- Wireless access points (Wi-Fi) for member and staff device connectivity
- VLAN segmentation for separating administrative and member networks
- Quality of Service (QoS) configuration for prioritizing critical operations

Security measures include:

- Hardware firewalls protecting the network perimeter
- Intrusion detection and prevention systems (IDS/IPS)
- Real-time monitoring tools for system performance and security alerts
- Access control systems for server rooms and network equipment
- Regular security audits and vulnerability assessments

Physical infrastructure protection includes:

- Climate-controlled server room with cooling systems
- Fire suppression systems for equipment protection
- Physical access controls (keycard entry, surveillance cameras)
- Environmental monitoring for temperature and humidity

This hardware setup ensures secure, reliable, and scalable system performance across the fitness center, supporting both current operational needs and future growth. The architecture can accommodate increasing user loads, expanding membership, and additional features without requiring major infrastructure changes.

The hardware architecture connectivity diagram (see `hardware_connectivity.puml`) illustrates the complete network topology, showing how components are interconnected from internet access through firewalls, core network switches, server infrastructure, user workstations, and storage systems, with appropriate security measures and redundancy at each layer.

## 4.3. Software Architecture

The Maxfit+ Fitness Management System is built on a layered, modular web architecture designed to streamline fitness center operations. The system uses C# with .NET 9.0 and ASP.NET Core MVC, ensuring scalability, maintainability, and modern web standards.

**Software Architecture Layers:**

**Presentation Layer:** Web UI with Razor Views (CSHTML), HTML5, CSS3, JavaScript, and Bootstrap 5 for responsive design. Provides role-specific dashboards for administrators, staff, and members.

**Business Logic Layer (Controllers):** Manages workflows such as user authentication, member registration, class scheduling with conflict detection, payment processing, and attendance tracking using ASP.NET Core MVC Controllers.

**Data Access Layer:** Uses Entity Framework Core 9.0 ORM with Repository pattern to interact with Microsoft SQL Server database, providing abstraction and clean data access interfaces.

**Data Layer:** Microsoft SQL Server database storing all operational data including members, staff, classes, payments, attendance records, and equipment inventory.

**Core Modules:**

**User Management:** Role-based access control with ASP.NET Core Identity (Admin, Staff, Member roles)

**Member Management:** Registration, profile management, membership packages, and photo uploads

**Class Scheduling:** Session management with automated conflict detection for trainers and rooms

**Payment Processing:** Multi-method payment support (Cash, Credit Card, Bank Transfer, Online) with transaction tracking

**Attendance Tracking:** Real-time check-in/check-out system with usage analytics

**Equipment Management:** Inventory tracking and maintenance record management

**Financial Reporting:** Revenue analytics, payment status tracking, and operational statistics

**Technology Stack:**

**Core Technologies:** C# 12, .NET 9.0, ASP.NET Core 9.0 MVC, Microsoft SQL Server

**Frontend:** Razor Views (CSHTML), Bootstrap 5, CSS3, JavaScript

**ORM & Data Access:** Entity Framework Core 9.0, LINQ

**Authentication & Security:** ASP.NET Core Identity, Cookie Authentication, HTTPS/SSL

**Development Tools:** Visual Studio 2022, Git version control, NuGet package manager

**Testing & Quality:** Unit testing frameworks, logging/monitoring tools

This modular architecture ensures clean separation of concerns, facilitates independent testing and maintenance of components, and supports future feature additions without disrupting existing functionality.

The software architecture module view diagram (see `software_module_view.puml`) provides a detailed structural perspective, illustrating all system components across the five architectural layers (Presentation, Controller, Service, Data Access, and Data), showing their relationships and data flows, and highlighting the modular design that enables independent development and testing of each component.

# 5. Human-Machine Interface

The Human-Machine Interface (HMI) for the Maxfit+ Fitness Management System has been carefully designed to ensure intuitive, accessible, and efficient interaction between users and the system. The user interface (UI) was developed using Razor Views (CSHTML), HTML5, CSS (Bootstrap 5 Framework), and JavaScript, with a focus on usability, responsiveness, and accessibility.

## User Profiles & Access Modes

**Administrator:** Access to all modules including system configuration, user management, member registration, staff management, class scheduling, payment processing, and full CRUD capabilities across all entities.

**Staff/Trainer:** Access limited to their assigned classes and member interactions. Can view schedules, process check-ins, manage class registrations, and update session information.

**Member:** Access to self-service features including class browsing, registration, profile management, payment history, and attendance tracking.

**Authentication:** Login interface supports cookie-based authentication with ASP.NET Core Identity, CSRF token protection, and password hashing for security.

Each user is presented with a role-specific UI dashboard that dynamically shows or hides menu items based on their access rights using `[Authorize(Roles = "...")]` directives.

## Interface Layout & Navigation

**Top Navigation Bar:** Includes links such as Home, Members, Classes, Staff, Payments, Check-ins, Equipment, and User Profile. Navigation adapts based on user role.

**Dashboard Widgets:** Role-specific dashboards display relevant statistics (today's check-ins, active members, revenue, upcoming sessions) with visual cards and charts.

**Breadcrumbs:** Used to indicate page hierarchy for user orientation across multi-level pages.

**Action Buttons:**
- Primary actions use Bootstrap-styled buttons (btn-primary, btn-success, btn-warning, btn-danger)
- Icons (FontAwesome) support visual clarity (e.g., ✏️ Edit, 🗑️ Delete, ✓ Check-in, 💳 Payment)

## Input and Output Forms

All CRUD operations are handled via user-friendly forms:

**Input fields:**
- Use proper type attributes (text, email, number, date, select, file for photo uploads)
- Required fields indicated with asterisks and data annotations (`[Required]`, `[EmailAddress]`)
- Client-side validation using jQuery Validation and server-side validation with ModelState
- Layouts use Bootstrap grid system (row, col-md-*) for alignment

**Buttons:** Create, Update, Delete, Cancel with distinct color coding and confirmation dialogs for destructive actions

**Tables:** DataTables used for record display with:
- Sorting by columns (Member ID, Name, Registration Date, etc.)
- Search functionality across all fields
- Pagination for large datasets
- Filtering options (Active/Inactive members, Payment status, Class types)
- Inline action buttons for quick access

## Responsive Design & Cross-Device Compatibility

**Web-first approach** using Bootstrap 5 ensures compatibility across:
- Desktop computers (1920x1080, 1366x768)
- Tablets (iPad, Android tablets)
- Smartphones (responsive mobile layout)

Elements automatically adjust using media queries and responsive grid system. Cards stack vertically on mobile, tables become scrollable, and navigation collapses into hamburger menu. Touch-friendly buttons sized appropriately for mobile interaction.

## Accessibility Features

- All forms and buttons include `aria-label` and `title` attributes for screen reader support
- Keyboard navigation supported for all major UI components (tab-based focus order)
- Color contrast checked for WCAG 2.1 AA compliance
- Icons include accessible text descriptions
- Form validation errors announced to screen readers
- Focus indicators visible on interactive elements

## Visual Aesthetics and UX Consistency

**Typography:** Clear sans-serif fonts like Segoe UI, Roboto, or system UI font for readability

**Color scheme:** Professional fitness-themed palette with semantic color coding:
- Blue (Primary Actions), Green (Success/Active), Red (Danger/Delete), Orange (Warning), Gray (Inactive/Disabled)

**Consistency:** Same layout structure and component styles used across all modules to reduce learning curve and improve user experience

**Dashboard Cards:** Statistics displayed in clean, modern card layouts with icons and numerical values prominently shown

## Error Handling & System Feedback

**Form Validation:**
- Client-side validation using jQuery Validation Unobtrusive
- Server-side validation with ModelState error messages rendered under input fields
- Model binding validation for data integrity

**User Feedback:**
- Toast notifications or modal alerts for actions (member created, payment processed, check-in successful)
- Confirmation popups before delete actions with clear warning messages
- Success/error messages with appropriate color coding
- 404, 403, and 500 error pages with user-friendly explanations

## Interface Examples

**Member Dashboard:**
- Personalized greeting with member name
- Upcoming registered classes displayed in card layout
- Quick links to Browse Classes, Update Profile, View Payment History
- Check-in history summary with attendance statistics

**Class Scheduling Interface:**
- Weekly calendar view showing all scheduled sessions
- Color-coded classes by type (Yoga, CrossFit, Pilates, etc.)
- Add Session button opens modal with form including:
  - Class dropdown, Trainer assignment, Room selection
  - Start/End time pickers with conflict detection
  - Capacity field with current registration count
- Edit/Delete actions with confirmation dialogs

**Payment Processing Page:**
- Member selection with autocomplete search
- Membership type dropdown with price auto-fill
- Payment method selection (Cash, Credit Card, Bank Transfer, Online)
- Amount field with validation
- Transaction status tracking table

## Planned UI Enhancements

- AJAX-based real-time updates for check-in status and class availability
- Dashboard analytics with charts (member growth, revenue trends, class popularity)
- Dark mode / light theme toggle for user preference
- Mobile application (iOS/Android) for member self-service
- QR code check-in for faster attendance tracking
- Drag-and-drop class scheduling interface
- Email/SMS notification integration with in-app notification center
- Multi-language support (Turkish/English localization)

The current Human-Machine Interface balances clarity, simplicity, and functionality. It ensures that users can easily manage fitness center operations such as member registration, class scheduling, payment processing, and attendance tracking with minimal technical training, while supporting future expandability and integration with external systems.

Activity diagrams illustrate key system workflows: The member registration process (see `member_registration_activity.puml`) shows the complete workflow from initial data entry through photo upload, membership package selection, payment processing, and user account creation. The class registration process (see `class_registration_activity.puml`) demonstrates the member self-service workflow including class browsing, conflict detection, capacity checking, and registration confirmation with automated notifications.

Sequence diagrams detail system interactions: The login sequence (see `login_sequence_diagram.puml`) illustrates the authentication flow between user, browser, AccountController, ASP.NET Core Identity managers (SignInManager, UserManager), and database, including role-based redirection to appropriate dashboards. The check-in/check-out sequence (see `checkin_sequence_diagram.puml`) shows the real-time attendance tracking workflow with member validation, duplicate check-in prevention, and timestamp recording for both entry and exit. The payment processing sequence (see `payment_sequence_diagram.puml`) demonstrates the complete payment workflow including form validation, transaction recording with multiple payment methods (Cash, Credit Card, Bank Transfer, Online), membership updates, and automated email receipt generation. The class scheduling sequence (see `class_scheduling_sequence_diagram.puml`) illustrates the session creation process with parallel conflict detection checking for trainer and room availability, preventing double-booking and ensuring schedule integrity.

## 5.1. Inputs

**Input Media:** Data entry screens accessible via web browser on desktop computers, tablets, and smartphones. Users can input data through keyboard, mouse, or touchscreen interfaces.

**Additional Devices:** Barcode scanners and QR code readers supported for member ID verification, check-in/check-out tracking, and equipment inventory management. Mobile devices can capture profile photos directly through camera integration.

**Input Forms:** Structured forms with field-level validation for data integrity. Examples include:
- Member registration forms (personal information, membership package selection, photo upload)
- Class scheduling forms (class type, trainer assignment, room allocation, time selection with conflict detection)
- Payment processing forms (member selection, payment method, amount, transaction details)
- Check-in/check-out forms (member identification, timestamp recording)
- Staff management forms (staff information, position, work schedules)
- Equipment inventory forms (equipment details, maintenance records)

**Access Restrictions:** Input screens are protected based on user roles using ASP.NET Core Identity authorization:
- Only administrators can create/edit staff members and system configuration
- Administrators and staff can process payments and manage member registrations
- Members can only update their own profiles and register for available classes
- Role-specific input fields dynamically shown/hidden based on user permissions

**Security Considerations:** 
- Input data transmitted over encrypted HTTPS/SSL channels
- All input fields sanitized to prevent SQL injection and XSS attacks
- Server-side validation with ModelState ensures data integrity
- CSRF token protection on all forms to prevent cross-site request forgery
- Password fields use secure hashing (ASP.NET Core Identity)
- File uploads (profile photos) validated for type and size restrictions
- Session timeout after inactivity to protect sensitive data
- Audit logging of critical operations (payment processing, member deletion, staff modifications)

## 5.2. Outputs

**Output Types:**

**Dynamic Data Displays:** Real-time screens including:
- Role-specific dashboards (Admin, Staff, Member) with live statistics
- Class schedules with real-time availability and capacity tracking
- Member attendance records and check-in/check-out logs
- Payment transaction histories and financial summaries
- Equipment inventory status and maintenance tracking

**Reports:** Both scheduled and on-demand reports including:
- Member registration and renewal summary reports
- Revenue and payment status reports (daily, weekly, monthly)
- Class attendance and popularity analytics
- Staff performance and session statistics
- Equipment maintenance and cost reports
- Member demographics and activity patterns

**Notifications:** Automated email alerts (with planned SMS support) triggered by system events:
- Class registration confirmations
- Upcoming class reminders (24 hours before)
- Membership expiration warnings (7 days, 3 days, 1 day before)
- Payment receipt confirmations
- Schedule changes or class cancellations
- Equipment maintenance due notifications

**Access Control:**

Outputs containing sensitive data are restricted to authorized users, enforced through Role-Based Access Control (RBAC) policies:
- Member financial data visible only to administrators, staff, and the member themselves
- Staff salary and performance data visible only to administrators
- System configuration and audit logs restricted to administrators
- Member personal information protected by role-specific views

**Reporting Requirements:**

**Daily:** 
- Check-in/check-out summary automatically generated for daily attendance tracking
- Revenue summary for daily financial reconciliation

**Weekly:** 
- Class schedules automatically emailed to all trainers and staff members
- Member registration summary sent to administrators

**Monthly:** 
- Financial compliance reports generated for accounting and auditing purposes
- Membership renewal forecasts for retention planning
- Equipment maintenance schedules and cost summaries

**Screen Design:**

Each screen presents context-specific data with filtering, sorting, and search capabilities:
- **Admin Dashboard:** Displays today's check-ins, active members count, revenue statistics, upcoming sessions, gender demographics, and recent member registrations
- **Member Dashboard:** Shows personalized greeting, upcoming registered classes, membership status, payment history, and check-in history
- **Class Scheduling Screen:** Calendar view with color-coded class types, trainer assignments, room allocations, capacity indicators, and conflict warnings
- **Payment Processing Screen:** Member selection with autocomplete, payment method options, transaction tracking table with status filters

**Design Documentation:**

All graphical layouts and data element definitions are maintained in the system design repository, including:
- Razor View templates (.cshtml files) with embedded C# logic
- Bootstrap component configurations and custom CSS styling
- JavaScript functions for dynamic interactions
- Entity models defining data structures
- Controller action methods specifying business logic

The use case diagram (see `use_case_diagram.puml`) provides a comprehensive view of system actors (Admin, Staff/Trainer, Member, External Systems) and their interactions with four primary modules (Admin Module, UI/UX Module, Business Module, Integration Module), illustrating the complete functional scope and user interactions within the Maxfit+ system.
