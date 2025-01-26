# InsuranceClaimVisualiser

# InsuranceClaimVisualiser
# A C# program providing graphical aides for an SQL Server database containing information on policies and claims 

Starting the program:  
This is a Windows Only Program  
Add your SQL Server Username and Password to System Environment Variables under SQL_SERVER_USERNAME and SQL_SERVER_PASSWORD  
Adjust server name and port number from programs.cs if needed  

Spinning up the database - Use these queries on an SQL Server of your choice:  
--INITIALISE DATABASE  
CREATE DATABASE PolicyClaimsTrackerDB  

--Regions Bridge Table  
CREATE TABLE Regions (  
	RegionID int IDENTITY(1, 1) PRIMARY KEY,  
	RegionName VARCHAR(50) NOT NULL  
	);  

-- PolicyTypes Bridge Table  
CREATE TABLE PolicyTypes (  
    PolicyTypeID INT IDENTITY(1, 1) PRIMARY KEY,  
    PolicyName VARCHAR(50) NOT NULL  
);  

-- Policies Table  
CREATE TABLE Policies (  
    PolicyID INT IDENTITY(1, 1) PRIMARY KEY,  
    PolicyTypeID INT FOREIGN KEY REFERENCES PolicyTypes(PolicyTypeID),  
    RegionID INT FOREIGN KEY REFERENCES Regions(RegionID),  
    PremiumCollected DECIMAL(15, 2)  
);  

CREATE TABLE Claims (  
	ClaimID int IDENTITY(1, 1) PRIMARY KEY,  
	PolicyID int FOREIGN KEY references Policies(PolicyID),  
	ClaimAmount DECIMAL(15, 2),  
	ClaimDate DATE  
	);  

-- Insert data into PolicyTypes  
INSERT INTO PolicyTypes (PolicyName) VALUES   
('Home Insurance'),  
('Car Insurance'),  
('Life Insurance'),  
('Travel Insurance'),  
('Health Insurance');  

-- Insert data into Regions table  
INSERT INTO Regions (RegionName) VALUES   
('UK'),  
('Europe'),  
('Asia'),  
('North America'),  
('Australia');  

-- Insert data into Policies table  
INSERT INTO Policies (RegionID, PremiumCollected, PolicyTypeID) VALUES   
-- UK Policies  
(1, 1200.00, 1),  
(1, 950.00, 2),  
(1, 1800.00, 3),  
(1, 400.00, 4),  
(1, 700.00, 5),  
-- Europe Policies  
(2, 2000.00, 1),  
(2, 1000.00, 2),  
(2, 2500.00, 3),  
(2, 600.00, 4),  
(2, 800.00, 5),  
-- Asia Policies  
(3, 1500.00, 1),  
(3, 1200.00, 2),  
(3, 500.00, 4),  
(3, 750.00, 5),  
-- North America Policies  
(4, 1300.00, 1),  
(4, 1100.00, 2),  
(4, 3000.00, 3),  
-- Australia Policies  
(5, 1400.00, 1),  
(5, 950.00, 2),  
(5, 550.00, 4),  
(5, 800.00, 5);  

-- Insert data into Claims table (claims only for some policies)  
INSERT INTO Claims (PolicyID, ClaimAmount, ClaimDate) VALUES   
-- UK Claims  
(1, 500.00, '2025-01-10'), -- Home Insurance (UK)  
(2, 300.00, '2025-01-15'), -- Car Insurance (UK)  
-- Europe Claims  
(6, 1000.00, '2025-01-20'), -- Home Insurance (Europe)  
(7, 800.00, '2025-01-22'), -- Car Insurance (Europe)  
-- Asia Claims  
(13, 500.00, '2025-01-18'), -- Travel Insurance (Asia)  
-- North America Claims  
(15, 700.00, '2025-01-25'), -- Home Insurance (North America)  
-- Australia Claims  
(18, 900.00, '2025-01-28'); -- Car Insurance (Australia)  
