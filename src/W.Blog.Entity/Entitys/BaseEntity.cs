﻿namespace W.Blog.Entity.Entitys
{
    using System;
    using W.Blog.Entity.Enums;

    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DeleteEnum IsDelete { get; set; } = DeleteEnum.Normal;

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
