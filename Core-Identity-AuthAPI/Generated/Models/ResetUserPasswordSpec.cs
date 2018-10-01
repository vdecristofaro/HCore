/*
 * RHCore Identity Auth API
 *
 * The RHCore Identity Auth API provides the most common methods to handle authentication server side using ASP.NET Identity Core.
 *
 * OpenAPI spec version: 1.0.0-s2
 * Contact: holzner@invest-fit.at
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ReinhardHolzner.Core.Identity.AuthAPI.Generated.Models
{ 
    /// <summary>
    /// The information required to reset the password for the user
    /// </summary>
    [DataContract]
    [NotMapped]
	public partial class ResetUserPasswordSpec : IEquatable<ResetUserPasswordSpec>
    { 
		private string _Email;
		
		/// <summary>
        /// The email address of the user
        /// </summary>
        /// <value>The email address of the user</value>
        [Required]
        [DataMember(Name="email")]
		public string Email { get => _Email; set { _Email = value; EmailSet = true; } }
		
		public bool EmailSet = false;		

		private string _Password;
		
		/// <summary>
        /// The password of the user
        /// </summary>
        /// <value>The password of the user</value>
        [Required]
        [DataMember(Name="password")]
		public string Password { get => _Password; set { _Password = value; PasswordSet = true; } }
		
		public bool PasswordSet = false;		

		private string _PasswordConfirmation;
		
		/// <summary>
        /// The password confirmation
        /// </summary>
        /// <value>The password confirmation</value>
        [DataMember(Name="password_confirmation")]
		public string PasswordConfirmation { get => _PasswordConfirmation; set { _PasswordConfirmation = value; PasswordConfirmationSet = true; } }
		
		public bool PasswordConfirmationSet = false;		

		private string _Code;
		
		/// <summary>
        /// The password reset code sent by e-mail to the user
        /// </summary>
        /// <value>The password reset code sent by e-mail to the user</value>
        [Required]
        [DataMember(Name="code")]
		public string Code { get => _Code; set { _Code = value; CodeSet = true; } }
		
		public bool CodeSet = false;		

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ResetUserPasswordSpec {\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  Password: ").Append(Password).Append("\n");
            sb.Append("  PasswordConfirmation: ").Append(PasswordConfirmation).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ResetUserPasswordSpec)obj);
        }

        /// <summary>
        /// Returns true if ResetUserPasswordSpec instances are equal
        /// </summary>
        /// <param name="other">Instance of ResetUserPasswordSpec to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ResetUserPasswordSpec other)
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
                    Code == other.Code ||
                    Code != null &&
                    Code.Equals(other.Code)
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
                    if (Password != null)
                    hashCode = hashCode * 59 + Password.GetHashCode();
                    if (PasswordConfirmation != null)
                    hashCode = hashCode * 59 + PasswordConfirmation.GetHashCode();
                    if (Code != null)
                    hashCode = hashCode * 59 + Code.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ResetUserPasswordSpec left, ResetUserPasswordSpec right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ResetUserPasswordSpec left, ResetUserPasswordSpec right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
