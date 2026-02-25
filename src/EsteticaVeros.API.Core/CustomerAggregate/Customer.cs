using EsteticaVeros.API.Core.CustomerAggregate.Events;

namespace EsteticaVeros.API.Core.CustomerAggregate;

public class Customer(
  CustomerNumber customerNumber,
  string firstName,
  string lastName,
  string phoneNumber,
  Gender gender) : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public CustomerNumber CustomerNumber { get; private set; } = customerNumber;
  public string FirstName { get; private set; } = firstName;
  public string LastName { get; private set; } = lastName;
  public string PhoneNumber { get; private set; } = phoneNumber;
  public Gender Gender { get; private set; } = gender;
  public string? Email { get; private set; }
  public DateTime? DateOfBirth { get; private set; }
  public string? Address { get; private set; }
  public string? City { get; private set; }
  public string? Allergies { get; private set; }
  public string? MedicalConditions { get; private set; }
  public string? Medications { get; private set; }
  public int? PreferredAssistantId { get; private set; }
  public string? ProfilePictureUrl { get; private set; }
  public int LoyaltyPoints { get; private set; } = 0;
  public decimal TotalSpent { get; private set; } = 0;
  public int TotalVisits { get; private set; } = 0;
  public DateTime? LastVisitDate { get; private set; }
  public bool IsActive { get; private set; } = true;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }
  public int? CreatedByUserId { get; private set; }

  public string FullName => $"{FirstName} {LastName}";
  public int? Age => DateOfBirth.HasValue 
    ? (int)((DateTime.UtcNow - DateOfBirth.Value).TotalDays / 365.25) 
    : null;

  public Customer UpdateEmail(string? email)
  {
    if (Email == email) return this;
    Email = email;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer UpdateDateOfBirth(DateTime? dateOfBirth)
  {
    if (DateOfBirth == dateOfBirth) return this;
    DateOfBirth = dateOfBirth;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer UpdateContactInfo(string? address, string? city)
  {
    Address = address;
    City = city;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer UpdateMedicalInfo(string? allergies, string? medicalConditions, string? medications)
  {
    Allergies = allergies;
    MedicalConditions = medicalConditions;
    Medications = medications;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer UpdatePreferredAssistant(int? assistantId)
  {
    PreferredAssistantId = assistantId;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer UpdateCreatedBy(int userId)
  {
    CreatedByUserId = userId;
    return this;
  }

  public Customer UpdateBasicInfo(string firstName, string lastName, string phoneNumber, Gender gender)
  {
    if (FirstName == firstName && LastName == lastName && PhoneNumber == phoneNumber && Gender == gender) 
      return this;

    FirstName = firstName;
    LastName = lastName;
    PhoneNumber = phoneNumber;
    Gender = gender;
    UpdatedAt = DateTime.UtcNow;
    RegisterDomainEvent(new CustomerUpdatedEvent(this));
    return this;
  }

  public Customer UpdateProfilePicture(string? profilePictureUrl)
  {
    ProfilePictureUrl = profilePictureUrl;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer AddLoyaltyPoints(int points)
  {
    LoyaltyPoints += points;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer RedeemLoyaltyPoints(int points)
  {
    if (points > LoyaltyPoints)
    {
      throw new InvalidOperationException($"Cannot redeem {points} points. Customer only has {LoyaltyPoints} points.");
    }
    
    LoyaltyPoints -= points;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer RecordVisit(decimal amountSpent, int pointsEarned)
  {
    TotalVisits++;
    TotalSpent += amountSpent;
    LoyaltyPoints += pointsEarned;
    LastVisitDate = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer Activate()
  {
    if (IsActive) return this;
    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Customer Deactivate()
  {
    if (!IsActive) return this;
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    RegisterDomainEvent(new CustomerDeactivatedEvent(this));
    return this;
  }
}