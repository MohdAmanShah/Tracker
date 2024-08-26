CREATE DATABASE ExpenseTracker;
USE ExpenseTracker;

CREATE TABLE tblCategory
(
    CategoryName nvarchar(50) NOT NULL UNIQUE,
    CategoryId INT PRIMARY KEY IDENTITY(1,1)
);

CREATE TABLE tblUser
(
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName nvarchar(40) NOT NULL,
    Email varchar(50) UNIQUE NOT NULL,
    [Password] BINARY(60),
    Salt BINARY(60)
);

CREATE TABLE tblTracker
(
    TrackerName NVARCHAR(40) NOT NULL,
    Flag BIT DEFAULT 0,
    TrackerDeleted BIT DEFAULT 0,
    Link NVARCHAR(255),
    OwnerId INT NOT NULL,
    TrackerId INT PRIMARY KEY IDENTITY(1,1),
    CONSTRAINT FK_tblTracker_OwnerId FOREIGN KEY(OwnerId) REFERENCES tblUser(UserId)
);

CREATE TABLE tblExpense
(
    ExpenseId INT PRIMARY KEY IDENTITY(1,1),
    ExpenseName NVARCHAR(50) NOT NULL,
    Amount INT CHECK (Amount > 0),
    ExpenseDeleted BIT DEFAULT 0,
    CategoryId INT NOT NULL,
    TrackerId INT NOT NULL,
    CONSTRAINT FK_tblTracker_CategoryId FOREIGN KEY(CategoryId) REFERENCES tblCategory(CategoryId),
    CONSTRAINT FK_tblTracker_TrackerId FOREIGN KEY(TrackerId) REFERENCES tblTracker(TrackerId)
);


INSERT INTO tblCategory (CategoryName)
VALUES 
('Groceries'),
('Restaurants'),
('Coffee Shops'),
('Snacks'),
('Fuel'),
('Public Transit'),
('Parking'),
('Taxis/Rideshare'),
('Rent/Mortgage'),
('Utilities'),
('Internet/Cable'),
('Maintenance/Repairs'),
('Phone Bill'),
('Electricity'),
('Water'),
('Gas'),
('Internet'),
('Movies'),
('Concerts'),
('Subscriptions'),
('Games'),
('Gym Memberships'),
('Doctor Visits'),
('Medications'),
('Supplements'),
('Haircuts'),
('Skincare'),
('Cosmetics'),
('Toiletries'),
('Clothing'),
('Electronics'),
('Home Goods'),
('Gifts'),
('Flights'),
('Hotels'),
('Car Rentals'),
('Tours/Activities'),
('Books'),
('Courses'),
('School Fees'),
('Supplies'),
('Health Insurance'),
('Car Insurance'),
('Home Insurance'),
('Life Insurance'),
('Credit Card Payments'),
('Loan Payments'),
('Mortgage Payments'),
('Savings Deposits'),
('Investments'),
('Retirement Contributions'),
('Donations'),
('Charity'),
('Unexpected Expenses');


SELECT CategoryName FROM tblCategory;