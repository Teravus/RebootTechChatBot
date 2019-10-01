PRAGMA temp_store = 2;
CREATE TABLE TwitchUser
(
	Id INTEGER Primary Key AutoIncrement,
	UserId varchar(10), 
	UserName nvarchar(100),
	DisplayName nvarchar(100),
	UserType INTEGER, 
	IsTurbo INTEGER,
	FirstTimeSeen DateTime, 
	LastSeen DateTime, 
	ChatTime INTEGER,
	ReferringStreamer nvarchar(100), 
	TotalTimesSeen INTEGER,
	TotalChatMessages INTEGER,
	TotalWhisperMessages INTEGER, 
	UserScore INTEGER, 
	ProcessStatus INTEGER	
);
CREATE TABLE TwitchFollow
(
	Id INTEGER Primary Key AutoIncrement,
	FromUserId INTEGER, 
	ToUserId INTEGER, 
	FollowDate DateTime
);
CREATE TABLE UserProcess
(
	ProcessId INTEGER Primary Key AutoIncrement,
	ProcessName varchar(50),
	ChannelId varchar(10)
);
CREATE TABLE UserProcessStatus
(
	ProcessStatusId INTEGER Primary Key AutoIncrement,
	ProcessId INTEGER,
	PriorityId INTEGER,
	ProcessStatusName varchar(20)
);
CREATE TABLE UserProcessHistory
(
	ProcessHistoryId INTEGER Primary Key AutoIncrement,
	ProcessId INTEGER, 
	ProcessStatusId INTEGER,
	CompletedDate DateTime,
	ChatTime INTEGER,
	TotalChats INTEGER,
	TotalWhispers INTEGER, 
	TotalTimesSeen INTEGER, 
	Category nvarchar(50),
	StreamTitle nvarchar(300),
	UserScore INTEGER
);

CREATE TEMP TABLE _Variables (VarName TEXT, VarValue INTEGER);

INSERT INTO _Variables(VarName, VarValue) VALUES ('UserProcessId',0);

INSERT INTO UserProcess
(ProcessName)
VALUES
('Road To Follow');

UPDATE _Variables
set VarValue = last_insert_rowid()
where VarName = 'UserProcessId';

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Seen', 10);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Chat', 20);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Whisper', 30);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'GetPoints', 40);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Follow', 50);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Host', 60);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Raid', 70);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Bits', 80);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Dontation', 90);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'PrimeSub', 100);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Sub', 100);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Resub', 110);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Gift', 120);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Redemptions', 130);

INSERT INTO UserProcessStatus
(ProcessId,ProcessStatusName, PriorityId)
VALUES
((select VarValue from _Variables where VarName='UserProcessId'),'Merch', 140);

DROP TABLE _Variables;