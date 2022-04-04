namespace IssueSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using IssueSystem.Data.Models;

    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> image)
        {
            image.HasOne(x => x.EmployeePicture)
                .WithOne(x => x.ProfilePicture)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
