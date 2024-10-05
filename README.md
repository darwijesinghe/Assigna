# Assigna

## Project Purpose

This simple task management application was built to demonstrate my coding skills to potential employers interested in hiring me as a .NET developer.

This application was created to showcase basic CRUD operations. Designed and developed by myself, Assigna is ideal for small teams to manage their project tasks efficiently. It can also be scaled to include more advanced features as needed.

## Contributors

- Darshana Wijesinghe

## Application Structure

### Front-end design

- HTML 5
- CSS 3
- JavaScript ES6

### Back-end design

- .NET Core 3.1
  - C #

### Architecture

- Three-Tier Architecture
  - Domain Layer
  - Service Layer
  - Web Layer

### Database

- Microsoft SQL Server EF Core

### Pages and access control

- Sign In / Sign Up
  - Local accounts
  - External accounts (Google)
- New task
  - Only for the team lead
- All tasks
  - Team lead can access
  - Team member has access only for own tasks
- Pending tasks
  - Team lead can access
  - Team member has access only for own tasks
- Complete tasks
  - Team lead can access
  - Team member has access only for own tasks
- High / Medium / Low priority
  - Team lead can access
  - Team member has access only for own tasks
- Sign Out

## App Features

- Add a new task
  - This can do within only team lead account
- Tasks are categorized
  - Pending tasks
  - Complete tasks
  - High priority
  - Medium priority
  - Low priority
- Role based access
  - team-lead
  - team-member
- Tasks are prioritized
  - High
  - Medium
  - Low
- View task information
  - Assigner can Edit the task if the task is still pending
  - Assigner can Delete a task whether it is completed or not
  - Assignee can add a Note for the task if the task is still not done
- Task count
  - Every user can see live task count
- Send warning email
  - Team lead can send a task completion warning to the assignee via email
- Alert
  - At end of the every operation, user will be notified

## Test Accounts

Team Lead

- First Name: Harshi
- Username: harshi@lead
- Email Address: Any email address
- Password: team@lead123

Team Member

- First Name: Nadeesha
- Username: nadeesha@work
- Email Address: Any email address
- Password: team@member123

## Support

Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License

This project is licensed under the terms of the **MIT** license.
