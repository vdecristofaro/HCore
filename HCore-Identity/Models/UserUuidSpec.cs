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
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HCore.Identity.Models
{ 
    /// <summary>
    /// The information required to initiate the \&quot;forgot password\&quot; flow for the user
    /// </summary>
    [DataContract]
    [NotMapped]
	public partial class UserUuidSpec : IEquatable<UserUuidSpec>
    { 
		private string _UserUuid;
		
		/// <summary>
        /// The UUID of the user
        /// </summary>
        /// <value>The UUID of the user</value>
        [Required]
        [DataMember(Name="user_uuid")]
		public string UserUuid { get => _UserUuid; set { _UserUuid = value; UserUuidSet = true; } }
		
		public bool UserUuidSet = false;		

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserUuidSpec {\n");
            sb.Append("  UserUuid: ").Append(UserUuid).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UserForgotPasswordSpec)obj);
        }

        /// <summary>
        /// Returns true if UserForgotPasswordSpec instances are equal
        /// </summary>
        /// <param name="other">Instance of UserForgotPasswordSpec to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserUuidSpec other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    UserUuid == other.UserUuid ||
                    UserUuid != null &&
                    UserUuid.Equals(other.UserUuid)
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
                    if (UserUuid != null)
                    hashCode = hashCode * 59 + UserUuid.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UserUuidSpec left, UserUuidSpec right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserUuidSpec left, UserUuidSpec right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
