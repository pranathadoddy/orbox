﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.Store
{
    public class CreateModel
    {
        [Required]
        [Display(Name = "City", ResourceType = typeof(LocationResource))]
        public ulong CityId { get; set; }

        public SelectList Cities { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(LocationResource))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address", ResourceType = typeof(LocationResource))]
        public string Address { get; set; }

        [Required]
        [Display(Name = "MapUrl", ResourceType = typeof(LocationResource))]
        public string MapUrl { get; set; }
    }
}
