//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;

//namespace MtCoffee.Web.Models.Authentication
//{
//    public class UserProfile
//    {
//        public int? UserId { get; set; }
//        public string EmailAddress { get; set; }
//        public List<string> Roles { get; set; }
//        public string FirstName { get; set; }
//        public string LastName { get; set; }

//        [Obsolete("Deprecated for now.", false)]
//        public List<int> OrganizationIds { get; set; }

//        public UserProfile()
//        {
//        }

//        public static UserProfile FromClaims(IEnumerable<Claim> claims)
//        {
//            var up = new UserProfile()
//            {
//                EmailAddress = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
//                Roles = (claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "")
//                .Split(",", StringSplitOptions.RemoveEmptyEntries).ToList(),
//                UserId = claims.FirstOrDefault(c => c.Type == Constants.CLAIM_USERID)?.Value.ToNullable<int>(),
//#pragma warning disable CS0618 // Type or member is obsolete
//                OrganizationIds = JsonConvert.DeserializeObject<List<int>>(claims.FirstOrDefault(c => c.Type == Constants.CLAIM_ORGANIZATION_ID)?.Value),
//#pragma warning restore CS0618 // Type or member is obsolete
//                FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
//                LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value
//            };

//            return up;
//        }

//        public IEnumerable<Claim> ToClaims()
//        {
//            var claims = new List<Claim>();

//            //claims.Add(new Claim(ClaimTypes.Email, this.EmailAddress));
//            //claims.Add(new Claim(ClaimTypes.Role, string.Join(",", this.Roles)));
//            //claims.Add(new Claim(ClaimTypes.Upn, this.EmailAddress));
//            claims.Add(new Claim(Constants.CLAIM_USERID, this.UserId?.ToString()));
//            //claims.Add(new Claim(ClaimTypes.GivenName, this.FirstName));
//            //claims.Add(new Claim(ClaimTypes.Surname, this.LastName));

//            return claims;
//        }
//    }
//}
