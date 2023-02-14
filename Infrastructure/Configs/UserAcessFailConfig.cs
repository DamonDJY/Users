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
    internal class UserAcessFailConfig : IEntityTypeConfiguration<UserAcessFail>
    {
        public void Configure(EntityTypeBuilder<UserAcessFail> builder)
        {
            builder.ToTable("T_UserAcessFails");
            builder.Property("isLookOut");
        }
    }
}
