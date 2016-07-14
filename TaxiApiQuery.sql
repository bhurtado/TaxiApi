GO
Create Table Color(
Id int identity not null Primary key,
Metalic bit not null,
ColorName nvarchar(50) not null
)
GO
Create Table Fuel(
id int identity not null primary key,
FuelType nvarchar(50) not null
)
Go
Create Table Manufacturer(
Id int identity not null primary key,
ManufactName nvarchar(50) not null
)
GO
Create Table Model(
Id int identity not null primary key,
ModelType nvarchar(50) not null
)
GO
Create Table Employee(
Id int identity not null primary key,
FirstName nvarchar(50) not null,
LastName nvarchar(50) not null,
UserName nvarchar(7) not null,
Pword nvarchar(100) not null,
Salt nvarchar(100) not null,
[Admin] bit not null

)
GO

Create Table Vehicle(
RegNr nvarchar(6) not null primary key,
Manufact_Id int not null foreign key references Manufacturer(Id),
Color_Id int not null foreign key references Color(Id),
Model_Id int not null foreign key references Model(Id),
YearModel int not null,
TripMeter int not null
)
GO
Create Table VehicleFuel(
Id int identity not null primary key,
Fuel_Id int not null foreign key references Fuel(Id),
Vehicle_Id nvarchar(6) not null foreign key references Vehicle(RegNr)
)
GO
Create Table Maintenance(
Id int identity not null primary key,
Vehicle_Id nvarchar(6) not null foreign key references Vehicle(Regnr),
MaintenanceType nvarchar(100) not null,
MaintenanceCost decimal(18,2) not null,
ReportDate Date not null
)
Go
Create Table Report(
Id int identity not null primary key,
Employee_Id int not null foreign key references Employee(Id),
Vehicle_Id nvarchar(6) not null foreign key references Vehicle(RegNr),
PrimaryFuel_Id int not null foreign key references VehicleFuel(Id),
PrimaryFuelAmount decimal(18,2) not null,
PrimaryFuelUnitPrice decimal(18,2) not null,
SecondaryFuel_Id int null foreign key references VehicleFuel(Id),
SecondaryFuelAmount int null foreign key references VehicleFuel(Id),
SecondaryFuelUnitPrice int null foreign key references VehicleFuel(Id),
ReportDate DateTime not null,
TotalPrice Decimal(18,2) not null,
Distance int not null
)

--drop table report
--drop table employee
--Drop table maintenance
--drop table vehiclefuel
--drop table vehicle
--drop table model
--drop table manufacturer
--drop table fuel
--drop table color