DROP TABLE IF EXISTS Log;
		
CREATE TABLE Log
		LogId 					INT				        			NOT NULL 		PRIMARY KEY AUTO_INCREMENT
	,	Date 					TIMESTAMP         			NOT NULL 		DEFAULT CURRENT_TIMESTAMP()  
	,	Thread 				VARCHAR(255) 				NOT NULL 
	,	Level 					VARCHAR(20) 				NOT NULL
	,	Logger 				VARCHAR(255) 				NOT NULL
	,	Message 			VARCHAR(4000) 			NOT NULL 
	,	StackTrace 		VARCHAR(4000) 			NULL 
)