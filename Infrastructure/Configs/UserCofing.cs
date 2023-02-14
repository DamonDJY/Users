using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Users.Infrastructure.Configs
{
    internal class UserCofing : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_Users");
            builder.OwnsOne(x => x.PhoneNumber, nb => {
                nb.Property(x => x.RegionCode).HasMaxLength(5).IsUnicode(false);
                nb.Property(x => x.Number).HasMaxLength(20).IsUnicode(false);
            });
            builder.Property("passwordHash").HasMaxLength(100).IsUnicode(false);
            builder.HasOne(x => x.UserAcessFail).WithOne(x => x.User)
                .HasForeignKey<UserAcessFail>(x => x.UserId);
            
        }
    }
}
