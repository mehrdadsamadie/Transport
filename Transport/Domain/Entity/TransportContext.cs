namespace Domain.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TransportContext : DbContext
    {
        static TransportContext()
        {
            Database.SetInitializer<TransportContext>(null);
        }
        public TransportContext()
            : base("name=TransportEntity")
        {
        }


        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<DailyDay> DailyDays { get; set; }
        public virtual DbSet<DailyDriver> DailyDrivers { get; set; }
        public virtual DbSet<DailyTime> DailyTimes { get; set; }
        public virtual DbSet<DelayPrice> DelayPrices { get; set; }
        public virtual DbSet<Distance> Distances { get; set; }
        public virtual DbSet<DistancePrice> DistancePrices { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DriverHistory> DriverHistories { get; set; }
        public virtual DbSet<DriverType> DriverTypes { get; set; }
        public virtual DbSet<DriverTypePrice> DriverTypePrices { get; set; }
        public virtual DbSet<Eparchy> Eparchies { get; set; }
        public virtual DbSet<Factory> Factories { get; set; }
        public virtual DbSet<FactoryUnit> FactoryUnits { get; set; }
        public virtual DbSet<FactoryUserManager> FactoryUserManagers { get; set; }
        public virtual DbSet<Guaranty> Guaranties { get; set; }
        public virtual DbSet<LicensePlateType> LicensePlateTypes { get; set; }
        public virtual DbSet<OverTime> OverTimes { get; set; }
        public virtual DbSet<Path> Paths { get; set; }
        public virtual DbSet<Personnel> Personnels { get; set; }
        public virtual DbSet<PersonStatusType> PersonStatusTypes { get; set; }
        public virtual DbSet<RDriverDriverType> RDriverDriverTypes { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<SalaryDay> SalaryDays { get; set; }
        public virtual DbSet<SalaryMonth> SalaryMonths { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceStatu> ServiceStatus { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Suggestion> Suggestions { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaxiCompany> TaxiCompanies { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleDriver> VehicleDrivers { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<ManagerPersonel> ManagerPersonels { get; set; }
        public virtual DbSet<Way> Waies { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasMany(e => e.Factories)
                .WithRequired(e => e.Address)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.RequestsBeginingAddress)
                .WithOptional(e => e.BeginingAddress)
                .HasForeignKey(e => e.BiginningAddressId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.RequestsDestinationAddress)
                .WithOptional(e => e.DestinationAddress)
                .HasForeignKey(e => e.DestinationAddressId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.ServicesBeginingAddress)
                .WithOptional(e => e.BeginingAddress)
                .HasForeignKey(e => e.Biginning);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.ServicesDestinationAddress)
                .WithOptional(e => e.DestinationAddress)
                .HasForeignKey(e => e.Destination);

            modelBuilder.Entity<Personnel>()
                .HasMany(e => e.Managers)
                .WithRequired(e => e.Manager)
                .HasForeignKey(e => e.ManagerPersonelId);

            modelBuilder.Entity<Personnel>()
                .HasMany(e => e.ManagerPersonels)
                .WithRequired(e => e.Personnel)
                .HasForeignKey(e => e.PersonelId);


            modelBuilder.Entity<Bank>()
                .HasOptional(e => e.Bank1)
                .WithRequired(e => e.Bank2);

            modelBuilder.Entity<Contract>()
                .HasMany(e => e.Guaranties)
                .WithRequired(e => e.Contract)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DailyDay>()
                .HasMany(e => e.DailyDrivers)
                .WithRequired(e => e.DailyDay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DailyTime>()
                .HasMany(e => e.DailyDays)
                .WithRequired(e => e.DailyTime)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Distance>()
                .HasMany(e => e.DistancePrices)
                .WithRequired(e => e.Distance)
                .HasForeignKey(e => e.DistanseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Distance>()
                .HasMany(e => e.Paths)
                .WithRequired(e => e.Distance)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DistancePrice>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DistancePrice>()
                .HasMany(e => e.Services)
                .WithRequired(e => e.DistancePrice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Driver>()
                .Property(e => e.Stamp)
                .IsFixedLength();

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.Driver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.DailyDrivers)
                .WithRequired(e => e.Driver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.DriverHistories)
                .WithRequired(e => e.Driver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.RDriverDriverTypes)
                .WithRequired(e => e.Driver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.Services)
                .WithRequired(e => e.Driver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.VehicleDrivers)
                .WithRequired(e => e.Driver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DriverType>()
                .HasMany(e => e.DelayPrices)
                .WithRequired(e => e.DriverType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DriverType>()
                .HasMany(e => e.DriverTypePrices)
                .WithRequired(e => e.DriverType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DriverType>()
                .HasMany(e => e.RDriverDriverTypes)
                .WithRequired(e => e.DriverType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DriverTypePrice>()
                .Property(e => e.DailyPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Eparchy>()
                .HasMany(e => e.Addresses)
                .WithRequired(e => e.Eparchy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Factory>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Factory>()
                .Property(e => e.CellPhone)
                .IsFixedLength();

            modelBuilder.Entity<Factory>()
                .HasMany(e => e.FactoryUnits)
                .WithRequired(e => e.Factory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactoryUnit>()
                .Property(e => e.Telephone)
                .IsFixedLength();

            modelBuilder.Entity<FactoryUnit>()
                .HasMany(e => e.FactoryUserManagers)
                .WithRequired(e => e.FactoryUnit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactoryUnit>()
                .HasMany(e => e.Personnels)
                .WithRequired(e => e.FactoryUnit)
                .HasForeignKey(e => e.FactoryUnitId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactoryUnit>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.FactoryUnit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Guaranty>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OverTime>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Path>()
                .HasMany(e => e.Services)
                .WithRequired(e => e.Path)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Path>()
                .HasMany(e => e.Waies)
                .WithRequired(e => e.Path)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personnel>()
                .Property(e => e.NationalCode)
                .IsFixedLength();

            modelBuilder.Entity<Personnel>()
                .Property(e => e.CellPhone)
                .IsFixedLength();

            modelBuilder.Entity<Personnel>()
                .Property(e => e.Mobile)
                .IsFixedLength();

            modelBuilder.Entity<Personnel>()
                .Property(e => e.EmergencyPhone)
                .IsFixedLength();

            modelBuilder.Entity<Personnel>()
                .HasMany(e => e.Drivers)
                .WithRequired(e => e.Personnel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personnel>()
                .HasMany(e => e.FactoryUserManagers)
                .WithRequired(e => e.Personnel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personnel>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Personnel)
                .HasForeignKey(e => e.PersonelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonStatusType>()
                .Property(e => e.StatusName)
                .IsFixedLength();

            modelBuilder.Entity<PersonStatusType>()
                .HasMany(e => e.DailyDrivers)
                .WithRequired(e => e.PersonStatusType)
                .HasForeignKey(e => e.StatusTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalaryDay>()
                .Property(e => e.SumService)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryDay>()
                .Property(e => e.DailySalary)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.SumDay)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.PriceDay)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.SumHours)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.SumService)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.PriceService)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.SumOverTime)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.PriceOverTime)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.SumDelay)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.PriceDelay)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.SumRush)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.PriceRush)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.SumDeductions)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.PriceDeductions)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SalaryMonth>()
                .Property(e => e.TotalPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Service)
                .HasForeignKey(e => e.ServiceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceStatu>()
                .HasMany(e => e.Services)
                .WithRequired(e => e.ServiceStatu)
                .HasForeignKey(e => e.ServiceStatusId);

            modelBuilder.Entity<State>()
                .HasMany(e => e.Eparchies)
                .WithRequired(e => e.State)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Eparchy>()
                .HasMany(e => e.Suggestions)
                .WithRequired(e => e.Eparchy)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Vehicle>()
                .Property(e => e.TechnicalCheckupNumber)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.Vehicle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.VehicleDrivers)
                .WithRequired(e => e.Vehicle)
                .WillCascadeOnDelete(false);

        }
    }
}
