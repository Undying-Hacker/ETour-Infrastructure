﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.InterfaceImpls
{
    [Index(nameof(Title), IsUnique = true)]
    public class Post : IPost<Employee>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string CoverImgUrl { get; set; }
        public int ID { get; set; }
        public bool IsSoftDeleted { get; set; }
        public DateTime LastUpdated { get; set; }
        public Employee Owner { get; set; }
        public string OwnerID { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public IPost<Employee>.PostCategory Category { get; set; }
        public List<string> Tags { get; set; } = new();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<string> GetUnusedImageUrls(List<string> newUrls)
        {
            return ImageUrls.Where(url => !newUrls.Contains(url));
        }

        public void Hide()
        {
            IsSoftDeleted = true;
        }

        public void Show()
        {
            IsSoftDeleted = false;
        }
    }
}
