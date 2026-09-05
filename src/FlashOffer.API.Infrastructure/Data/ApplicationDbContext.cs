using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace FlashOffer.API.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly ICurrentUserService _currentUserService;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
		: base(options)
	{
		_currentUserService = currentUserService;
	}

    DatabaseFacade IApplicationDbContext.Database => Database;
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
	public DbSet<GroupBuyingRequest> GroupBuyingRequests { get; set; }
	public DbSet<OfferRequest> OfferRequests { get; set; }
    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Partner> Partners { get; set; }
    public DbSet<PartnerProduct> PartnerProducts { get; set; }
    public DbSet<PartnerCommission> PartnerCommissions { get; set; }
    public DbSet<SocialPost> SocialPosts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<BusinessField> BusinessFields { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<AuthAuditLog> AuthAuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Apply all entity configurations from this assembly
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		// Global query filter for soft delete
		foreach (var entityType in modelBuilder.Model.GetEntityTypes())
		{
			if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
			{
				var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
				var property = System.Linq.Expressions.Expression.Property(parameter, "IsDeleted");
				var condition = System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false));
				var lambda = System.Linq.Expressions.Expression.Lambda(condition, parameter);

				entityType.SetQueryFilter(lambda);
			}
		}
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var currentUserId = _currentUserService.UserId;
		var currentUserName = _currentUserService.UserName;

		// 📝 Audit log: capture TRƯỚC khi soft-delete / gán CreatedBy/UpdatedBy
		// để bắt đúng action Delete (Deleted) và snapshot dữ liệu gốc.
		var auditLogs = AuditLogHelper.CreateAuditLogs(
			ChangeTracker, currentUserId, currentUserName, _currentUserService.IpAddress);

		var entries = ChangeTracker.Entries<BaseEntity>();

		foreach (var entry in entries)
		{
			// Soft delete: convert Deleted to Modified with IsDeleted = true
			if (entry.State == EntityState.Deleted)
			{
				entry.State = EntityState.Modified;
				entry.Entity.IsDeleted = true;
				entry.Entity.UpdatedAt = DateTime.UtcNow;
				entry.Entity.UpdatedBy = currentUserName ?? currentUserId ?? "System";
				continue;
			}

			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedAt = DateTime.UtcNow;
				entry.Entity.CreatedBy = currentUserName ?? currentUserId ?? "System";
			}

			if (entry.State == EntityState.Modified)
			{
				entry.Entity.UpdatedAt = DateTime.UtcNow;
				entry.Entity.UpdatedBy = currentUserName ?? currentUserId ?? "System";
			}
		}

		if (auditLogs.Count > 0)
			AuditLogs.AddRange(auditLogs);

		return await base.SaveChangesAsync(cancellationToken);
	}
}
