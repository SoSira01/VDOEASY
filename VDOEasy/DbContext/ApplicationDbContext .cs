using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VDOEasy.Models;

namespace VDOEasy.Dbcontext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet properties for each entity
        public DbSet<MasMemberType> MasMemberTypes { get; set; }
        public DbSet<MasMovieType> MasMovieTypes { get; set; }
        public DbSet<MasBranches> MasBranches { get; set; }
        public DbSet<TrnMembers> TrnMembers { get; set; }
        public DbSet<TrnMembersMovieType> TrnMembersMovieTypes { get; set; }


        // Get all TrnMembers
        public IEnumerable<TrnMembers> GetTrnMembers()
        {
            return TrnMembers.FromSqlRaw("EXEC [dbo].[sp_sel_trnMembers]").ToList();
        }

        // Get all MasMemberType
        public IEnumerable<MasMemberType> GetMemberType()
        {
            return MasMemberTypes.FromSqlRaw("EXEC [dbo].[sp_sel_masMemberType]").ToList();
        }

        // Get all MasMovieType
        public IEnumerable<MasMovieType> GetMasMovieType()
        {
            return MasMovieTypes.FromSqlRaw("EXEC [dbo].[sp_sel_masMoviesType]").ToList();
        }

        // Get all MasBranches
        public IEnumerable<MasBranches> GetMasBranches()
        {
            return MasBranches.FromSqlRaw("EXEC [dbo].[sp_sel_masBranches]").ToList();
        }

        // Insert TrnMembersMovieType
        public void InsertTrnMembersMovieType(int movieTypeId, int memberId)
        {
            Database.ExecuteSqlRaw("EXEC [dbo].[sp_ins_trnMembersMovieType] @MovieTypeID, @MemberID",
                new SqlParameter("@MovieTypeID", movieTypeId),
                new SqlParameter("@MemberID", memberId));
        }

        // Insert TrnMembers
        public void InsertTrnMembers(int branchId, string firstName, string lastName, DateTime birthDate, string address, string idCardNumber, int memberTypeId)
        {
            Database.ExecuteSqlRaw("EXEC [dbo].[sp_ins_trnMembers] @BranchID, @Firstname, @Lastname, @Birthdate, @Address, @IDCardNumber, @MemberTypeID",
                new SqlParameter("@BranchID", branchId),
                new SqlParameter("@Firstname", firstName),
                new SqlParameter("@Lastname", lastName),
                new SqlParameter("@Birthdate", birthDate),
                new SqlParameter("@Address", address),
                new SqlParameter("@IDCardNumber", idCardNumber),
                new SqlParameter("@MemberTypeID", memberTypeId));
        }

        // Update TrnMembers
        public async Task<int> UpdateTrnMembers(int memberId, int branchId, string firstName, string lastName, DateTime birthdate, string address, string idCardNumber, int memberTypeId)
        {
            var parameters = new[]
            {
               new SqlParameter("@MemberID", memberId),
               new SqlParameter("@BranchID", branchId),
               new SqlParameter("@Firstname", firstName),
               new SqlParameter("@Lastname", lastName),
               new SqlParameter("@Birthdate", birthdate),
               new SqlParameter("@Address", address),
               new SqlParameter("@IDCardNumber", idCardNumber),
               new SqlParameter("@MemberTypeID", memberTypeId)
            };

            return await Database.ExecuteSqlRawAsync("EXEC [dbo].[sp_upd_trnMembers] @MemberID, @BranchID, @Firstname, @Lastname, @Birthdate, @Address, @IDCardNumber, @MemberTypeID", parameters);
        }

        // Delete TrnMembersMovieType
        public void DeletetrnMembersMovieTypeById(int id)
        {
            Database.ExecuteSqlRaw("EXEC [dbo].[sp_del_trnMembersMovieTypeByID] @MemberID",
                new SqlParameter("@MemberID", id));
        }

        //update IsActive status of TrnMembers
        public void UpdateTrnMembersIsActiveById(int id, bool isActive)
        {
            var idParam = new SqlParameter("@ID", id);
            var isActiveParam = new SqlParameter("@IsActive", isActive);

            Database.ExecuteSqlRaw("EXEC [dbo].[sp_upd_trnMembersIsActiveByID] @ID, @IsActive", idParam, isActiveParam);
        }

        // Get TrnMembers by ID
        public IEnumerable<TrnMembers> GetTrnMembersById(int memberId)
        {
            return TrnMembers.FromSqlRaw("EXEC [dbo].[sp_sel_trnMembersByID] @MemberID",
                new SqlParameter("@MemberID", memberId)).ToList();
        }

        // Get TrnMembersMovieType by Member ID
        public IEnumerable<TrnMembersMovieType> GetTrnMembersMovieTypeById(int Id)
        {
            return TrnMembersMovieTypes.FromSqlRaw("EXEC [dbo].[sp_sel_trnMembersMovieTypeByID] @ID",
                                    new SqlParameter("@ID", Id)).ToList();
        }

    }
}
