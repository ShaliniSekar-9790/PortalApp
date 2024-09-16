CREATE TABLE dbo.Category(
Category_Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
Category_Name varchar(100)
);

CREATE TABLE NewsInfos(
News_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
Title varchar(300),
News_Description varchar(600),
Create_Date datetime,
Updated_Date datetime,
Category_Id INT NOT NULL 
constraint fk_newsinfo_category foreign key (Category_Id) references Category(Category_Id)
);

drop constraint fk_newsinfo_category;

