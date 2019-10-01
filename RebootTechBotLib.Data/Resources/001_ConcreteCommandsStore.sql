CREATE TABLE InformationalChatCommand 
(
	CommandId INTEGER PRIMARY KEY AutoIncrement,
	ChannelName nvarchar(50),
	CommandTrigger nvarchar(50), 
	CommandResponse TEXT,
	UserCreated nvarchar(50),
	DateCreated DateTime, 
	UserModified nvarchar(50), 
	DateModified DateTime,
	CommandPermissionRequired varchar(15),
	CoolDownSeconds INTEGER, 
	IsActive int
);
CREATE TABLE PeriodicChatSpeak
(
	SpeakId INTEGER PRIMARY KEY AutoIncrement,
	ChannelName nvarchar(50),
	SpeakText TEXT,
	CoolDownSeconds INTEGER,
	UserCreated nvarchar(50), 
	DateCreated DateTime, 
	UserModified nvarchar(50), 
	DateModified DateTime, 
	IsActive int
);
