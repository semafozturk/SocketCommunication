using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocketCommunication.Api.Models;

namespace SocketCommunication.Api.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(11);
            builder.Property(x => x.UserName).HasMaxLength(50);
            builder.Property(x => x.UserSurname).HasMaxLength(50);
            builder.Property(x => x.UserInfo).HasMaxLength(50);
            builder.ToTable("Users");
        }
    }
}
