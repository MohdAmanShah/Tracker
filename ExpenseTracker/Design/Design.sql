-- User 
--     Name
--     Email - UNIQUE
--     Password
--     UserId - Primary Key

-- Tracker
--     Name
--     Flag - 1: Joint, 0: Single USER
--     Link - Will be generated if Flag is 1, to invite people into tracker GROUP
--     OwnerId - FK - User TABLE
--     TrackerId - Primary KEY
--     Deleted - bool to mark entry as deleted but actually delete the data.

-- Expense
--     Name
--     Category: FK - Category TABLE
--     Amount
--     TrackerId - FK - Tracker TABLE
--     DATETIME
--     ExpenseId - Primary Key

-- Category
--     NAMES
--     CategoryId - Primary Key
