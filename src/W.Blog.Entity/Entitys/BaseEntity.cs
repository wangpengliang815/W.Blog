namespace W.Blog.Entity.Entitys
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using W.Blog.Entity.Enums;

    public abstract class BaseEntity
    {
        public int Id { get; set; }

        [Column(Order = 100)]
        public DeleteEnum IsDelete { get; set; }

        [Column(Order = 101)]
        public DateTime CreateTime { get; set; }
    }
}
