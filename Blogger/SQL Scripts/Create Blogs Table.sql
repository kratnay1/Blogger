CREATE TABLE Blogs
(
	BlogId INT IDENTITY NOT NULL PRIMARY KEY,
	BlogTitle VARCHAR(85) NOT NULL,
	CreatedDate DATETIME2 DEFAULT CURRENT_TIMESTAMP NOT NULL
);

/*
	IDENTITY tells SQL Server to automatically insert a auto incremented number.
	So you can insert a blog using INSERT INTO Blogs(BlogTitle) VALUES('Blog #1') and it would automatically
	populate BlogId with a new number.


	DEFAULT tells SQL Server to automatically insert a default value if no value was provided in the 
	INSERT SQL statement. So you can insert a blog using INSERT INTO Blogs(BlogTitle) VALUES('Blog #1') and it would
	automatically populate CreatedDate with today's date


	POPULATE for TESTING:
	INSERT INTO Blogs(BlogTitle) VALUES ('My Blog');
*/

