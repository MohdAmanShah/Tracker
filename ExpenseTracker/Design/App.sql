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
    EmailVerified BIT DEFAULT 0,
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

CREATE TABLE tblUserTracker
(
    UserId INT NOT NULL,
    TrackerId INT NOT NULL,
    AccessDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_tblUserTracker_UserId FOREIGN KEY(UserId) REFERENCES tblUser(UserId),
    CONSTRAINT FK_tblUserTracker_TrackerId FOREIGN KEY(TrackerId) REFERENCES tblTracker(TrackerId),
    CONSTRAINT PK_UserTracker PRIMARY KEY (UserId, TrackerId)
);


INSERT INTO tblCategory
    (CategoryName)
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


SELECT CategoryName
FROM tblCategory;

ALTER TABLE tblUser
ADD EmailVerified BIT DEFAULT 0;


SELECT UserName, Email, EmailVerified, [Password], Salt
FROM tblUser;
use expensetracker;
SELECT name
FROM sys.tables;
SELECT *
FROM sys.databases;

use ExpenseTracker;


select *
from tblCategory;



INSERT INTO tblUser
    (UserName, Email, [Password], Salt, EmailVerified)
VALUES
    ('John Doe', 'john.doe@example.com', 0xabcdef, 0x123456, 1),
    ('Jane Smith', 'jane.smith@example.com', 0xbcdefa, 0x654321, 1),
    ('Mohd Aman Shah', 'aman.shah@example.com', 0xabcdef, 0xabcdef, 1),
    ('Alice Brown', 'alice.brown@example.com', 0xabcdef, 0x123123, 0),
    ('Bob Grey', 'bob.grey@example.com', 0xabcdef, 0x987654, 0),
    ('Charlie White', 'charlie.white@example.com', 0xabcdef, 0x654321, 1),
    ('Daniel Black', 'daniel.black@example.com', 0xabcdef, 0xabcdef, 0),
    ('Eve Green', 'eve.green@example.com', 0xabcdef, 0x123456, 1),
    ('Fred Yellow', 'fred.yellow@example.com', 0xabcdef, 0x654321, 1),
    ('Grace Blue', 'grace.blue@example.com', 0xabcdef, 0xabcdef, 0);

INSERT INTO tblTracker
    (TrackerName, Flag, Link, OwnerId)
VALUES
    ('Personal Finance', 1, 'http://yourapp.com/invite/a1b2c3d4', 1),
    ('Travel Expenses', 0, NULL, 2),
    -- Not sharable, no link generated
    ('Home Budget', 1, 'http://yourapp.com/invite/i9j0k1l2', 3),
    ('Project Costs', 0, NULL, 4),
    -- Not sharable, no link generated
    ('Monthly Bills', 1, 'http://yourapp.com/invite/q7r8s9t0', 5),
    ('Event Management', 0, NULL, 6),
    -- Not sharable, no link generated
    ('Car Maintenance', 1, 'http://yourapp.com/invite/y5z6a7b8', 7),
    ('Medical Bills', 0, NULL, 8),
    -- Not sharable, no link generated
    ('Investment Tracker', 1, 'http://yourapp.com/invite/g3h4i5j6', 9),
    ('Freelance Work', 0, NULL, 10);
-- Not sharable, no link generated

-- Users joining the 'Personal Finance' tracker (TrackerId = 1)
INSERT INTO tblUserTracker
    (UserId, TrackerId)
VALUES
    (2, 1),
    -- Jane Smith joins 'Personal Finance'
    (3, 1);
-- Mohd Aman Shah joins 'Personal Finance'

-- Users joining the 'Home Budget' tracker (TrackerId = 3)
INSERT INTO tblUserTracker
    (UserId, TrackerId)
VALUES
    (4, 3),
    -- Alice Brown joins 'Home Budget'
    (5, 3);
-- Bob Grey joins 'Home Budget'

-- Users joining the 'Monthly Bills' tracker (TrackerId = 5)
INSERT INTO tblUserTracker
    (UserId, TrackerId)
VALUES
    (6, 5),
    -- Charlie White joins 'Monthly Bills'
    (7, 5);
-- Daniel Black joins 'Monthly Bills'

-- Users joining the 'Car Maintenance' tracker (TrackerId = 7)
INSERT INTO tblUserTracker
    (UserId, TrackerId)
VALUES
    (8, 7),
    -- Eve Green joins 'Car Maintenance'
    (9, 7);
-- Fred Yellow joins 'Car Maintenance'

-- Users joining the 'Investment Tracker' tracker (TrackerId = 9)
INSERT INTO tblUserTracker
    (UserId, TrackerId)
VALUES
    (10, 9),
    -- Grace Blue joins 'Investment Tracker'
    (1, 9);
-- John Doe joins 'Investment Tracker'


INSERT INTO tblExpense
    (ExpenseName, Amount, CategoryId, TrackerId)
VALUES
    ('Groceries', 100, 1, 1),
    ('Flight Tickets', 500, 2, 2),
    ('Electricity Bill', 150, 3, 3),
    ('Hotel Stay', 300, 4, 4),
    ('Water Bill', 80, 5, 5),
    ('Software License', 200, 6, 6),
    ('Car Repair', 250, 7, 7),
    ('Medical Checkup', 100, 8, 8),
    ('Stock Purchase', 1000, 9, 9),
    ('Freelance Payment', 750, 10, 10);



select *
from tblCategory;
select *
from tblExpense;
select *
from tblTracker;
select *
from tblUser;
select *
from tblUserTracker;

-- DECLARE @TrackerId INT;
DECLARE @TrackerId INT;
SET @TrackerId = 2;
SELECT DISTINCT
    t.TrackerId,
    t.TrackerName,
    t.Flag,
    t.TrackerDeleted AS IsMarkedDeleted,
    t.Link,
    u.UserId AS Owner_UserId,
    u.UserName AS Owner_UserName,
    u.Email AS Owner_Email,
    u.EmailVerified AS Owner_EmailVerified,
    (SELECT
        ut.UserId,
        ut.TrackerId,
        ut.AccessDate
    FROM tblUserTracker ut
    WHERE ut.TrackerId = t.TrackerId
    FOR JSON PATH) AS userTrackers
FROM tblTracker t
    INNER JOIN tblUser u ON t.OwnerId = u.UserId
    LEFT JOIN tblUserTracker ut ON t.TrackerId = ut.TrackerId
WHERE t.TrackerId = @TrackerId
FOR JSON PATH;

    SELECT DISTINCT
        t.TrackerId,
        t.TrackerName,
        t.Flag,
        t.TrackerDeleted AS IsMarkedDeleted,
        t.Link,
        (SELECT TOP 1
            UserId,
            UserName,
            Email,
            EmailVerified AS IsEmailVerified
        FROM tblUser
        WHERE UserId = t.OwnerId
        FOR JSON PATH) AS Owner,
        (SELECT
            ut.UserId,
            ut.TrackerId,
            ut.AccessDate
        FROM tblUserTracker ut
        WHERE ut.TrackerId = t.TrackerId
        FOR JSON PATH) AS UserTrackers
    FROM tblTracker t
        INNER JOIN tblUser u ON t.OwnerId = u.UserId
        LEFT JOIN tblUserTracker ut ON t.TrackerId = ut.TrackerId
    FOR JSON PATH;





        SELECT *
        FROM tblTracker
            INNER JOIN tblExpense ON tblTracker.TrackerId = tblExpense.TrackerId;

        SELECT TrackerId, COUNT(*) AS Expenses
        FROM tblExpense
        GROUP BY TrackerId;



        SELECT *
        FROM
            tblTracker
            INNER JOIN
            tblUserTracker
            ON
tblTracker.TrackerId = tblUserTracker.TrackerId;


        SELECT *
        FROM tblUserTracker;

        DECLARE @userid INT;
        SET @userid =1
        ;
        SELECT UserName, Email
        FROM tblUser
        WHERE UserId = @userid;
        SELECT TrackerName, Flag, Link, OwnerId, tblTracker.TrackerId, AccessDate
        FROM
            tblTracker
            INNER JOIN
            tbluserTracker
            ON tblTracker.TrackerId = tblUserTracker.TrackerId
                AND UserId = @userid
                AND TrackerDeleted = 0;