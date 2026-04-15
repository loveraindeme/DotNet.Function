using SqlSugar;
using SugarSqlCore.Abstraction.Entities;
using SugarSqlCore.Entities;

namespace DotNet.SugarSqlCore.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    [SugarTable("user")]
    public class User : Entity<Guid>, ICreationAuditedEntity, IUpdationAuditedEntity, IDeletionAuditedEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public override Guid Id { get; protected set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public Guid? LastModifierId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        public Guid? DeleterId { get; set; }

        /// <summary>
        /// 是否删除，0：未删除，1：已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
