/*
 * HCore Identity Auth API
 *
 * The HCore Identity Auth API provides the most common methods to handle authentication server side using ASP.NET Identity Core.
 *
 * OpenAPI spec version: 1.0.0-s2
 * Contact: holzner@invest-fit.at
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HCore.Identity.Resources;

namespace HCore.Identity.Models
{ 
    /// <summary>
    /// The information required to register the new user
    /// </summary>
    [DataContract]
    [NotMapped]
	public partial class UserSpec : IEquatable<UserSpec>
    { 
		private string _Email;

        /// <summary>
        /// The email address of the user
        /// </summary>
        /// <value>The email address of the user</value>
        [Required(ErrorMessageResourceType = typeof(Translations.Resources.Messages), ErrorMessageResourceName = "email_missing")]
        [Display(ResourceType = typeof(Messages), Name = "email_address")]
        [DataMember(Name = "email")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Translations.Resources.Messages), ErrorMessageResourceName = "email_invalid")]
        public string Email { get => _Email; set { _Email = value; EmailSet = true; } }
		
		public bool EmailSet = false;

        private string _FirstName;

        /// <summary>
        /// The first name of the user
        /// </summary>
        /// <value>The first name of the user</value>
        [Required(ErrorMessageResourceType = typeof(Translations.Resources.Messages), ErrorMessageResourceName = "first_name_missing")]
        [Display(ResourceType = typeof(Messages), Name = "first_name")]
        [DataMember(Name = "first_name")]
        public string FirstName { get => _FirstName; set { _FirstName = value; FirstNameSet = true; } }

        public bool FirstNameSet = false;

        private string _LastName;

        /// <summary>
        /// The last name of the user
        /// </summary>
        /// <value>The last name of the user</value>
        [Required(ErrorMessageResourceType = typeof(Translations.Resources.Messages), ErrorMessageResourceName = "last_name_missing")]
        [Display(ResourceType = typeof(Messages), Name = "last_name")]
        [DataMember(Name = "last_name")]
        public string LastName { get => _LastName; set { _LastName = value; LastNameSet = true; } }

        public bool LastNameSet = false;

        private string _Password;

        /// <summary>
        /// The password of the user
        /// </summary>
        /// <value>The password of the user</value>
        [Required(ErrorMessageResourceType = typeof(Translations.Resources.Messages), ErrorMessageResourceName = "password_missing")]
        [Display(ResourceType = typeof(Messages), Name = "password")]
        [DataMember(Name = "password")]
        public string Password { get => _Password; set { _Password = value; PasswordSet = true; } }
		
		public bool PasswordSet = false;		

		private string _PasswordConfirmation;

        /// <summary>
        /// The password confirmation
        /// </summary>
        /// <value>The password confirmation</value>
        [Required(ErrorMessageResourceType = typeof(Translations.Resources.Messages), ErrorMessageResourceName = "password_confirmation_missing")]
        [Display(ResourceType = typeof(Messages), Name = "password_confirmation")]
        [DataMember(Name = "password_confirmation")]
        [Compare("Password", ErrorMessageResourceType = typeof(Translations.Resources.Messages), ErrorMessageResourceName = "password_confirmation_no_match")]
        public string PasswordConfirmation { get => _PasswordConfirmation; set { _PasswordConfirmation = value; PasswordConfirmationSet = true; } }
		
		public bool PasswordConfirmationSet = false;

        private string _PhoneNumber;

        /// <summary>
        /// The phone number of the user
        /// </summary>
        /// <value>The phone number of the user</value>
        [Required]
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; PhoneNumberSet = true; } }

        public bool PhoneNumberSet = false;

        private bool? _PhoneNumberConfirmed;

        /// <summary>
        /// Indicates if the phone number of the user has already been confirmed
        /// </summary>
        /// <value>Indicates if the phone number of the user has already been confirmed</value>
        [DataMember(Name = "phone_number_confirmed")]
        public bool? PhoneNumberConfirmed { get => _PhoneNumberConfirmed; set { _PhoneNumberConfirmed = value; PhoneNumberConfirmedSet = true; } }

        public bool PhoneNumberConfirmedSet = false;
      
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserSpec {\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  FirstName: ").Append(FirstName).Append("\n");
            sb.Append("  LastName: ").Append(LastName).Append("\n");
            sb.Append("  Password: ").Append(Password).Append("\n");
            sb.Append("  PasswordConfirmation: ").Append(PasswordConfirmation).Append("\n");
            sb.Append("  PhoneNumber: ").Append(PhoneNumber).Append("\n");
            sb.Append("  PhoneNumberConfirmed: ").Append(PhoneNumberConfirmed).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((UserSpec)obj);
        }

        /// <summary>
        /// Returns true if UserSpec instances are equal
        /// </summary>
        /// <param name="other">Instance of UserSpec to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserSpec other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Email == other.Email ||
                    Email != null &&
                    Email.Equals(other.Email)
                ) &&
                (
                    FirstName == other.FirstName ||
                    FirstName != null &&
                    FirstName.Equals(other.FirstName)
                ) &&
                (
                    LastName == other.LastName ||
                    LastName != null &&
                    LastName.Equals(other.LastName)
                ) &&
                (
                    Password == other.Password ||
                    Password != null &&
                    Password.Equals(other.Password)
                ) && 
                (
                    PasswordConfirmation == other.PasswordConfirmation ||
                    PasswordConfirmation != null &&
                    PasswordConfirmation.Equals(other.PasswordConfirmation)
                ) &&
                (
                    PhoneNumber == other.PhoneNumber ||
                    PhoneNumber != null &&
                    PhoneNumber.Equals(other.PhoneNumber)
                ) &&
                (
                    PhoneNumberConfirmed == other.PhoneNumberConfirmed ||
                    PhoneNumberConfirmed != null &&
                    PhoneNumberConfirmed.Equals(other.PhoneNumberConfirmed)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Email != null)
                        hashCode = hashCode * 59 + Email.GetHashCode();
                    if (FirstName != null)
                        hashCode = hashCode * 59 + FirstName.GetHashCode();
                    if (LastName != null)
                        hashCode = hashCode * 59 + LastName.GetHashCode();
                    if (Password != null)
                        hashCode = hashCode * 59 + Password.GetHashCode();
                    if (PasswordConfirmation != null)
                    hashCode = hashCode * 59 + PasswordConfirmation.GetHashCode();
                    if (PhoneNumber != null)
                        hashCode = hashCode * 59 + PhoneNumber.GetHashCode();
                    if (PhoneNumberConfirmed != null)
                        hashCode = hashCode * 59 + PhoneNumberConfirmed.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UserSpec left, UserSpec right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserSpec left, UserSpec right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
