select column_name from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Categories';

select name from sys.tables;
select count(name) as tables from sys.tables;

select  top 1 *  from Categories;


select count(categoryid) from categories;

select * from categories;

select * from 
Employees;

select * from Categories, Employees;


select * from orders;

select * from customers;

select count(customerid) from customers;
select count(orderid) from orders;

select customerid, count(orderid) as orders from orders group by CustomerId having count(orderid) > 5;