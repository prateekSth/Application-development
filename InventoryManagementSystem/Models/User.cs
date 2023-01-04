﻿namespace InventoryManagementSystem.Models;

public class User
{
    public Guid Id { get; set; }  = Guid.NewGuid();

    public string Username { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public bool HasInitialPassword { get; set; } = true;

    public Role Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public Guid CreatedBy { get; set; } 
}
