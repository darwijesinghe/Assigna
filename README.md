# Assigna

## Project Purpose

This simple task management application was made to showcase my coding ability to those who would like to hire me as a .NET developer.

This app was mainly developed to demonstrate basic CRUD operations. This full app was designed and developed by myself. Assigna can use for a small team to simply manage their project tasks.

Assigna live link - [https://assigna.azurewebsites.net](https://assigna.azurewebsites.net)

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

- First Name: Manudi
- Username: manudi@lead
- Email Address
  - team.mail.lead@gmail.com
  - team@lead123
- Password: team@lead123

Team Member

- First Name: Peter
- Username: peter@work
- Email Address
  - team.mail.member@gmail.com
  - team@member123
- Password: team@member123

## Support

Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License

This project is licensed under the terms of the **MIT** license.
