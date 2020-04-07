namespace Visitor_Registration_Data.newHoang1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model11")
        {
        }

        public virtual DbSet<tbl_Approval> tbl_Approval { get; set; }
        public virtual DbSet<tbl_BlackList> tbl_BlackList { get; set; }
        public virtual DbSet<tbl_Department_Infor> tbl_Department_Infor { get; set; }
        public virtual DbSet<tbl_Permisstion> tbl_Permisstion { get; set; }
        public virtual DbSet<tbl_Request_Infor> tbl_Request_Infor { get; set; }
        public virtual DbSet<tbl_Role> tbl_Role { get; set; }
        public virtual DbSet<tbl_Taxi_Approval_Infor> tbl_Taxi_Approval_Infor { get; set; }
        public virtual DbSet<tbl_Taxi_Department_Infor> tbl_Taxi_Department_Infor { get; set; }
        public virtual DbSet<tbl_Taxi_Request_Infor> tbl_Taxi_Request_Infor { get; set; }
        public virtual DbSet<tbl_Taxi_User_Infor> tbl_Taxi_User_Infor { get; set; }
        public virtual DbSet<tbl_User> tbl_User { get; set; }
        public virtual DbSet<tbl_User_Role> tbl_User_Role { get; set; }
        public virtual DbSet<tbl_Visitor_Infor> tbl_Visitor_Infor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_Approval>()
                .Property(e => e.ApproverId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_BlackList>()
                .Property(e => e.NationalId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_BlackList>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Department_Infor>()
                .Property(e => e.Head_id)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Department_Infor>()
                .Property(e => e.Deputy_id)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Department_Infor>()
                .HasMany(e => e.tbl_Approval)
                .WithRequired(e => e.tbl_Department_Infor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Permisstion>()
                .HasMany(e => e.tbl_Role)
                .WithMany(e => e.tbl_Permisstion)
                .Map(m => m.ToTable("tbl_Role_Permisstion").MapLeftKey("Permisstion_Id").MapRightKey("Role_Id"));

            modelBuilder.Entity<tbl_Request_Infor>()
                .Property(e => e.EmployeeId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Request_Infor>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Request_Infor>()
                .HasMany(e => e.tbl_Approval)
                .WithRequired(e => e.tbl_Request_Infor)
                .HasForeignKey(e => e.Request_Infor_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Role>()
                .HasMany(e => e.tbl_User_Role)
                .WithRequired(e => e.tbl_Role)
                .HasForeignKey(e => e.Role_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Taxi_Approval_Infor>()
                .Property(e => e.ApproverId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Taxi_Department_Infor>()
                .Property(e => e.Head_id)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Taxi_Department_Infor>()
                .Property(e => e.Deputy_id)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Taxi_Department_Infor>()
                .HasMany(e => e.tbl_Taxi_Approval_Infor)
                .WithRequired(e => e.tbl_Taxi_Department_Infor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Taxi_Request_Infor>()
                .Property(e => e.EmployeeId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Taxi_Request_Infor>()
                .HasMany(e => e.tbl_Taxi_Approval_Infor)
                .WithRequired(e => e.tbl_Taxi_Request_Infor)
                .HasForeignKey(e => e.Request_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Taxi_Request_Infor>()
                .HasMany(e => e.tbl_Taxi_User_Infor)
                .WithRequired(e => e.tbl_Taxi_Request_Infor)
                .HasForeignKey(e => e.Taxi_Request_Infor_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Taxi_User_Infor>()
                .Property(e => e.EmployeeId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Taxi_User_Infor>()
                .Property(e => e.Cost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tbl_Taxi_User_Infor>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Taxi_User_Infor>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.EmployeeId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .Property(e => e.FushanAd)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_User>()
                .HasMany(e => e.tbl_Request_Infor)
                .WithRequired(e => e.tbl_User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_User>()
                .HasMany(e => e.tbl_Taxi_Request_Infor)
                .WithRequired(e => e.tbl_User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_User>()
                .HasMany(e => e.tbl_User_Role)
                .WithRequired(e => e.tbl_User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_User_Role>()
                .Property(e => e.EmployeeId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Visitor_Infor>()
                .Property(e => e.NationalId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Visitor_Infor>()
                .Property(e => e.EscorterId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Visitor_Infor>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);
        }
    }
}
