CREATE TABLE BlogPosts
(
	PostId INT IDENTITY NOT NULL PRIMARY KEY,
	BlogId INT NOT NULL,
	PostTitle VARCHAR(85) NOT NULL,
	MessageContent VARCHAR(255) NOT NULL,
	CreatedDate DATETIME2 DEFAULT CURRENT_TIMESTAMP NOT NULL
);

/*
	IDENTITY tells SQL Server to automatically insert a auto incremented number.
	So you can insert a blog using 
	INSERT INTO BlogPosts(BlogId, PostTitle, MessageContent) VALUES(1, 'Post #1', 'Hello World!') and it would automatically
	populate BlogId with a new number.


	DEFAULT tells SQL Server to automatically insert a default value if no value was provided in the 
	INSERT SQL statement. So you can insert a blog using INSERT INTO Blogs(BlogTitle) VALUES('Blog #1') and it would
	automatically populate CreatedDate with today's date


	POPULATE for TESTING:
	INSERT INTO BlogPosts(BlogId, PostTitle, MessageContent)
	VALUES((SELECT TOP 1 BlogId FROM Blogs), 'My first post', 'Hello World!');

	INSERT INTO BlogPosts(BlogId, PostTitle, MessageContent)
	VALUES((SELECT TOP 1 BlogId FROM Blogs), 'My second post', 'test');

	INSERT INTO BlogPosts(BlogId, PostTitle, MessageContent)
	VALUES((SELECT TOP 1 BlogId FROM Blogs), 'My third post', 'Hello 2!');
*/

